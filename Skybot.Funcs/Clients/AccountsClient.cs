using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Skybot.Funcs.Models;

namespace Skybot.Funcs.Clients
{
    public class AccountsClient : BaseClient
    {
        public async Task<UserAccount> GetAccount(string phoneNumber)
        {
            await RequestAndAssignToken();

            var response = await HttpClient.GetAsync($"{Settings.SkybotAccountsUri}/api/accounts/{phoneNumber}");
            if (response.StatusCode.Equals(HttpStatusCode.Found))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserAccount>(responseContent);
            }

            return new UserAccount();
        }

        public async Task<bool> HasAccount(string phoneNumber)
        {
            await RequestAndAssignToken();

            var response = await HttpClient.GetAsync($"{Settings.SkybotAccountsUri}/api/accounts/check/{phoneNumber}");
            return response.StatusCode.Equals(HttpStatusCode.Found);
        }
    }
}
