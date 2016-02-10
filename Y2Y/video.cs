using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Y2Y
{
    class video
    {
        private string id;
        private string name;
        public video(string videoId)
        {
            id = videoId;
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
        }
        private string linkBase = "https://www.youtube.com/watch?v=";
        int maxRetryValue = 2;
        public int maxRetry
        {
            get { return this.maxRetryValue; }
            set { this.maxRetryValue = value; }
        }
        public void download()
        {  
            string link = linkBase + id;
            IEnumerable<YoutubeExtractor.VideoInfo> videoInfos = YoutubeExtractor.DownloadUrlResolver.GetDownloadUrls(link);
            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            name = video.Title.Trim(new Char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' });
            var videoDownloader = new VideoDownloader(video, Path.Combine(config.workDirectory, name + video.VideoExtension));
            //videoDownloader.DownloadProgressChanged += (sender, args) => Console.WriteLine(args.ProgressPercentage);
            try
            {
                Console.WriteLine("Trying to download: " + name);
                videoDownloader.Execute();
                StreamWriter file = new StreamWriter(Path.Combine(config.workDirectory, "video_download.txt"), true);
                file.WriteLine(id);
                file.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                if (maxRetry > 0)
                {
                    maxRetry--;
                    this.download();
                }
                else
                {
                    Console.WriteLine("*Video download failed:" + id);
                }
            }
        }
    }
}
