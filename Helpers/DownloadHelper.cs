using SteamWorkshopDownloader.Utilities;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SteamWorkshopDownloader.Helpers
{
    class DownloadHelper
    {
        private LoadingBar Loader = new LoadingBar();
        private TaskCompletionSource<string> Promise = new TaskCompletionSource<string>();
        public async Task DownloadFile(string url, string target, string title)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler((sender, e) => client_DownloadProgressChanged(sender, e, title));
            client.DownloadFileCompleted += new AsyncCompletedEventHandler((sender, e) => Promise.TrySetResult(e.Error.Message));
            client.DownloadFileAsync(new Uri(url), target);
            string asdf = await Promise.Task;
            var fff = "lol";
        }
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e, string title)
        {
            Loader.Update(title, SizeSuffix.calculate(e.BytesReceived, 2), SizeSuffix.calculate(e.TotalBytesToReceive, 2), double.Parse(e.BytesReceived.ToString()) / double.Parse(e.TotalBytesToReceive.ToString()) * 100);
        }
        public async Task DownloadWorkshopItem(string url, string target)
        {
            var id = "";
            var querys = new Uri(url).Query.Substring(1).Split("&");
            for (int i = 0; i < querys.Length; i++)
            {
                if (querys[i].Split("=")[0] == "id") id = querys[i].Split("=")[1];
            }
            if (id == "") return;
            dynamic infos = await HTTPClientHelper.PostJsonAsync("https://node0" + new Random().Next(1, 5) + ".steamworkshopdownloader.io/prod/api/details/file", "[" + id + "]");
            string fileName = (string)infos[0].title;
            string downloadUrl = (string)infos[0].file_url;
            if(downloadUrl == null || downloadUrl == "")
            {
                string uuid = (string) (await HTTPClientHelper.PostJsonAsync("https://node0" + new Random().Next(1, 5) + ".steamworkshopdownloader.io/prod/api/download/request", "{\"publishedFileId\":" + (string)infos[0].publishedfileid + ",\"collectionId\":null,\"hidden\":false,\"downloadFormat\":\"raw\",\"autodownload\":false}{\"publishedFileId\":377856298,\"collectionId\":null,\"hidden\":false,\"downloadFormat\":\"raw\",\"autodownload\":false}")).uuid;
                downloadUrl = "https://node0" + new Random().Next(1, 5) + ".steamworkshopdownloader.io/prod/storage/" + (string)infos[0].creator_appid + "/" + (string)infos[0].publishedfileid + "/" + (string)infos[0].time_updated + "/" + (string)infos[0].publishedfileid + "_" + (string)infos[0].title_disk_safe + ".raw.download.zip?uuid=" + uuid;
            }
            Array.ForEach(Path.GetInvalidFileNameChars(), c => fileName = fileName.Replace(c.ToString(), String.Empty));
            if (!Directory.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SteamWorkshopDownloader", "tmp"))) Directory.CreateDirectory(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SteamWorkshopDownloader", "tmp"));
            Console.WriteLine();
            await DownloadFile(downloadUrl, Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SteamWorkshopDownloader", "tmp", fileName + ".zip"), "Downloading '" + (string)infos[0].title + "'...");
            Functions.TypeWriter("Unzipping file...");
            System.IO.Compression.ZipFile.ExtractToDirectory(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SteamWorkshopDownloader", "tmp", fileName + ".zip"), target);
            Functions.TypeWriter("Done!");
        }
    }
}
