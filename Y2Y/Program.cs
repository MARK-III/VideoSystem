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
    public static class config
    {
        public static string workDirectory = "d:/视频";
        public static string channelId = "UCUBmdNqlY5YGSPIDpRAz3WA";
        public static string playlistId = "PLXs35j9aQ_b7J6eYxIeH9OgiICVTmePEi";
        public static string key = "AIzaSyBQDGJGC1R28k4Bj7UUi2dKanJqJ0g4IYo";
        public static int maxThreads = 3;
    }
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader downloadFile = new StreamReader(Path.Combine(config.workDirectory, "video_download.txt"));
            ArrayList downloadList = new ArrayList();
            string line;
            while ((line = downloadFile.ReadLine()) != null)
            {
                downloadList.Add(line);
            }
            downloadFile.Close();

            youtubeSearcher alpha = new youtubeSearcher();
            string[] idList = alpha.searchPlaylist(config.playlistId, config.key);

            Parallel.ForEach(idList, new ParallelOptions { MaxDegreeOfParallelism = config.maxThreads }, (id) =>
            {
                if (!( downloadList.Contains(id)))
                {
                    video mine = new video(id);
                    mine.download();
                }
            });
        }
    }
}
