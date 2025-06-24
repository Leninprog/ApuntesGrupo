using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApuntesGrupo.Repositories; // ✅ Agregado
using static ApuntesGrupo.Models.WeatherModels;

namespace ApuntesGrupo.ViewModels
{
    class WeatherViewModel : INotifyPropertyChanged
    {
        private WeatherData _weatherData = new WeatherData();
        public WeatherData WeatherDataInfo
        {
            get => _weatherData;
            set
            {
                if (_weatherData != value)
                {
                    _weatherData = value;
                    OnPropertyChanged();
                }
            }
        }

        public WeatherViewModel()
        {
            GetCurrentWeatherData();
        }

        public async void GetCurrentWeatherData()
        {
            WeatherRepository weatherRepository = new WeatherRepository();
            WeatherDataInfo = await weatherRepository.GetCurrentLocationWeatherData(); // ✅ Corregido
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
