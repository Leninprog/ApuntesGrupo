using System.Net.Http;
using Newtonsoft.Json;
using ApuntesGrupo.Models;

namespace ApuntesGrupo.Repositories
{
    public class WeatherRepository
    {
        public async Task<WeatherData> GetCurrentLocationWeatherData()
        {
            GeolocationRepository _geoRepository = new GeolocationRepository();
            var location = await _geoRepository.GetCurrentLocationAsync();
            return await GetWeatherDataAsync(location.Latitude, location.Longitude);
        }

        public async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            string latitude_str = latitude.ToString().Replace(",", ".");
            string longitude_str = longitude.ToString().Replace(",", ".");
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude_str}&longitude={longitude_str}&current=temperature_2m,relative_humidity_2m,rain&timezone=Europe%2FBerlin";

            using HttpClient httpclient = new();
            var response = await httpclient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo obtener datos del clima");

            var result = await response.Content.ReadAsStringAsync();
            WeatherData data = JsonConvert.DeserializeObject<WeatherData>(result);
            return data;
        }
    }
}
