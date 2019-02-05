using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Skybot.Funcs.Extensions;
using Skybot.Funcs.Models;
using Twilio.AspNet.Core;

namespace Skybot.Funcs
{
    public static class AdmissionFunc
    {
        private static readonly AccountsClient AccountsClient;
        private static readonly QueueClient NewAccountsQueueClinet;
        private static readonly QueueClient ExistingAccountsQueueClient;

        static AdmissionFunc()
        {
            AccountsClient = new AccountsClient();
            NewAccountsQueueClinet = new QueueClient(Settings.ServiceBusConnectionStringWithSend, "newaccounts");
            ExistingAccountsQueueClient = new QueueClient(Settings.ServiceBusConnectionStringWithSend, "incomingquery");
        }

        [FunctionName("AdmissionFunc")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
            ILogger log)
        {
            log.LogInformation($"AdmissionFunc| SMS received..");
            if (RequestIsAuthorized(request))
            {
                if (request.Form.TryGetValue("From", out var phoneNumber))
                {
                    if (await AccountsClient.HasAccount(phoneNumber))
                    {
                        log.LogInformation($"AdmissionFunc| Sending request to create new account..");
                        await NewAccountsQueueClinet.SendAsync(new Message
                        {
                            To = "NewAccounts",
                            Body = new UserAccount {PhoneNumber = phoneNumber}.GetBytes()
                        });
                    }
                    else
                    {
                        await PassQuery(phoneNumber, log);
                    }

                    return new TwiMLResult();
                }

                log.LogInformation($"AdmissionFunc| Sender number is not available.");
                return new BadRequestResult();
            }

            log.LogInformation($"AdmissionFunc| incoming SMS request is not authorized.");
            return new UnauthorizedResult();
        }

        private static bool RequestIsAuthorized(HttpRequest request)
        {
            return Settings.SecretKey.Equals(request.Query["key"], StringComparison.InvariantCulture);
        }

        private static async Task PassQuery(string phoneNumber, ILogger log)
        {
            var userAccount = await AccountsClient.GetAccount(phoneNumber);
            log.LogInformation(
                $"AdmissionFunc| Sending request to process query of '{userAccount.Name} / {userAccount.PhoneNumber}'");

            await ExistingAccountsQueueClient.SendAsync(new Message
            {
                To = "ExistingAccounts",
                Body = userAccount.GetBytes()
            });
        }
    }
}
