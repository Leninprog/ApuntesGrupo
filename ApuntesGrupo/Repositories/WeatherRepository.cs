using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static ApuntesGrupo.Models.WeatherModels;

namespace ApuntesGrupo.Repositories
{
    class WeatherRepository
    {
        public async Task<WeatherData> GetCurrentLocationWeatherData()
        {
            GeolocationRepository _geoRepository = new GeolocationRepository(); 
            Location location = await _geoRepository.GetCurrentLocationAsync();
            return await GetWeatherDataAsync(location.Latitude, location.Longitude);
        }
        public async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            string latitude_str = latitude.ToString().Replace(",", ".");
            string longitude_str = longitude.ToString().Replace(",", ".");
            string url = $"https://api.open-meteo.com/v1/forecast?latitude="+ latitude_str + "&longitude=" + longitude_str + "&current_weather=true&timezone=Europe%2FBerlin";

            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            WeatherData data = JsonConvert.DeserializeObject<WeatherData>(result);
            return data;
        }
    }
}
