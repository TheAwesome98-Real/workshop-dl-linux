using Newtonsoft.Json.Linq;
using SteamWorkshopDownloader.Helpers;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SteamWorkshopDownloader.Utilities
{
    class Functions
    {
        private readonly string BaseDir;
        private readonly string CurrentVersion;
        private readonly string PreviousVersion;
        private readonly HttpClient HTTPClient = new HttpClient();

        public Functions(string currentVersion, string previousVersion, string baseDir)
        {
            BaseDir = baseDir;
            CurrentVersion = currentVersion;
            PreviousVersion = previousVersion;
        }

        public string GetDownloadPrefix()
        {
            string prefix = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) prefix += "win-";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) prefix += "linux-";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) prefix += "osx-";
            prefix += RuntimeInformation.OSArchitecture.ToString().ToLower() + "-";
            return prefix;
        }

        public void UpdateCleanup()
        {
            if (File.Exists(Path.Join(BaseDir, GetDownloadPrefix() + "SteamWorkshopDownloader.exe"))) File.Delete(Path.Join(BaseDir, GetDownloadPrefix() + "SteamWorkshopDownloader.exe"));
#if !DEBUG
            try
            {
                if (File.Exists(Path.Join(BaseDir, "SteamWorkshopDownloader.exe"))) File.Delete(Path.Join(BaseDir, "SteamWorkshopDownloader.exe"));
            }
            catch { }
#endif
            if (File.Exists(Path.Join(BaseDir, "SteamWorkshopDownloader-" + PreviousVersion + ".exe"))) File.Delete(Path.Join(BaseDir, "SteamWorkshopDownloader-" + PreviousVersion + ".exe"));
        }

        public static void TypeWriter(string txt, bool newLine = true)
        {
            if(newLine) Console.WriteLine();
            for (int i = 0; i < txt.Length + 1; i++)
            {
                Console.Write("\r> " + txt.Substring(0, i));
                System.Threading.Thread.Sleep(30);
            }
        }

        public void InitLogo()
        {
            Console.CursorVisible = false;
            Console.WriteLine("\n ______     __     __     _____     __        \n/\\  ___\\   /\\ \\  _ \\ \\   /\\  __-.  /\\ \\       \n\\ \\___  \\  \\ \\ \\/ \".\\ \\  \\ \\ \\/\\ \\ \\ \\ \\____  \n \\/\\_____\\  \\ \\__/\".~\\_\\  \\ \\____-  \\ \\_____\\ \n  \\/_____/   \\/_/   \\/_/   \\/____/   \\/_____/ \n");
            System.Threading.Thread.Sleep(1000);
            TypeWriter("SteamWorkshopDownloader v" + CurrentVersion);
            TypeWriter("BY HUSKO & H110M");
            System.Threading.Thread.Sleep(1000);
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void ClearConsole()
        {
            int currentPosition = Console.CursorTop + 1;
            for (int i = 0; i < currentPosition; i++)
            {
                if (i < 10) continue;
                Console.SetCursorPosition(0, i);
                ClearCurrentConsoleLine();
            }
            Console.SetCursorPosition(0, 10);
        }

        public async Task<bool> CheckForUpdates(DiscordRPHelper DRPClient)
        {
            DRPClient.SetRP("Checking for Updates...", "Updating", "update");
            TypeWriter("Checking for Updates...");
            
            dynamic latestVersionObj = (await HTTPClientHelper.GetJsonAsync(ConfigurationManager.AppSettings.Get("GithubAPI_Releases")))[0];
            string latestVersion = (String) latestVersionObj.tag_name;
            
            if (Version.Parse((latestVersion).Split(".")) > Version.Parse(CurrentVersion.Split(".")))
            {
                DRPClient.SetRP("Updating to " + latestVersion + "...", "Updating", "update");
                TypeWriter("New version: v" + latestVersion);

                string sourceURI = (string) ((dynamic) new JArray(((JArray)latestVersionObj.assets).Where(item => item.Value<string>("name").StartsWith(GetDownloadPrefix() + "SteamWorkshopDownloader")))[0]).browser_download_url;
                string targetURI = Path.Join(BaseDir, "SteamWorkshopDownloader-" + latestVersion + ".exe");

                await new DownloadHelper().DownloadFile(sourceURI, targetURI, "Downloading Update " + latestVersion);
                TypeWriter("Restarting...");

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Path.Join(BaseDir, "SteamWorkshopDownloader-" + (String)latestVersionObj.tag_name + ".exe");
                startInfo.UseShellExecute = true;
                startInfo.CreateNoWindow = false;
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(startInfo);
                Environment.Exit(0);

                return true;
            }
            TypeWriter("Your up to date ^^");
            System.Threading.Thread.Sleep(1000);
            return false;
        }

        public void InitialiseApp()
        {

        }
    }
}
