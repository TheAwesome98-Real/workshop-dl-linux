using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace SteamWorkshopDownloader
{
    class DiscordRPHelper
    {
        private DiscordRpcClient client;
        public DiscordRPHelper()
        {
            client = new DiscordRpcClient("945401698401284116");
            client.Initialize();
        }
        public void SetRP(string text, string small_text, string small_image)
        {
            client.SetPresence(new RichPresence()
            {
                State = text,
                Assets = new Assets()
                {
                    LargeImageKey = "bridge",
                    SmallImageKey = small_image,
                    SmallImageText = small_text
                },
                Buttons = new Button[]
                {
                    new Button() { Label = "Download tool", Url = "https://github.com/Official-Husko/Husko-s-SteamWorkshop-Downloader" },
                    new Button() { Label = "The \"other dude\"", Url = "https://github.com/h110m" }
                }
            });
        }
    }
}
