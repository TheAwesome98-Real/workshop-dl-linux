using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace SteamWorkshopDownloader.Utilities
{
    class Version
    {
        public static string GetCurrent()
        {
            return ConfigurationManager.AppSettings.Get("currentVersion");
        }
        public static string GetPrevious()
        {
            return ConfigurationManager.AppSettings.Get("previousVersion");
        }
        public static int Parse(string[] verArr)
        {
            int verNum = 0;
            for (int i = 0; i < verArr.Length; i++)
            {
                verNum += int.Parse(verArr[i]);
            }
            return verNum;
        }
    }
}
