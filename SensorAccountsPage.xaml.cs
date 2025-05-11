namespace MauiApp1
{
    public partial class SensorAccountsPage : ContentPage
    {
        public SensorAccountsPage()
        {
            InitializeComponent();
            BindingContext = new MauiApp1.MVVM.ViewModels.SensorAccountViewModel();
        }
    

        private async void OnAirQualitySensorClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Air Quality Sensor", "Air Quality Sensor clicked!", "OK");
        }

        private async void OnWaterQualitySensorClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Water Quality Sensor", "Water Quality Sensor clicked!", "OK");
        }

        private async void OnWeatherSensorClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Weather Sensor", "Weather Sensor clicked!", "OK");
        }
    }
}
