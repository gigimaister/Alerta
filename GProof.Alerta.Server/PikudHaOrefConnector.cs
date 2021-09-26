using System;
using System.IO;
using System.Net;

namespace GProof.Alerta.Server
{
    public class PikudHaOrefConnector
    {
        public string RetrieveAlert()
        {
            try
            {
                WebRequest pikodHaOrefClient = GetPikodHaOrefClient();
                using Stream s = pikodHaOrefClient.GetResponse().GetResponseStream();
                using StreamReader sr = new StreamReader(s);
                return sr.ReadToEnd();
            }
            catch (Exception e)
            {
            }

            return null;
        }

        private WebRequest GetPikodHaOrefClient()
        {
            var webRequest = WebRequest.Create("https://www.oref.org.il/WarningMessages/alert/alerts.json");
            webRequest.Method = "GET";
            webRequest.Timeout = 12000;
            webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            webRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.Headers.Add("Referer", "https://www.oref.org.il/11088-13708-he/Pakar.aspx");
            webRequest.Headers.Add("sec-ch-ua",
                "\"Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            webRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            webRequest.Headers.Add("User-Agent", "");
            return webRequest;
        }
    }
}
