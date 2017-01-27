using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.IO;

namespace WeatherUpdate
{
   class Program
   {
      static void Main(string[] args)
      {
         WeatherLocationList WxLocationList = new WeatherLocationList();

         try
         {
            WxLocationList.load();
         }
         catch(Exception ex){
            TextFileLogger.LogThis(ex.Message, LogType.Error);
            
            //  Create the XML file with default values.
            WxLocationList.addDefaultData();
            WxLocationList.save();
         }

         OpenWeatherMapResponse response = new OpenWeatherMapResponse();
         response.list = new List<Currently>();

         try
         {
            int takeRecords = 1;
            int recordsRetreived = 0;

            while (WxLocationList.WxLocations.Count > recordsRetreived)
            {
               response.list.Add(OpenWeatherMapRequest.Get(WxLocationList.WxLocationIds(recordsRetreived, takeRecords)));
               recordsRetreived += takeRecords;
            }
         }
         catch (Exception ex)
         {
            TextFileLogger.LogThis(ex.Message, LogType.Error);
         }

         WeatherDataList wxInfoList = new WeatherDataList();

         if (response.list != null && response.list.Count == WxLocationList.WxLocations.Count)
         {
            foreach (Currently wxReport in response.list)
            {
               var reportLoc = WxLocationList.WxLocations.Find(s => s.id == wxReport.id);

               if (wxReport.coord != null)
                  reportLoc.coord = new wxCoord(wxReport.coord.lat, wxReport.coord.lon);

               double totalPrecip = wxReport.rain.threeHour + wxReport.snow.threeHour;

               WeatherData wxData = new WeatherData(reportLoc, wxReport.weather[0].main, wxReport.weather[0].description, wxReport.weather[0].icon, wxReport.main.temp,
                  wxReport.wind.deg, wxReport.wind.speed, wxReport.clouds.all, wxReport.main.pressure, wxReport.main.humidity, wxReport.sys.sunrise, wxReport.sys.sunset,
                  totalPrecip, wxReport.dt);

               wxInfoList.WxDataList.Add(wxData);
            }

            wxInfoList.WxDataList.Sort();
            string wxDataContent = wxInfoList.ToString();

            foreach (string sharepointUrl in WxLocationList.sharepointUrls)
               WebPut.uploadFile(sharepointUrl, wxDataContent);

         }else
            TextFileLogger.LogThis("The WX data service did not return any data.", LogType.Error);
      }
   }
}
