using System.Text;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Skybot.Funcs.Models;

namespace Skybot.Funcs.Extensions
{
    public static class MessageExtensions
    {
        public static byte[] GetBytes(this UserAccount userAccount)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userAccount));
        }

        public static byte[] GetBytes(this TextMessage textMessage)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(textMessage));
        }

        public static T Convert<T>(this Message message)
        {
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
        }
    }
}
