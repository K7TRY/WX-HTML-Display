using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenWeatherMap
{
    [DataContract]
    public class OpenWeatherMapResponse
    {
        [DataMember]
        public int cnt { get; set; }
        [DataMember]
        public List<Currently> list { get; set; }
    }

    [DataContract]
    public class Currently
    {
       public Currently()
       {
          this.rain = new rainAndSnowInfo();
          this.snow = new rainAndSnowInfo();
       }

       [DataMember]
       public coordinates coord { get; set; }
       [DataMember]
       public systemInfo sys { get; set; }
       [DataMember]
       public List<wx> weather { get; set; }
       [DataMember]
       public mainInfo main { get; set; }
       [DataMember]
       public windInfo wind { get; set; }
       [DataMember]
       public rainAndSnowInfo rain { get; set; }
       [DataMember]
       public rainAndSnowInfo snow { get; set; }

       [DataMember]
       public cloudInfo clouds { get; set; }

       [DataMember]
       public int dt { get; set; }
       [DataMember]
       public int id { get; set; }
       [DataMember]
       public string name { get; set; }
    }

    [DataContract]
    public class coordinates
    {
       [DataMember]
       public double lon { get; set; }
       [DataMember]
       public double lat { get; set; }
    }

    [DataContract]
    public class systemInfo
    {
       [DataMember]
       public int type { get; set; }
       [DataMember]
       public int id { get; set; }
       [DataMember]
       public double message { get; set; }
       [DataMember]
       public string country { get; set; }
       [DataMember]
       public Int64 sunrise { get; set; }
       [DataMember]
       public Int64 sunset { get; set; }
    }

    [DataContract]
    public class wx
    {
       [DataMember]
       public int id { get; set; }
       [DataMember]
       public string main { get; set; }
       [DataMember]
       public string description { get; set; }
       [DataMember]
       public string icon { get; set; }
    }

    [DataContract]
    public class mainInfo
    {
       [DataMember]
       public double temp { get; set; }
       [DataMember]
       public double pressure { get; set; }
       [DataMember]
       public int humidity { get; set; }
       [DataMember]
       public double temp_min { get; set; }
       [DataMember]
       public double temp_max { get; set; }
       [DataMember]
       public double sea_level { get; set; }
       [DataMember]
       public double grnd_level { get; set; }
    }

    [DataContract]
    public class windInfo
    {
       [DataMember]
       public double speed { get; set; }
       [DataMember]
       public double deg { get; set; }
       [DataMember]
       public double gust { get; set; }
    }

    [DataContract]
    public class cloudInfo
    {
       [DataMember]
       public int all { get; set; }
    }

    [DataContract]
    public class rainAndSnowInfo
    {
       public rainAndSnowInfo() { threeHour = 0.0d; }

       [DataMember]
       public double threeHour { get; set; }
    }

}
