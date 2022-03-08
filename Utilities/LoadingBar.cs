using System;
using System.Collections.Generic;
using System.Text;

namespace SteamWorkshopDownloader.Utilities
{
    class LoadingBar
    {
        private readonly char Bar = '\u2593';
        private readonly string[] LoaderFrames = { "\u2500", "\\", "|", "/" };
        private int LoaderIndex = 0;
        private int SlowDown = 0;
        private string OffsetLoader()
        {
            string Loader = LoaderFrames[LoaderIndex];
            SlowDown++;
            if (SlowDown >= 15)
            {
                SlowDown = 0;
                LoaderIndex++;
            }
            if (LoaderIndex >= Loader.Length) LoaderIndex = 0;
            return Loader;
        }
        public void Update(string title, string total, string current, double percentage)
        {
            Console.Write("\r[" + new string(Bar, (int)(0.25 * percentage)) + new string(' ', 25 - (int)(0.25 * percentage)) + "]  " + OffsetLoader() + "  " + title + "  " + current + "/" + total + "  " + (int)percentage + "%");
        }
    }
}
