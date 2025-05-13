using MauiApp1.ViewModels;

namespace MauiApp1
{
    public partial class AirQualityPage : ContentPage
    {
        public AirQualityPage()
        {
            InitializeComponent();
            BindingContext = new AirQualityViewModel();
        }
    }
}
