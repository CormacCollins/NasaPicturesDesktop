using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp2
{

    class RestRequest
    {
        private static HttpWebRequest request;
        public RestRequest() {
        }

        /// <summary>
        /// Returns Stream from url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Stream GetReqImage(string url) {

            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            //Get stream and read JSON response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            var res = sr.ReadToEnd();

            //Convert result to JSON and get image url 
            JObject json = JObject.Parse(res);
            Console.WriteLine(json);
            string s = json["url"].ToString();

            //Request image for download 
            request = (HttpWebRequest)WebRequest.Create(s);
            response = (HttpWebResponse)request.GetResponse();
            Stream str = response.GetResponseStream();
            return str;
        }

        /// <summary>
        /// Returns image from Get Request from specified url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Image GetRequestImage(string url) {
            Stream str = GetReqImage(url);
            return Image.FromStream(str);
        }
    }
}
