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
                _weatherData = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand CargarClimaCommand { get; }
        private readonly WeatherRepository _weatherRepository;

        public WeatherViewModel()
        {
            CargarClimaCommand = new Command(async () => await GetCurrentWeatherData());
            GetCurrentWeatherData();
        }

        public async Task GetCurrentWeatherData()
        {
            try
            {
                IsLoading = true;
                WeatherDataInfo = new WeatherData();

                WeatherDataInfo = await _weatherRepository.GetCurrentLocationWeatherData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener clima: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
