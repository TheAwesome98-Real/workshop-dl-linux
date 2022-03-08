using SteamWorkshopDownloader.Utilities;
using System;

namespace SteamWorkshopDownloader.Helpers
{
    class MenuHelper
    {
        public static string Generate(string Title, string[] Options, int selectedIndex = 0, int ConsoleY = -1)
        {
            if (ConsoleY == -1)
            {
                Functions.TypeWriter(Title);
                ConsoleY = Console.CursorTop + 1;
            }
            Console.SetCursorPosition(0, ConsoleY);
            for (int i = 0; i < Options.Length; i++)
            {
                Functions.ClearCurrentConsoleLine();
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("  > ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(Options[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("  > " + Options[i]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.DownArrow:
                    if (selectedIndex != Options.Length - 1) return Generate(Title, Options, selectedIndex + 1, ConsoleY);
                    return Generate(Title, Options, selectedIndex, ConsoleY);
                case ConsoleKey.UpArrow:
                    if (selectedIndex != 0) return Generate(Title, Options, selectedIndex - 1, ConsoleY);
                    return Generate(Title, Options, selectedIndex, ConsoleY);
                case ConsoleKey.Enter:
                    Console.SetCursorPosition(0, ConsoleY - 1);
                    Functions.ClearCurrentConsoleLine();
                    Console.WriteLine(">> " + Options[selectedIndex]);
                    for (int i = 0; i < Options.Length; i++)
                    {
                        Console.SetCursorPosition(0, ConsoleY + i);
                        Functions.ClearCurrentConsoleLine();
                    }
                    Console.SetCursorPosition(0, ConsoleY);
                    return Options[selectedIndex];
                default:
                    return Generate(Title, Options, selectedIndex, ConsoleY);
            }
        }
        public static string GenerateWithValue(string Title, string[][] Options, int selectedIndex = 0, int ConsoleY = -1)
        {
            if (ConsoleY == -1)
            {
                Functions.TypeWriter(Title);
                ConsoleY = Console.CursorTop + 1;
            }
            Console.SetCursorPosition(0, ConsoleY);
            for (int i = 0; i < Options.Length; i++)
            {
                Functions.ClearCurrentConsoleLine();
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("  > ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(Options[i][0]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" (" + Options[i][1] + ")");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  > " + Options[i][0]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" (" + Options[i][1] + ")");
                }
            }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.DownArrow:
                    if (selectedIndex != Options.Length - 1) return GenerateWithValue(Title, Options, selectedIndex + 1, ConsoleY);
                    return GenerateWithValue(Title, Options, selectedIndex, ConsoleY);
                case ConsoleKey.UpArrow:
                    if (selectedIndex != 0) return GenerateWithValue(Title, Options, selectedIndex - 1, ConsoleY);
                    return GenerateWithValue(Title, Options, selectedIndex, ConsoleY);
                case ConsoleKey.Enter:
                    Console.SetCursorPosition(0, ConsoleY - 1);
                    Functions.ClearCurrentConsoleLine();
                    Console.WriteLine(">> " + Options[selectedIndex][0]);
                    for (int i = 0; i < Options.Length; i++)
                    {
                        Console.SetCursorPosition(0, ConsoleY + i);
                        Functions.ClearCurrentConsoleLine();
                    }
                    Console.SetCursorPosition(0, ConsoleY);
                    return Options[selectedIndex][0];
                default:
                    return GenerateWithValue(Title, Options, selectedIndex, ConsoleY);
            }
        }
    }
}
