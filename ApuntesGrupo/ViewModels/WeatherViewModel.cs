using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ApuntesGrupo.Repositories;
using ApuntesGrupo.Models;

namespace ApuntesGrupo.ViewModels
{
    public class WeatherViewModel : INotifyPropertyChanged
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

        public ICommand CargarClimaCommand { get; }

        public WeatherViewModel()
        {
            CargarClimaCommand = new Command(async () => await GetCurrentWeatherData());
            GetCurrentWeatherData(); // también carga automáticamente al abrir
        }

        public async Task GetCurrentWeatherData()
        {
            WeatherRepository weatherRepository = new WeatherRepository();
            WeatherDataInfo = await weatherRepository.GetCurrentLocationWeatherData();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
