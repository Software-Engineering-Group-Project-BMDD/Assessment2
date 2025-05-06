using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.Libs.Core.MVVM.Models
{
    /// <summary>
    /// Represents the weather measurements for a specific location at a given time.
    /// Includes temperature, humidity, wind speed, and wind direction data.
    /// Implements INotifyPropertyChanged to notify of property changes.
    /// </summary>
    public class WeatherMeasurementsModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The timestamp when the weather measurement was taken. Nullable to represent missing or unknown times.
        /// </summary>
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

        /// <summary>
        /// The temperature in Celsius recorded at the time of the measurement.
        /// </summary>
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

        /// <summary>
        /// The humidity percentage (0-100) recorded at the time of the measurement.
        /// </summary>
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

        /// <summary>
        /// The wind speed in meters per second recorded at the time of the measurement.
        /// </summary>
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

        /// <summary>
        /// The wind direction in degrees (0-360) recorded at the time of the measurement.
        /// This can be converted to a cardinal direction (N, E, S, W) later on if needed.
        /// </summary>
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

        /// <summary>
        /// Event triggered when a property is changed. Implements INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method to raise the PropertyChanged event for property change notifications.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Returns a string representation of the weather measurement, including time, temperature, and humidity.
        /// </summary>
        /// <returns>A formatted string representing the weather data.</returns>
        public override string ToString()
        {
            return $"Time: {Time:g}, Temp: {Temperature}°C, Humidity: {Humidity}%";
        }
    }
}
