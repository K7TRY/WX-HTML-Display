using System;

namespace WeatherUpdate
{
   public static class staticHelper
   {
      public static string formatDouble(double value, string formatTag)
      {
         return string.Format("{0}{1}", roundToZeroPlaces(value), formatTag);
      }

      public static string roundToZeroPlaces(double origValue)
      {
         return Math.Round(origValue, 0, MidpointRounding.AwayFromZero).ToString();
      }

      public static int roundDouble(double origValue)
      {
         return (int)Math.Round(origValue, 0, MidpointRounding.AwayFromZero);
      }

      public static DateTime UnixTimeStampToUtcDateTime(double unixTimeStamp)
      {
         System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
         dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
         return dtDateTime;
      }

      public static double convertKelvinToCelcius(double kelvin)
      {
         return kelvin - 273d;
      }

      public static double convertKelvinToFahrenheit(double kelvin)
      {
         return (1.8d * (kelvin - 273d)) + 32d;
      }

   }
}
