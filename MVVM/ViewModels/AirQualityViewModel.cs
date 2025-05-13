using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MauiApp1.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for managing air quality data and providing commands for refreshing the data.
    /// </summary>
    public partial class AirQualityViewModel : ObservableObject
    {
        /// <summary>
        /// Indicates whether the data is currently being refreshed.
        /// </summary>
        [ObservableProperty]
        private bool isRefreshing;

        /// <summary>
        /// Gets the collection of air quality data items.
        /// </summary>
        public ObservableCollection<AirQualityData> AirQualityItems { get; } = new();

        /// <summary>
        /// Gets the command to refresh the air quality data.
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AirQualityViewModel"/> class.
        /// </summary>
        public AirQualityViewModel()
        {
            RefreshCommand = new RelayCommand(RefreshData);
            LoadInitialData();
        }

        /// <summary>
        /// Loads the initial air quality data into the collection.
        /// </summary>
        private void LoadInitialData()
        {
            // Simulate loading initial data
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "10:00", PM25 = 12.5, PM10 = 25.0, CO = 0.8 });
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "11:00", PM25 = 15.0, PM10 = 30.0, CO = 1.0 });
        }

        /// <summary>
        /// Refreshes the air quality data by clearing the current collection and adding new data.
        /// </summary>
        private void RefreshData()
        {
            IsRefreshing = true;

            // Simulate refreshing data
            AirQualityItems.Clear();
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "12:00", PM25 = 10.0, PM10 = 20.0, CO = 0.7 });
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "13:00", PM25 = 11.5, PM10 = 22.0, CO = 0.9 });

            IsRefreshing = false;
        }
    }

    /// <summary>
    /// Represents a single air quality data record.
    /// </summary>
    public class AirQualityData
    {
        /// <summary>
        /// Gets or sets the date of the air quality measurement.
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the time of the air quality measurement.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the PM2.5 concentration value.
        /// </summary>
        public double PM25 { get; set; }

        /// <summary>
        /// Gets or sets the PM10 concentration value.
        /// </summary>
        public double PM10 { get; set; }

        /// <summary>
        /// Gets or sets the CO concentration value.
        /// </summary>
        public double CO { get; set; }
    }
}
