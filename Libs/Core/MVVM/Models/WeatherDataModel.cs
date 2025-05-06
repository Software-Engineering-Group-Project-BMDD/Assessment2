using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MauiApp1.Libs.Core.MVVM.Models
{
    /// <summary>
    /// Represents the weather data including location and a collection of weather measurements.
    /// Acts as a parent in a one-to-many relationship with measurements.
    /// Implements INotifyPropertyChanged for data binding support.
    /// </summary>
    public class WeatherDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The connection string for accessing the local SQL Server database.
        /// </summary>
        private readonly string _connectionString = @"Server=localhost;Database=MauiAppDB;Integrated Security=True;TrustServerCertificate=True;";

        private LocationInfoModel _location = new();

        /// <summary>
        /// Gets or sets the location information.
        /// Changing this value triggers PropertyChanged notifications.
        /// </summary>
        public LocationInfoModel Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LocationSummary));
                }
            }
        }

        private ObservableCollection<WeatherMeasurementsModel> _measurements = new();

        /// <summary>
        /// Gets or sets the collection of weather measurements.
        /// </summary>
        public ObservableCollection<WeatherMeasurementsModel> Measurements
        {
            get => _measurements;
            set
            {
                if (_measurements != value)
                {
                    _measurements = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Asynchronously loads the location and weather measurements for the specified location ID.
        /// </summary>
        /// <param name="locationId">The ID of the location to load data for.</param>
        public async Task LoadData(int locationId)
        {
            await LoadLocation(locationId);
            await LoadMeasurements(locationId);
        }

        /// <summary>
        /// Loads location details from the database based on the provided location ID.
        /// </summary>
        /// <param name="locationId">The ID of the location.</param>
        private async Task LoadLocation(int locationId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "SELECT * FROM Locations WHERE LocationId = @LocationId";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocationId", locationId);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    Location = new LocationInfoModel
                    {
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Latitude = reader.GetDouble(reader.GetOrdinal("Latitude")),
                        Longitude = reader.GetDouble(reader.GetOrdinal("Longitude")),
                        Elevation = reader.GetInt32(reader.GetOrdinal("Elevation")),
                        Timezone = reader.GetString(reader.GetOrdinal("Timezone")),
                        UtcOffset = reader.GetInt32(reader.GetOrdinal("UtcOffset"))
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Location load error: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads weather measurements from the database for the given location ID.
        /// </summary>
        /// <param name="locationId">The ID of the location.</param>
        private async Task LoadMeasurements(int locationId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "SELECT * FROM WeatherMeasurements WHERE LocationId = @LocationId ORDER BY Time DESC";
                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocationId", locationId);

                var measurements = new ObservableCollection<WeatherMeasurementsModel>();
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    measurements.Add(new WeatherMeasurementsModel
                    {
                        Time = reader.GetDateTime(reader.GetOrdinal("Time")),
                        Temperature = reader.GetDouble(reader.GetOrdinal("Temperature")),
                        Humidity = reader.GetInt32(reader.GetOrdinal("Humidity")),
                        WindSpeed = reader.GetDouble(reader.GetOrdinal("WindSpeed")),
                        WindDirection = reader.GetInt32(reader.GetOrdinal("WindDirection"))
                    });
                }

                Measurements = measurements;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Measurements load error: {ex.Message}");
            }
        }

        /// <summary>
        /// Asynchronously counts the number of available location entries in the database.
        /// </summary>
        /// <returns>The total number of locations.</returns>
        public async Task<int> CountLocationsAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var query = "SELECT COUNT(*) FROM Locations";
                using var command = new SqlCommand(query, connection);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Count Locations error: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Provides a formatted summary string of the current location, including timezone and coordinates.
        /// </summary>
        public string LocationSummary => $"{Location?.Timezone} ({Location?.Latitude:N2}°, {Location?.Longitude:N2}°)";

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the given property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Automatically set by the compiler if omitted.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
