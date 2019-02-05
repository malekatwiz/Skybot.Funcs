using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Skybot.Funcs.Clients;
using Skybot.Funcs.Extensions;
using Skybot.Funcs.Models;

namespace Skybot.Funcs
{
    public static class MessageProcessorFunction
    {
        private static readonly SkybotAppClient SkybotAppClient;
        private static readonly QueueClient OutgoingMessageQueueClient;

        static MessageProcessorFunction()
        {
            SkybotAppClient = new SkybotAppClient();
            OutgoingMessageQueueClient = new QueueClient(Settings.ServiceBusConnectionStringWithSend, "outgoingtext");
        }

        [FunctionName("MessageProcessorFunc")]
        public static async void Run([ServiceBusTrigger("incomingquery", Connection = "ServiceBusConnectionStringListen")]Message message, ILogger log)
        {
            var textMessage = message.Convert<TextMessage>();
            log.LogInformation($"MessageProcessorFunc| Processing query '{textMessage.Body}' from: '{textMessage.Account.PhoneNumber}' ..");

            var queryResult = await SkybotAppClient.SubmitQuery(textMessage.Body);
            if (!string.IsNullOrEmpty(queryResult))
            {
                log.LogInformation($"MessageProcessorFunc| Passing query result to outgoing queue..");

                textMessage.Body = queryResult;
                await OutgoingMessageQueueClient.SendAsync(new Message
                {
                    To = "outgoingtext",
                    Body = textMessage.GetBytes()
                });
            }
        }
    }
}
