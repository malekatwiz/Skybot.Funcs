using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Skybot.Funcs.Models;

namespace Skybot.Funcs
{
    public class AccountsClient
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public AccountsClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<UserAccount> GetAccount(string phoneNumber)
        {
            await CheckToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var response = await _httpClient.GetAsync($"{Settings.SkybotAccountsUri}/api/accounts/{phoneNumber}");
            if (response.StatusCode.Equals(HttpStatusCode.Found))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserAccount>(responseContent);
            }

            return new UserAccount();
        }

        public async Task<bool> HasAccount(string phoneNumber)
        {
            await CheckToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var response = await _httpClient.GetAsync($"{Settings.SkybotAccountsUri}/api/accounts/check/{phoneNumber}");
            return response.StatusCode.Equals(HttpStatusCode.Found);
        }

        private async Task CheckToken()
        {
            if (string.IsNullOrEmpty(_token))
            {
                await RequestToken();
            }
        }

        private async Task RequestToken()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"client_id", Settings.SkybotAuthClientId },
                {"client_secret", Settings.SkybotAuthClientSecret },
                {"grant_type", "client_credentials" }
            });

            var response = await _httpClient.PostAsync($"{Settings.SkybotAuthUri}/connect/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedContent = JsonConvert.DeserializeObject<dynamic>(responseContent);

            _token = deserializedContent.access_token;
        }
    }
}
