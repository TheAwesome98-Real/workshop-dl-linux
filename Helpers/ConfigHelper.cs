using Newtonsoft.Json;
using SteamWorkshopDownloader.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SteamWorkshopDownloader.Helpers
{
    class Config
    {
        public List<string[]> ModPaths { get; set; }
        public bool Proxies { get; set; }
        public int RequestTimeout { get; set; }
    }

    class ConfigHelper
    {
        public static Config config;
        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static void SaveConfig()
        {
            File.WriteAllText(Path.Join(AppDataPath, "SteamWorkshopDownloader", "config.json"), JsonConvert.SerializeObject(config, Formatting.Indented));
        }

        public static void GetConfig()
        {
            if (!Directory.Exists(Path.Join(AppDataPath, "SteamWorkshopDownloader"))) Directory.CreateDirectory(Path.Join(AppDataPath, "SteamWorkshopDownloader"));
            if (!File.Exists(Path.Join(AppDataPath, "SteamWorkshopDownloader", "config.json")))
            {
                Functions.TypeWriter("Hello Friend :3");
                Functions.TypeWriter("Looks like this is your first time...");
                Functions.TypeWriter("Let me just setup some things for you :) Brb");
                config = new Config
                {
                    ModPaths = new List<string[]>(),
                    Proxies = false,
                    RequestTimeout = 10
                };
                SaveConfig();
            }
            else
            {
                Functions.TypeWriter("Loading config...");
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Join(AppDataPath, "SteamWorkshopDownloader", "config.json")));
            }
            Functions.TypeWriter("Done!");
        }
        public static void Configure()
        {
            var objIndex = MenuHelper.GenerateWithValue("What do you want to change?", new string[][] {
                new string[]
                {
                    "Use proxy",
                    config.Proxies ? "true" : "false"
                },
                new string[]
                {
                    "Request timeout",
                    config.RequestTimeout.ToString()
                }
            });
            switch (objIndex)
            {
                case "Use proxy":
                    config.Proxies = Input.Generate<bool>("Input new value for use proxy");
                    break;
                case "Request timeout":
                    config.RequestTimeout = Input.Generate<int>("Input new value for Request timeout");
                    break;
            }
            SaveConfig();
            Functions.ClearConsole();
        }

        public static string GetModsPath()
        {
            if (config.ModPaths.Count == 0)
            {
                Functions.TypeWriter("Looks like you dont have a Gamepath...");
                Functions.TypeWriter("Let's create one ^^");
                config.ModPaths.Add(new string[] {
                    Input.Generate<string>("Input the Games name (will be used as name in the list)"),
                    Input.Generate<string>("Perfect. Now input the path to your games folder (will be used to put in the downloaded content)")
                });
                SaveConfig();
                Functions.ClearConsole();
            }
            List<string> options = config.ModPaths.Select(path => path[0]).ToList();
            options.Add("New Gamepath");
            string selectedOption = MenuHelper.Generate("Select a option", options.ToArray());
            if (selectedOption == "New Gamepath")
            {
                config.ModPaths.Add(new string[] {
                    Input.Generate<string>("Input the Games name (will be used as name in the list)"),
                    Input.Generate<string>("Perfect. Now input the path to your games folder (will be used to put in the downloaded content)")
                });
                SaveConfig();
                Functions.ClearConsole();
                return GetModsPath();
            }
            Functions.ClearConsole();
            return config.ModPaths.First(x => x[0] == selectedOption)[1];
        }
    }
}
