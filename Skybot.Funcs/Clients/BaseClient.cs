using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Skybot.Funcs.Clients
{
    public class BaseClient
    {
        protected readonly HttpClient HttpClient;
        private string _token;

        public BaseClient()
        {
            HttpClient = new HttpClient();
        }

        protected virtual async Task RequestAndAssignToken()
        {
            if (string.IsNullOrEmpty(_token))
            {
                await RequestToken();
            }

            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
        }

        private async Task RequestToken()
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"client_id", Settings.SkybotAuthClientId },
                {"client_secret", Settings.SkybotAuthClientSecret },
                {"grant_type", "client_credentials" }
            });

            var response = await HttpClient.PostAsync($"{Settings.SkybotAuthUri}/connect/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedContent = JsonConvert.DeserializeObject<dynamic>(responseContent);

            _token = deserializedContent.access_token;
        }
    }
}
