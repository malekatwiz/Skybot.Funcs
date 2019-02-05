using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Skybot.Funcs
{
    public static class MessageProcessorFunction
    {
        [FunctionName("MessageProcessorFunc")]
        public static void Run([ServiceBusTrigger("incomingquery", Connection = "ServiceBusConnectionStringListen")]string item, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {item}");
        }
    }
}
