using MauiApp1.Libs.Core.MVVM.ViewModels;

namespace MauiApp1.Libs.Core.MVVM.Views;

public partial class WeatherPage : ContentPage
{


    public WeatherPage()
    {
        InitializeComponent();

        BindingContext = new WeatherViewModel();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((WeatherViewModel)BindingContext).LoadWeatherData(locationId: 1); // Load Edinburgh data
    }
}