using ApuntesGrupo.ViewModels;
namespace ApuntesGrupo.Views;

public partial class WeatherView : ContentPage
{
	public WeatherView()
	{
        InitializeComponent();
        BindingContext = new WeatherViewModel();
	}
}