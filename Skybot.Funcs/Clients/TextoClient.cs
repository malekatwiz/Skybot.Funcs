using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Skybot.Funcs.Models;

namespace Skybot.Funcs.Clients
{
    public class TextoClient
    {
        private readonly HttpClient _httpClient;
        private string _token;

        public TextoClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task Send(TextMessage message)
        {
            await CheckToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

            var content = new
            {
                ToNumber = message.Account.PhoneNumber,
                Message = message.Body
            };
            await _httpClient.PostAsJsonAsync($"{Settings.SkybotTextoUri}/api/text/send", content);
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
