using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using SteamWorkshopDownloader.Helpers;
using SteamWorkshopDownloader.Utilities;

namespace SteamWorkshopDownloader
{
    class Program
    {
        static readonly string CurrentVersion = ConfigurationManager.AppSettings.Get("CurrentVersion");
        static readonly string PreviousVersion = ConfigurationManager.AppSettings.Get("PreviousVersion");
        static readonly string BaseDir = Directory.GetCurrentDirectory();

        static DiscordRPHelper DRPClient = new DiscordRPHelper();
        static Functions Utils = new Functions(CurrentVersion, PreviousVersion, BaseDir);

        static async Task Main(string[] args)
        {
            Utils.UpdateCleanup();
            Utils.InitLogo();
            Console.WriteLine();
            if (await Utils.CheckForUpdates(DRPClient)) return;
            Functions.ClearConsole();
            ConfigHelper.GetConfig();
            while (true)
            {
                Functions.ClearConsole();
                string WhatToDo = MenuHelper.Generate("Select what you wanna do...", new string[] {
                    "Change config",
                    "Download a workshop item",
                    "Exit"
                });
                if (WhatToDo == "Exit") break;
                Functions.ClearConsole();
                switch (WhatToDo)
                {
                    case "Change config":
                        ConfigHelper.Configure();
                        break;
                    case "Download a workshop item":
                        string path = ConfigHelper.GetModsPath();
                        await new DownloadHelper().DownloadWorkshopItem(Input.Generate<string>("Input a workshop url"), path);
                        break;
                }
            }
            return;
        }
    }
}
