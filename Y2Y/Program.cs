using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExtractor;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Y2Y
{
    class Program
    {
        static void Main(string[] args)
        {
            string channelId = "UCpbB8LHOqFkHU3C22r_BJLQ";
            string key = "AIzaSyBQDGJGC1R28k4Bj7UUi2dKanJqJ0g4IYo";
            youtubeSearcher alpha = new youtubeSearcher();
            string[] idList = alpha.searchChannel(channelId, key);
            Parallel.ForEach(idList, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (id1) =>
            {
                video LingLing = new video(id1);
                LingLing.download();

                StreamWriter file = new StreamWriter("C:/Data/BZINGGA/Y2Y/video_download.txt",true);
                file.WriteLine(id1 + "|" + LingLing.Name);
                file.Close();
            });
        }
    }
}
