using System;

namespace EasyJobInfraCode.Core.Preprocessor
{
    public static class Constants
    {
        public static string APP_EXECUTABLE_PATH = AppDomain.CurrentDomain.BaseDirectory;
        public static string APP_TEMP_PATH = AppDomain.CurrentDomain.BaseDirectory + "temp\\";
        public static string ENV_SYSTEM_DIRECTORY = Environment.SystemDirectory;
        public static string ENV_USER_DOMAIN_NAME = Environment.UserDomainName;
        public static string ENV_USERNAME = Environment.UserName;
        public static string ENV_MACHINENAME = Environment.MachineName;
    }
}
