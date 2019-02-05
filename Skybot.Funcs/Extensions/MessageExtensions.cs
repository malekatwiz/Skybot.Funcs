using System.Text;
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
    }
}
