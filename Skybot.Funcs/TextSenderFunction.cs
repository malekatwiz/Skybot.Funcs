using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Skybot.Funcs.Clients;
using Skybot.Funcs.Extensions;
using Skybot.Funcs.Models;

namespace Skybot.Funcs
{
    public static class TextSenderFunction
    {
        private static readonly TextoClient TextoClient;

        static TextSenderFunction()
        {
            TextoClient = new TextoClient();
        }

        [FunctionName("TextSenderFunc")]
        public static async void Run([ServiceBusTrigger("outgoingtext", Connection = "")]
            Message message, ILogger log)
        {
            var textMessage = message.Convert<TextMessage>();
            log.LogInformation($"TextSenderFunc| Sending text message: '{textMessage.Body}' to: {textMessage.Account.PhoneNumber}..");

            await TextoClient.Send(textMessage);
        }
    }
}
