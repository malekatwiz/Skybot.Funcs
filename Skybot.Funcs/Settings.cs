using System;

namespace Skybot.Funcs
{
    public static class Settings
    {
        public static string SecretKey => GetEnvironmentVariable("SecretKey");
        public static string SkybotAuthClientId => GetEnvironmentVariable("AuthClientId");
        public static string SkybotAuthClientSecret => GetEnvironmentVariable("AuthClientSecret");
        public static string SkybotAuthUri => GetEnvironmentVariable("AuthUri");
        public static string SkybotAccountsUri => GetEnvironmentVariable("AccountsUri");
        public static string SkybotTextoUri => GetEnvironmentVariable("SkybotTextoUri");
        public static string ServiceBusConnectionStringWithSend => GetEnvironmentVariable("ServiceBusConnectionStringSend");
        public static string ServiceBusConnectionStringWithListen => GetEnvironmentVariable("ServiceBusConnectionStringListen");

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
