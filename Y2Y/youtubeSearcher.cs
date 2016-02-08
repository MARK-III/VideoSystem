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
    class youtubeSearcher
    {
        private string baseUrl = "https://www.googleapis.com/youtube/v3/search";
        private string part = "snippet";
        int maxResultValue = 50;
        public int maxResult
        {
            get { return this.maxResultValue; }
            set { this.maxResultValue = value; }
        }
        public string[] searchChannel(string channelId, string key)
        {
            string url = baseUrl + "?"
                         + "part=" + part
                         + "&channelId=" + channelId
                         + "&maxResults=" + maxResult
                         + "&key=" + key;
            ArrayList videoList = new ArrayList();
            string nextPageToken = "";
            StreamWriter videoFile = new StreamWriter("C:/Data/BZINGGA/Y2Y/video_list.txt");
            try
            {
                do
                {
                    WebRequest request = WebRequest.Create(url + "&pageToken=" + nextPageToken);
                    request.Method = "GET";
                    WebResponse response = request.GetResponse();
                    StreamReader jsonStream = new StreamReader(response.GetResponseStream());
                    string jsonString = jsonStream.ReadToEnd();
                    response.Close();
                    JObject json = JObject.Parse(jsonString);
                    if (json["nextPageToken"] == null)
                    {
                        nextPageToken = "";
                    }
                    else
                    {
                        nextPageToken = (string)json["nextPageToken"];
                    }
                    IEnumerable<JToken> idList = json.SelectTokens("$..id.videoId");
                    foreach (JToken iid in idList)
                    {
                        videoList.Add(iid.ToString());
                        videoFile.WriteLine(iid.ToString());
                    }
                } while (nextPageToken != "");
                string[] list = (string[])videoList.ToArray(typeof(string));
                videoFile.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
