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
        private string searchChannelUrl = "https://www.googleapis.com/youtube/v3/search";
        private string searchPlaylistUrl = "https://www.googleapis.com/youtube/v3/playlistItems";
        private string part = "snippet";
        int maxResultValue = 50;
        public int maxResult
        {
            get { return this.maxResultValue; }
            set { this.maxResultValue = value; }
        }
        public string[] searchChannel(string channelId, string key)
        {
            string url = searchChannelUrl + "?"
                         + "part=" + part
                         + "&channelId=" + channelId
                         + "&maxResults=" + maxResult
                         + "&key=" + key;
            ArrayList videoList = new ArrayList();
            string nextPageToken = "";
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
                    }
                } while (nextPageToken != "");
                string[] list = (string[])videoList.ToArray(typeof(string));
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string[] searchPlaylist(string playListId, string key)
        {
            string url = searchPlaylistUrl + "?"
                         + "part=" + part
                         + "&playlistId=" + playListId
                         + "&maxResults=" + maxResult
                         + "&key=" + key;
            ArrayList videoList = new ArrayList();
            string nextPageToken = "";
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
                    IEnumerable<JToken> idList = json.SelectTokens("$..resourceId.videoId");
                    foreach (JToken iid in idList)
                    {
                        videoList.Add(iid.ToString());
                    }
                } while (nextPageToken != "");
                string[] list = (string[])videoList.ToArray(typeof(string));
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
