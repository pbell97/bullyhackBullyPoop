using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bullyPoop2.Droid.Resources
{
    public class WebRequestHelper
    {
        private string URL;

        public WebRequestHelper(string URL)
        {
            this.URL = URL;
        }

        public string getRequest(string endpoint)
        {
            if (!endpoint.StartsWith("/"))
            {
                endpoint = "/" + endpoint;
            }

            Uri url = new Uri(URL + endpoint);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(  URL + endpoint);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            string result;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            Console.WriteLine(result);
            return result;
        }

        public void postRequest(string endpoint, string serialized)
        {
            if (!endpoint.StartsWith("/"))
            {
                endpoint = "/" + endpoint;
            }
            byte[] dataBytes = Encoding.UTF8.GetBytes(serialized);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + endpoint);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "string";
            request.Method = "POST";

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

        }
    }
}
