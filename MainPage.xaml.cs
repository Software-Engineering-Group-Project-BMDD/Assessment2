using MauiApp1.Libs.Core.MVVM.Views;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();


            WeatherPage weatherPage = new WeatherPage();
        }

        // create a navigation to the weather page
        private async void NavigateToWeatherPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeatherPage());
        }


    }

}
