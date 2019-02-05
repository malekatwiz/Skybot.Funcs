using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Skybot.Funcs.Clients
{
    public class SkybotAppClient : BaseClient
    {
        public async Task<string> SubmitQuery(string query)
        {
            await RequestAndAssignToken();

            var response = await HttpClient.PostAsJsonAsync($"{Settings.SkybotAppUri}/api/skybot/process",
                new {Query = query});

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(responseContent);
        }
    }
}
