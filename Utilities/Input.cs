using SteamWorkshopDownloader.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteamWorkshopDownloader.Utilities
{
    class Input
    {
        public static dynamic Generate<T>(string title)
        {
            if (typeof(T) == typeof(bool))
            {
                string res = MenuHelper.Generate(title, new string[] {
                    "true",
                    "false"
                });
                return res == "true";
            }
            else if (typeof(T) == typeof(string))
            {
                Functions.TypeWriter(title);
                Console.WriteLine();
                Console.Write("> ");
                return Console.ReadLine();
            }
            else if (typeof(T) == typeof(int))
            {
                Functions.TypeWriter(title);
                Console.WriteLine();
                Console.Write("> ");
                return int.Parse(Console.ReadLine());
            }
            else return false;
        }
    }
}
