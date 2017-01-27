using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Script.Serialization;

namespace WeatherUpdate
{
   [Serializable]
   public class WeatherDataList
   {
      public WeatherDataList()
      {
      }

      public DateTime cacheTimeUtc = DateTime.UtcNow;
      public List<WeatherData> WxDataList = new List<WeatherData>();

      public override string ToString()
      {
         var serializer = new JavaScriptSerializer();
         return serializer.Serialize(this);
      }
   }

   [Serializable]
   public class WeatherData : IComparable<WeatherData>
   {
      public WeatherData()
      {
      }

      public WeatherData(WeatherLocation _location, string _summary, string _longSummary, string _icon, double _temp, double _windDeg,
         double _windSpeed, double _cloudCover, double _pressure, double _humidity, long _sunriseTime, long _sunsetTime, double _precipitationLast3Hours, int _wxTime)
      {
         TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
         this.WxLocation = _location;
         this.summary = _summary;
         this.longSummary = textInfo.ToTitleCase(_longSummary);
         this.icon = _icon;
         this.currentTemp = new temperatureData(_temp);
         this.windInfo = new windData(_windSpeed, Math.Round(_windDeg, 2));
         this.cloudCover = _cloudCover;
         this.humidity = _humidity;

         this.sunRiseAndSet = new sun(_sunriseTime, _sunsetTime);
         this.precipLast3Hrs = new precipitationLast3Hours(_precipitationLast3Hours);

         this.wxTime = staticHelper.UnixTimeStampToUtcDateTime(_wxTime);
      }

      public WeatherLocation WxLocation { get; set; }

      public string summary { get; set; }

      public DateTime wxTime { get; set; }

      public string longSummary { get; set; }

      public string icon { get; set; }

      public temperatureData currentTemp { get; set; }

      public sun sunRiseAndSet { get; set; }

      public windData windInfo { get; set; }

      public double humidity { get; set; }

      public double cloudCover { get; set; }

      public precipitationLast3Hours precipLast3Hrs { get; set; }

      int IComparable<WeatherData>.CompareTo(WeatherData other)
      {
         if (this.WxLocation.coord.lon == other.WxLocation.coord.lon)
            return 0;
         if (this.WxLocation.coord.lon < other.WxLocation.coord.lon)
            return -1;

         return 1;
      }
   }

   [Serializable]
   public class temperatureData
   {
      public temperatureData()
      {
      }

      public temperatureData(double _tempInF)
      {
         this.temp = _tempInF;
      }

      protected double temp { get; set; } // Assumes the temp is in F.

      public int F
      {
         get { return staticHelper.roundDouble(staticHelper.convertKelvinToFahrenheit(temp)); }
      }

      public int C
      {
         get { return staticHelper.roundDouble(staticHelper.convertKelvinToCelcius(temp)); }
      }
   }

   [Serializable]
   public class windData
   {
      public windData()
      {
      }

      public windData(double _speedInMps, double _deg) //Meters per second
      {
         this.windSpeed = _speedInMps;
         this.windDirectionDeg = _deg;
      }

      protected double windSpeed { get; set; } // Assumes it is in Meters Per Second.

      public double windDirectionDeg { get; set; }

      public string windDirection
      {
         get {
            if (this.windDirectionDeg >= 348.75 || this.windDirectionDeg < 11.25)
               return "N";

            else if (this.windDirectionDeg >= 11.25 && this.windDirectionDeg < 33.75)
               return "NNE";

            else if (this.windDirectionDeg >= 33.75 && this.windDirectionDeg < 56.25)
               return "NE";

            else if (this.windDirectionDeg >= 56.25 && this.windDirectionDeg < 78.75)
               return "ENE";

            else if (this.windDirectionDeg >= 78.75 && this.windDirectionDeg < 101.25)
               return "E";

            else if (this.windDirectionDeg >= 101.25 && this.windDirectionDeg < 123.75)
               return "ESE";

            else if (this.windDirectionDeg >= 123.75 && this.windDirectionDeg < 146.25)
               return "SE";

            else if (this.windDirectionDeg >= 146.25 && this.windDirectionDeg < 168.75)
               return "SSE";

            else if (this.windDirectionDeg >= 168.75 && this.windDirectionDeg < 191.25)
               return "S";

            else if (this.windDirectionDeg >= 191.25 && this.windDirectionDeg < 213.75)
               return "SSW";

            else if (this.windDirectionDeg >= 213.75 && this.windDirectionDeg < 236.25)
               return "SW";

            else if (this.windDirectionDeg >= 236.25 && this.windDirectionDeg < 258.75)
               return "WSW";

            else if (this.windDirectionDeg >= 258.75 && this.windDirectionDeg < 281.25)
               return "W";

            else if (this.windDirectionDeg >= 281.25 && this.windDirectionDeg < 303.75)
               return "WNW";

            else if (this.windDirectionDeg >= 303.75 && this.windDirectionDeg < 326.25)
               return "NW";

            else if (this.windDirectionDeg >= 326.25 && this.windDirectionDeg < 348.75)
               return "NNW";

            return "&mdash;";
         }
      }

      public int Mph
      {
         get { return staticHelper.roundDouble(windSpeed * 2.2369362920544); }
      }

      public int Kph
      {
         get { return staticHelper.roundDouble(windSpeed * 3.6); }
      }

      public int Knots
      {
         get { return staticHelper.roundDouble(windSpeed * 1.9438444924406); }
      }
   }

   [Serializable]
   public class sun
   {
      public sun()
      {
      }

      public sun(long _sunriseTime, long _sunsetTime)
      {
         this.riseUtc = staticHelper.UnixTimeStampToUtcDateTime(_sunriseTime);
         this.setUtc = staticHelper.UnixTimeStampToUtcDateTime(_sunsetTime);
      }

      public DateTime riseUtc { get; set; }

      public DateTime setUtc { get; set; }

      public string daylight
      {
         get
         {
            if (riseUtc.CompareTo(setUtc) == -1) // This is the sunrise for today.
            {
               TimeSpan daylight = setUtc.Subtract(riseUtc);
               return String.Format("{0} hours and {1} minutes.", daylight.Hours, daylight.Minutes);
            }
            else
               return "&mdash;"; // Rarely there can be a sunrise and sunset on different days near the poles.
         }
      }
   }
   
   [Serializable]
   public class precipitationLast3Hours
   {
      public precipitationLast3Hours()
      {
      }

      public precipitationLast3Hours(double _precip) //Millimeters
      {
         this.precip = _precip;
      }

      protected double precip { get; set; } // Assumes it is in Millimeters.

      public double inches
      {
         get { return Math.Round((this.precip * 0.0393701), 2, MidpointRounding.AwayFromZero); }
      }

      public double cm
      {
         get { return Math.Round((this.precip / 10), 2, MidpointRounding.AwayFromZero); }
      }
   }
}