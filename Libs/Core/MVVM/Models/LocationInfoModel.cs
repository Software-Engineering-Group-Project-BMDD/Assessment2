using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiApp1.Libs.Core.MVVM.Models
{
    // a generic data model for handling site data corresponding to teh sampple weather data
    public class LocationInfoModel : INotifyPropertyChanged
    {
        // Name for city or area
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;

                    // update if changed using the interface 
                    OnPropertyChanged();
                }
            }
        }


        // latitude
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

        //longittude
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

        // meters above or below ground
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

        // offset but more precis minutes
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

        // GMT is hourly based
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

        // a typical event handler
        public event PropertyChangedEventHandler PropertyChanged;

        // this is a magic methid to update the property, it raises the changed event and observers or subscribers update
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}