using MauiApp1.Libs.Core.MVVM.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.Libs.Core.MVVM.ViewModels
{

    // i consider this class to be a thin data controller, with property change updates
    public class WeatherViewModel : INotifyPropertyChanged
    {

        // a get only WeatherData property 
        public WeatherDataModel WeatherData { get; } = new WeatherDataModel();


        // List of available location IDs, turned it into a property 
        private List<int> _locationIds = new List<int>();
        public List<int> LocationIds
        {
            // getter
            get => _locationIds;

            // setter
            set
            {
                // null check
                if (_locationIds != value)
                {
                    _locationIds = value;
                    OnPropertyChanged();
                }
            }
        }

        // an intial value used for a counter
        private int _currentLocationIndex = 0;

        // Command to go to the next location
        public ICommand NextLocationCommand { get; }

        // Command to go to the previous location
        public ICommand PreviousLocationCommand { get; }


        // class constructor
        public WeatherViewModel()
        {

            // Initialize the commands
            // up teh iteration id
            NextLocationCommand = new Command(async () => await ChangeLocation(true));

            // down the iterations
            PreviousLocationCommand = new Command(async () => await ChangeLocation(false));

            // discrd for safety to start teh process for initlaising the ids
            _ = InitializeAsync();
        }

        // this methods garb teh location amount, dsiplay the message and updates thelocal property
        public async Task InitializeAsync()
        {
            // Run the task to count the locations and get the count
            int locationCount = await WeatherData.CountLocationsAsync();

            // Debug the outcome of the SQL query
            Debug.WriteLine($"Number of locations: {locationCount}");

            // Update the list with the number of locations 
            LocationIds = new List<int>(locationCount);

            // Additional initialization logic here, start at 1 like the databse entry, + 1 on list match the real value
            for (int i = 1; i < locationCount + 1; i++)
            {
                // update the the property
                LocationIds.Add(i);
            }

            // Notify the UI that LocationIds has been updated
            OnPropertyChanged(nameof(LocationIds));
        }



        // update and refresh the data based on a location id
        public async Task LoadWeatherData(int locationId)
        {
            await WeatherData.LoadData(locationId);
        }


        // Change the location ID (either next or previous)
        private async Task ChangeLocation(bool isNext)
        {
            // bool control to choose direction
            if (isNext)
            {
                // a smart use of modulus to allow reseting backing to 0 after reaching the limit
                _currentLocationIndex = (_currentLocationIndex + 1) % _locationIds.Count;
            }
            else
            {
                // a smart use of modulus to allow reseting backing the end 
                _currentLocationIndex = (_currentLocationIndex - 1 + _locationIds.Count) % _locationIds.Count;
            }

            // grab the id 
            int newLocationId = _locationIds[_currentLocationIndex];

            // load teh data with that id
            await WeatherData.LoadData(newLocationId);
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