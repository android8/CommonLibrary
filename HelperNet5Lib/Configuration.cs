using System.Configuration;
using System.Collections.Specialized;

namespace VsscHelper_CoreLibrary
{
  public class Configuration
    {
        public static string getAppSetting(string getKey)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            string[] keys = appSettings.AllKeys;

            return appSettings.Get(getKey);

        }
    }
}
