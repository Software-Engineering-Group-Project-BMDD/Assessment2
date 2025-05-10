using MauiApp1.Libs.Core.MVVM.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.Libs.Core.MVVM.ViewModels
{
    /// <summary>
    /// A ViewModel class that serves as a controller for weather data, 
    /// responsible for handling the weather data and managing location changes.
    /// Implements INotifyPropertyChanged to notify the UI of property changes.
    /// </summary>
    public class WeatherViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the WeatherData model which contains the actual weather data.
        /// This is a read-only property, and the data is updated via commands.
        /// </summary>
        public WeatherDataModel WeatherData { get; } = new WeatherDataModel();

        /// <summary>
        /// List of available location IDs.
        /// This property holds the IDs of all locations for which weather data can be displayed.
        /// </summary>
        private List<int> _locationIds = new List<int>();
        public List<int> LocationIds
        {
            get => _locationIds;
            set
            {
                if (_locationIds != value)
                {
                    _locationIds = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The current index of the location in the LocationIds list.
        /// Used to track the current location when cycling through the list.
        /// </summary>
        private int _currentLocationIndex = 0;

        /// <summary>
        /// Command to go to the next location in the LocationIds list.
        /// </summary>
        public ICommand NextLocationCommand { get; }

        /// <summary>
        /// Command to go to the previous location in the LocationIds list.
        /// </summary>
        public ICommand PreviousLocationCommand { get; }

        /// <summary>
        /// Initializes the ViewModel, sets up commands, and starts the process of initializing location IDs.
        /// </summary>
        public WeatherViewModel()
        {
            // Initialize commands to handle location navigation
            NextLocationCommand = new Command(async () => await ChangeLocation(true));
            PreviousLocationCommand = new Command(async () => await ChangeLocation(false));

            // Initialize the location data asynchronously
            _ = InitializeAsync();
        }

        /// <summary>
        /// Asynchronously initializes the weather data by counting locations and populating the LocationIds list.
        /// It retrieves the number of locations and updates the LocationIds property accordingly.
        /// </summary>
        public async Task InitializeAsync()
        {
            // Retrieve the number of locations from the WeatherData model
            int locationCount = await WeatherData.CountLocationsAsync();

            // Log the number of locations for debugging
            Debug.WriteLine($"Number of locations: {locationCount}");

            // Initialize the LocationIds list based on the number of locations
            LocationIds = new List<int>(locationCount);

            // Populate the LocationIds list with values from 1 to locationCount
            for (int i = 1; i < locationCount + 1; i++)
            {
                LocationIds.Add(i);
            }

            // Notify the UI that the LocationIds property has been updated
            OnPropertyChanged(nameof(LocationIds));
        }

        /// <summary>
        /// Loads the weather data for a specific location ID.
        /// This method triggers the WeatherData model to load weather data based on the location ID.
        /// </summary>
        /// <param name="locationId">The ID of the location to load weather data for.</param>
        public async Task LoadWeatherData(int locationId)
        {
            await WeatherData.LoadData(locationId);
        }

        /// <summary>
        /// Changes the current location either to the next or previous location in the LocationIds list.
        /// The index is updated based on the provided direction (next or previous).
        /// </summary>
        /// <param name="isNext">A boolean flag indicating the direction to move (true for next, false for previous).</param>
        private async Task ChangeLocation(bool isNext)
        {
            if (isNext)
            {
                // Move to the next location in the list and wrap around to 0 if at the end
                _currentLocationIndex = (_currentLocationIndex + 1) % _locationIds.Count;
            }
            else
            {
                // Move to the previous location in the list and wrap around to the last element if at the start
                _currentLocationIndex = (_currentLocationIndex - 1 + _locationIds.Count) % _locationIds.Count;
            }

            // Retrieve the location ID from the updated index
            int newLocationId = _locationIds[_currentLocationIndex];

            // Load weather data for the new location ID
            await WeatherData.LoadData(newLocationId);
        }

        /// <summary>
        /// Event raised when a property changes. Implements INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method to raise the PropertyChanged event for the specified property.
        /// This method is invoked whenever a property value is updated.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
