namespace MauiApp1
{
    /// <summary>
    /// Code-behind for the SensorAccountsPage, handling initialization and sensor button click events.
    /// </summary>
    public partial class SensorAccountsPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SensorAccountsPage"/> class and sets the BindingContext.
        /// </summary>
        public SensorAccountsPage()
        {
            InitializeComponent();
            BindingContext = new MauiApp1.MVVM.ViewModels.SensorAccountViewModel();
        }

        /// <summary>
        /// Handles the click event for the Air Quality Sensor button.
        /// Displays an alert indicating the Air Quality Sensor was clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void OnAirQualitySensorClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Air Quality Sensor", "Air Quality Sensor clicked!", "OK");
        }

        /// <summary>
        /// Handles the click event for the Water Quality Sensor button.
        /// Displays an alert indicating the Water Quality Sensor was clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void OnWaterQualitySensorClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Water Quality Sensor", "Water Quality Sensor clicked!", "OK");
        }

        /// <summary>
        /// Handles the click event for the Weather Sensor button.
        /// Displays an alert indicating the Weather Sensor was clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private async void OnWeatherSensorClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Weather Sensor", "Weather Sensor clicked!", "OK");
        }
    }
}
