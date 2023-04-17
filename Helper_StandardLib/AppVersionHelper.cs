using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace VsscHelper_CoreLibrary
{
    public static class AppVersionHelper
    {
        public static string CurrentRevision()
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version.ToString();
            }
            catch
            {
                return "?.?.?.?";
            }
        }
    }
}