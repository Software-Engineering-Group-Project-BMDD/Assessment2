using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.Libs.Core.MVVM.Models
{
    // this is the measurements for teh weather data, properties 
    public class WeatherMeasurementsModel : INotifyPropertyChanged
    {
        // a nullable timestamp
        private DateTime? _time;
        public DateTime? Time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged();
                }
            }
        }

        // tempaerature in celcuis
        private double _temperature;
        public double Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged();
                }
            }
        }

        // humidty 1- 100 percent
        private int _humidity;
        public int Humidity
        {
            get => _humidity;
            set
            {
                if (_humidity != value)
                {
                    _humidity = value;
                    OnPropertyChanged();
                }
            }
        }

        // wind speed in meters per second
        private double _windSpeed;
        public double WindSpeed
        {
            get => _windSpeed;
            set
            {
                if (_windSpeed != value)
                {
                    _windSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        // wind direction in 360 degress, this can be converted later on to a cardinal systems N, E, S, W
        private int _windDirection;
        public int WindDirection
        {
            get => _windDirection;
            set
            {
                if (_windDirection != value)
                {
                    _windDirection = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Time: {Time:g}, Temp: {Temperature}°C, Humidity: {Humidity}%";
        }
    }
}