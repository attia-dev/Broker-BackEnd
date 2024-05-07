using Broker.Debugging;

namespace Broker
{
    public class BrokerConsts
    {
        public const string LocalizationSourceName = "Broker";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;

        public static class RequestHeaders
        {
            public const string Language = "LanguageCode";
            public const string RegistrationToken = "RegistrationToken";
            public const string OperatingSystemType = "OperatingSystemType";
            public const string OperatingSystemVersion = "OperatingSystemVersion";
        }
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "e0740cf210c54990a96615676881f9b0";
    }
}
