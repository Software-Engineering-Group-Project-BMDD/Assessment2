using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.Libs.Core.MVVM.Models
{
    // A generic data model for handling site data corresponding to the sample weather data

    /// <summary>
    /// Represents location information, including geographical coordinates and elevation.
    /// Implements the <see cref="INotifyPropertyChanged"/> interface to notify changes in property values.
    /// </summary>
    public class LocationInfoModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the city or area.
        /// </summary>
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;

                    // Updates if changed using the INotifyPropertyChanged interface 
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The latitude of the location, expressed in decimal degrees.
        /// </summary>
        private double _latitude = 0.0;
        public double Latitude
        {
            get => _latitude;
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The longitude of the location, expressed in decimal degrees.
        /// </summary>
        private double _longitude = 0.0;
        public double Longitude
        {
            get => _longitude;
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The elevation of the location, in meters, above or below sea level.
        /// </summary>
        private int _elevation = 0;
        public int Elevation
        {
            get => _elevation;
            set
            {
                if (_elevation != value)
                {
                    _elevation = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The UTC offset of the location, in minutes.
        /// </summary>
        private int _utcOffset = 0;
        public int UtcOffset
        {
            get => _utcOffset;
            set
            {
                if (_utcOffset != value)
                {
                    _utcOffset = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The timezone of the location, represented in a string (e.g., "GMT").
        /// </summary>
        private string _timezone = "GMT";
        public string Timezone
        {
            get => _timezone;
            set
            {
                if (_timezone != value)
                {
                    _timezone = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Event raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify subscribers of property changes.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
