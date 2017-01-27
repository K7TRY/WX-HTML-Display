using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace OpenWeatherMap
{
   public static class OpenWeatherMapRequest
   {
      public static Currently Get(string _locArray)
      {
         const int MaxAttempts = 4;
         int attempt = 0;

         var url = String.Format(Properties.Settings.Default.WeatherDataURl, _locArray);

         string result = string.Empty;
            using (var client = new CompressionEnabledWebClient())
            {
                client.Encoding = Encoding.UTF8;

                while (++attempt <= MaxAttempts)
                {
                    try
                    {
                        result = client.DownloadString(url).Replace("\"3h\"", "\"threeHour\"");
                    }
                    catch (WebException wex)
                    {
                        if (result == string.Empty)
                            if (((System.Net.HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.BadGateway || ((System.Net.HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.GatewayTimeout)
                                System.Threading.Thread.Sleep(1000 * 60 * 60);// One minute
                            else
                                System.Threading.Thread.Sleep(1000 * 60 * 60 * 10);// Ten minute
                    }
                }

            }

         var serializer = new JavaScriptSerializer();
         var dataObject = serializer.Deserialize<Currently>(result);

         return dataObject;
      }
   }
}
