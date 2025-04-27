using MauiApp1.Libs.Core.MVVM.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.Libs.Core.MVVM.ViewModels
{

    // i consider this class to be a thin data controller, with property change updates
    public class WeatherViewModel : INotifyPropertyChanged
    {

        // a get only WeatherData property 
        public WeatherDataModel WeatherData { get; } = new WeatherDataModel();

        // update and refresh the data based on a location id
        public async Task LoadWeatherData(int locationId)
        {
            await WeatherData.LoadData(locationId);
        }

        // event for changes
        public event PropertyChangedEventHandler PropertyChanged;

        // the magic method for property changes
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}