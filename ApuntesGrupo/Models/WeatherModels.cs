using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApuntesGrupo.Models
{
    public class WeatherData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double GenerationtimeMs { get; set; }
        public int UtcOffsetSeconds { get; set; }
        public string Timezone { get; set; }
        public string TimezoneAbbreviation { get; set; }
        public double Elevation { get; set; }
        public CurrentUnits CurrentUnits { get; set; }
        public CurrentWeather Current { get; set; }
    }

    public class CurrentUnits
    {
        public string Time { get; set; }
        public string Interval { get; set; }
        public string Temperature2m { get; set; }
        public string RelativeHumidity2m { get; set; }
        public string Rain { get; set; }
    }

    public class CurrentWeather
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("temperature_2m")]
        public double Temperature2m { get; set; }

        [JsonProperty("relative_humidity_2m")]
        public int RelativeHumidity2m { get; set; }

        [JsonProperty("rain")]
        public double Rain { get; set; }
    }
}
