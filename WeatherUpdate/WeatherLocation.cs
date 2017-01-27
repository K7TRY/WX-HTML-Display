using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherUpdate
{
   [Serializable]
   public class WeatherLocationList
   {
      public WeatherLocationList() { }

      public string[] sharepointUrls = new string[] { @"", @"" };

      public List<WeatherLocation> WxLocations = new List<WeatherLocation>();

      public string WxLocationIds(int iSkip, int iTake)
      {
         var returnInfo = WxLocations.Select(x => x.id.ToString()).Skip(iSkip).Take(iTake).ToArray();
         return String.Join(",", returnInfo);
      }

      public void load()
      {
         try
         {
            WeatherLocationList _tempList = (WeatherLocationList)SerializeObject.LoadFromXmlFile(typeof(WeatherLocationList));
            this.WxLocations.AddRange(_tempList.WxLocations);

            if (_tempList.sharepointUrls.Length > 0)
               this.sharepointUrls = _tempList.sharepointUrls;

            _tempList = null;
         }catch(Exception){
            this.addDefaultData();
            this.save();
         }
      }

      public void save()
      {
         SerializeObject.SaveToXmlFile(this);
      }

      public void addDefaultData()
      {
         this.WxLocations.Add(new WeatherLocation(5601933, "Nampa", "US", "America/Boise", -6, "", "Admin", "MAF-US", "", "http://www.operationworld.org/country/unsa/owtext.html", "https://en.wikipedia.org/wiki/Nampa,_Idaho", "http://www.maf.org/"));
         this.WxLocations.Add(new WeatherLocation(3038266, "Albertville", "France", "Europe/Paris", 2, "", "Language School", "MAF-US", "Africa", "http://www.operationworld.org/country/fran/owtext.html", "https://en.wikipedia.org/wiki/Albertville", ""));
         this.WxLocations.Add(new WeatherLocation(2735943, "Porto", "Portugal", "Europe/Lisbon", 1, "", "Language School", "MAF-US", "Africa", "http://www.operationworld.org/country/port/owtext.html", "https://en.wikipedia.org/wiki/Porto", ""));
         this.WxLocations.Add(new WeatherLocation(1215502, "Banda Aceh", "Indonesia", "Asia/Jakarta", 7, "Indonesia", "Air Base", "MAF-US", "Aceh", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Banda_Aceh", ""));
         this.WxLocations.Add(new WeatherLocation(1629131, "Salatiga", "Indonesia", "Asia/Jakarta", 7, "Indonesia", "Language School", "MAF-US", "Indonesia", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Salatiga", ""));
         this.WxLocations.Add(new WeatherLocation(1642911, "Jakarta", "Indonesia", "Asia/Jakarta", 7, "Indonesia", "Admin", "MAF-US", "Indonesia", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Jakarta", ""));
         this.WxLocations.Add(new WeatherLocation(1633118, "Palangka Raya", "Indonesia", "Asia/Pontianak", 7, "Indonesia", "Air Base", "MAF-US", "Kalimantan", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Palangkaraya", "http://www.maf.org/about/where-we-serve/kalimantan"));
         this.WxLocations.Add(new WeatherLocation(1624725, "Tarakan", "Indonesia", "Asia/Makassar", 8, "Indonesia", "Air Base", "MAF-US", "Kalimantan", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Tarakan,_North_Kalimantan", "http://www.maf.org/about/where-we-serve/kalimantan"));
         this.WxLocations.Add(new WeatherLocation(2082539, "Merauke", "Indonesia", "Asia/Jayapura", 9, "Indonesia", "Air Base", "MAF-US", "Papua", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Merauke", "http://www.maf.org/about/where-we-serve/papua"));
         this.WxLocations.Add(new WeatherLocation(1634614, "Nabire", "Indonesia", "Asia/Jayapura", 9, "Indonesia", "Air Base", "MAF-US", "Papua", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Nabire_Regency", "http://www.maf.org/about/where-we-serve/papua"));
         this.WxLocations.Add(new WeatherLocation(2082727, "Sentani", "Indonesia", "Asia/Jayapura", 9, "Indonesia", "Air Base", "MAF-US", "Papua", "http://www.operationworld.org/country/indo/owtext.html", "https://en.wikipedia.org/wiki/Sentani_Airport", "http://www.maf.org/about/where-we-serve/papua"));
         this.WxLocations.Add(new WeatherLocation(3652462, "Quito", "Ecuador", "America/Guayaquil", -5, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/ecua/owtext.html", "https://en.wikipedia.org/wiki/Quito", "https://www.facebook.com/pages/Alas-de-Socorro-Ecuador/158592038193"));
         this.WxLocations.Add(new WeatherLocation(3652584, "Shell", "Ecuador", "America/Guayaquil", -5, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/ecua/owtext.html", "https://en.wikipedia.org/wiki/Shell_Mera", "https://www.facebook.com/pages/Alas-de-Socorro-Ecuador/158592038193"));
         this.WxLocations.Add(new WeatherLocation(3383330, "Paramaribo", "Suriname", "America/Paramaribo", -3, "Latin America", "Air Base", "MAF Affiliate", "Surinaamse Zendings Vliegdienst", "http://www.operationworld.org/country/suri/owtext.html", "https://en.wikipedia.org/wiki/Paramaribo", "http://www.maf.sr/"));
         this.WxLocations.Add(new WeatherLocation(3598132, "Guatemala City", "Guatemala", "America/Guatemala", -6, "Latin America", "Air Base", "MAF Affiliate", "AGAPE", "http://www.operationworld.org/country/guat/owtext.html", "https://en.wikipedia.org/wiki/Guatemala_City", "http://www.agapeguatemala.org/"));
         this.WxLocations.Add(new WeatherLocation(3664980, "Boa Vista", "Brazil", "America/Boa_Vista", -4, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/braz/owtext.html", "https://en.wikipedia.org/wiki/Boa_Vista,_Roraima", "https://www.facebook.com/asas.desocorro"));
         this.WxLocations.Add(new WeatherLocation(3663517, "Manaus", "Brazil", "America/Manaus", -4, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/braz/owtext.html", "https://en.wikipedia.org/wiki/Manaus", "https://www.facebook.com/asas.desocorro"));
         this.WxLocations.Add(new WeatherLocation(3389353, "Santarem", "Brazil", "America/Santarem", -3, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/braz/owtext.html", "https://en.wikipedia.org/wiki/Santar%C3%A9m,_Par%C3%A1", "https://www.facebook.com/asas.desocorro"));
         this.WxLocations.Add(new WeatherLocation(3472287, "Anapolis", "Brazil", "America/Sao_Paulo", -3, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/braz/owtext.html", "https://en.wikipedia.org/wiki/An%C3%A1polis", "https://www.facebook.com/asas.desocorro"));
         this.WxLocations.Add(new WeatherLocation(3718426, "Port au Prince", "Haiti", "America/Port-au-Prince", -4, "Latin America", "Air Base", "MAF-US", "Haiti", "http://www.operationworld.org/country/hait/owtext.html", "https://en.wikipedia.org/wiki/Port-au-Prince", "http://www.maf.org/about/where-we-serve/haiti"));
         this.WxLocations.Add(new WeatherLocation(3621849, "San Jose", "Costa Rica", "America/Costa_Rica", -6, "Latin America", "LT", "MAF-US", "Latin America Learning Technologies", "http://www.operationworld.org/country/cost/owtext.html", "https://en.wikipedia.org/wiki/San_Jos%C3%A9,_Costa_Rica", ""));
         this.WxLocations.Add(new WeatherLocation(3522507, "Oaxaca", "Mexico", "America/Mexico_City", -5, "Latin America", "Air Base", "MAF Affiliate", "Alas de Socorro", "http://www.operationworld.org/country/mexi/owtext.html", "https://en.wikipedia.org/wiki/Oaxaca,_Oaxaca", "http://www.alasdesocorro.org.mx/"));
         this.WxLocations.Add(new WeatherLocation(373303, "Juba", "South Sudan", "Africa/Juba", 3, "Africa", "Air Base", "MAF-US", "Africa", "", "https://en.wikipedia.org/wiki/South_Sudan", "http://www.mafint.org/"));
         this.WxLocations.Add(new WeatherLocation(217695, "Bunia", "DRC", "Africa/Lubumbashi", 2, "Africa", "Air Base", "MAF-US", "East DRC", "http://www.operationworld.org/country/cong/owtext.html", "https://en.wikipedia.org/wiki/Bunia", "http://www.maf.org/about/where-we-serve/edrc"));
         this.WxLocations.Add(new WeatherLocation(233508, "Entebbe", "Uganda", "Africa/Kampala", 3, "Africa", "Air Base", "MAF-US", "Africa", "http://www.operationworld.org/country/ugan/owtext.html", "https://en.wikipedia.org/wiki/Entebbe", ""));
         this.WxLocations.Add(new WeatherLocation(932505, "Maseru", "Lesotho", "Africa/Maseru", 2, "Africa", "Air Base", "MAF-US", "Lesotho", "http://www.operationworld.org/country/leso/owtext.html", "https://en.wikipedia.org/wiki/Maseru", "http://www.maf.org/about/where-we-serve/lesotho"));
         this.WxLocations.Add(new WeatherLocation(1033356, "Nampula", "Mozambique", "Africa/Maputo", 2, "Africa", "Air Base", "MAF-US", "Mozambique", "http://www.operationworld.org/country/moza/owtext.html", "https://en.wikipedia.org/wiki/Nampula", "http://www.maf.org/about/where-we-serve/mozambique"));
         this.WxLocations.Add(new WeatherLocation(2314302, "Kinshasa", "DRC", "Africa/Kinshasa", 1, "Africa", "Air Base", "MAF-US", "West DRC", "http://www.operationworld.org/country/cong/owtext.html", "https://en.wikipedia.org/wiki/Kinshasa", "http://www.maf.org/about/where-we-serve/wdrc"));
         this.WxLocations.Add(new WeatherLocation(922704, "Lubumbashi", "DRC", "Africa/Lubumbashi", 2, "Africa", "Air Base", "MAF-US", "West DRC", "http://www.operationworld.org/country/cong/owtext.html", "https://en.wikipedia.org/wiki/Lubumbashi", "http://www.maf.org/about/where-we-serve/wdrc"));
         this.WxLocations.Add(new WeatherLocation(2253354, "Dakar", "Senegal", "Africa/Dakar", 0, "Africa", "Admin", "MAF-US", "Africa", "http://www.operationworld.org/country/sene/owtext.html", "https://en.wikipedia.org/wiki/Dakar", ""));
         this.WxLocations.Add(new WeatherLocation(2656955, "Ashford", "GB", "Europe/London", 0, "", "Admin", "MAF-I", "", "http://www.operationworld.org/country/unki/owtext.html", "https://en.wikipedia.org/wiki/Ashford,_Kent", "http://www.mafint.org/"));
         this.WxLocations.Add(new WeatherLocation(2064735, "Nhulunbuy", "Australia", "Australia/Darwin", 9.5, "Asia Pacific", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/aust/owtext.html", "https://en.wikipedia.org/wiki/Nhulunbuy", "http://www.maf-arnhemland.org/"));
         this.WxLocations.Add(new WeatherLocation(2172797, "Cairns", "Australia", "Australia/Brisbane", 10, "Asia Pacific", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/aust/owtext.html", "https://en.wikipedia.org/wiki/Cairns", "http://www.maf-arnhemland.org/"));
         this.WxLocations.Add(new WeatherLocation(2177541, "Mareeba", "Australia", "Australia/Brisbane", 10, "Asia Pacific", "Flight Training Centre", "MAF-I", "", "http://www.operationworld.org/country/aust/owtext.html", "https://en.wikipedia.org/wiki/Mareeba_Airfield", "http://www.acma.vic.edu.au/"));
         this.WxLocations.Add(new WeatherLocation(2088122, "Port Moresby", "PNG", "Pacific/Port_Moresby", 10, "Asia Pacific", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/papu/owtext.html", "https://en.wikipedia.org/wiki/Port_Moresby", "http://maf-papuanewguinea.org/"));
         this.WxLocations.Add(new WeatherLocation(2028462, "Ulaanbaatar", "Mongolia", "Asia/Ulaanbaatar", 8, "Asia", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/mong/owtext.html", "https://en.wikipedia.org/wiki/Ulan_Bator", "http://www.blueskyaviation.mn/"));
         this.WxLocations.Add(new WeatherLocation(1185241, "Dhaka", "Bangladesh", "Asia/Dhaka", 6, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/bang/owtext.html", "https://en.wikipedia.org/wiki/Dhaka", "http://www.mafbangladesh.org/"));
         this.WxLocations.Add(new WeatherLocation(2427036, "N’Djamena", "Chad", "Africa/Ndjamena", 1, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/chad/owtext.html", "https://en.wikipedia.org/wiki/N%27Djamena", ""));
         this.WxLocations.Add(new WeatherLocation(1645457, "Dili", "Timor-Leste", "Asia/Dili", 9, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/timo/owtext.html", "https://en.wikipedia.org/wiki/Dili", "http://www.maf-easttimor.org/"));
         this.WxLocations.Add(new WeatherLocation(184745, "Nairobi", "Kenya", "Africa/Nairobi", 3, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/keny/owtext.html", "https://en.wikipedia.org/wiki/Nairobi", ""));
         this.WxLocations.Add(new WeatherLocation(1070940, "Antananarivo", "Madagascar", "Indian/Antananarivo", 3, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/mada/owtext.html", "https://en.wikipedia.org/wiki/Antananarivo", "http://www.maf-madagascar.org/"));
         this.WxLocations.Add(new WeatherLocation(993800, "Johannesburg", "South Africa", "Africa/Johannesburg", 2, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/sout/owtext.html", "https://en.wikipedia.org/wiki/Johannesburg", ""));
         this.WxLocations.Add(new WeatherLocation(232422, "Kampala", "Uganda", "Africa/Kampala", 3, "Africa", "Air Base", "MAF-I", "", "http://www.operationworld.org/country/ugan/owtext.html", "https://en.wikipedia.org/wiki/Kampala", "http://www.maf-uganda.org/"));
         this.WxLocations.Add(new WeatherLocation(3347762, "Lubango", "Angola", "Africa/Luanda", 1, "Africa", "Air Base", "MAF-Canada", "", "http://www.operationworld.org/country/ango/owtext.html", "https://en.wikipedia.org/wiki/Lubango", "http://www.mafc.org/"));
      }
   }

   [Serializable]
   public class WeatherLocation
   {
      public WeatherLocation() { }

      public WeatherLocation(int _id, string _city, string _country, string _timeZone, double _offset, string _region, string _type, string _org, string _program, string _opWorldUrl, string _wikipediaUrl, string _mafUrl)
      {
         this.id = _id;
         this.city = _city;
         this.country = _country;
         this.timeZone = _timeZone;
         this.offset = _offset;
         this.region = _region;
         this.type = _type;
         this.org = _org;
         this.program = _program;
         this.opWorldUrl = _opWorldUrl;
         this.wikipediaUrl = _wikipediaUrl;
         this.mafUrl = _mafUrl;
      }

      public int id { get; set; }
      public string city { get; set; }
      public string country { get; set; }
      public string timeZone { get; set; }
      public double offset { get; set; }
      public string region { get; set; }
      public string type { get; set; }
      public string org { get; set; }
      public string program { get; set; }
      public string opWorldUrl { get; set; }
      public string wikipediaUrl { get; set; }
      public string mafUrl { get; set; }
      public wxCoord coord { get; set; }
   }

   [Serializable]
   public class wxCoord
   {
      public wxCoord()
      {
      }

      public wxCoord(double _lat, double _lon)
      {
         this.lat = _lat;
         this.lon = _lon;
      }

      public double lat { get; set; }
      public double lon { get; set; }
   }
}
