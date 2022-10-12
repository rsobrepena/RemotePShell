using System;
using System.Configuration;

namespace RemotePShell
{
    internal static class Globals
    {
        public static string ServerAddress = FetchGlobalConfig("ServerAddress");

        //Username used to login to the PS server
        public static readonly string ServerUsername = FetchGlobalConfig("ServerUsername");

        //password used to login to PS server
        public static readonly string ServerPassword = FetchGlobalConfig("ServerPassword");

        public static readonly string LogFolder = FetchGlobalConfig("LogFolder");

        public static string FetchGlobalConfig(string key)
        {
            // Fetches the configuration 'key' from the App.Config settings file

            string result;

            try
            {
                var globalConfigs = ConfigurationManager.AppSettings;
                result = globalConfigs[key] ?? "__Setting_Not_Found__";
                Console.WriteLine("CONFIG LOADED - key: " + result);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                result = "__Setting_Not_Found__";
            }
            return result;
        }
    }
}