using System.Net.Http;
using System.Threading.Tasks;
using Skybot.Funcs.Models;

namespace Skybot.Funcs.Clients
{
    public class TextoClient : BaseClient
    {
        public async Task Send(TextMessage message)
        {
            await RequestAndAssignToken();
            
            var content = new
            {
                ToNumber = message.Account.PhoneNumber,
                Message = message.Body
            };
            await HttpClient.PostAsJsonAsync($"{Settings.SkybotTextoUri}/api/text/send", content);
        }
    }
}
