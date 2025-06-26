using System.Net.Http;
using Newtonsoft.Json;
using ApuntesGrupo.Models;
using TimeZoneConverter;

namespace ApuntesGrupo.Repositories
{
    public class WeatherRepository
    {
        private readonly HttpClient _httpClient;

        public WeatherRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10); 
        }

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

            // Convertir zona horaria local de Windows a IANA
            string windowsTimeZoneId = TimeZoneInfo.Local.Id;
            string ianaTimeZone = TZConvert.WindowsToIana(windowsTimeZoneId);

            // Agregar parámetros para datos más actualizados
            string url = $"https://api.open-meteo.com/v1/forecast?" +
                        $"latitude={latitude_str}&longitude={longitude_str}" +
                        $"&current=temperature_2m,relative_humidity_2m,rain,weather_code,wind_speed_10m" +
                        $"&timezone={ianaTimeZone}" +
                        $"&forecast_days=1" +
                        $"&models=best_match"; // Usar el mejor modelo disponible

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error HTTP: {response.StatusCode}");

                var result = await response.Content.ReadAsStringAsync();
                WeatherData data = JsonConvert.DeserializeObject<WeatherData>(result);

                // Agregar timestamp actual para simular datos en tiempo real
                if (data?.Current != null)
                {
                    data.Current.Time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                }

                return data ?? new WeatherData();
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Timeout: La solicitud tardó demasiado en responder");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error de conexión: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
