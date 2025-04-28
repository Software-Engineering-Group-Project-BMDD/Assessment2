//using System.Data.SqlClient;
using Microsoft.Data.SqlClient; // this is updated and newest package
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace MauiApp1.Libs.Core.MVVM.Models
{
    // this is a weather model with the updating interface, it is the parent in a 1 to many relation for
    // location and weather measurements
    public class WeatherDataModel : INotifyPropertyChanged
    {
        // this is my connection string to my local sql database
        // i tried using docker but wasn't working maybe due to networking ang permissions
        // const string basically
        private readonly string _connectionString = @"Server=localhost;Database=MauiAppDB;Integrated Security=True;TrustServerCertificate=True;";

        // initilaize the location data class
        private LocationInfoModel _location = new();
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

        // this is a fancy list with add and update using the weather measurements, just initilaising
        private ObservableCollection<WeatherMeasurementsModel> _measurements = new();
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


        // loads the data based on the location information , 0,1,2 and so on 
        public async Task LoadData(int locationId)
        {
            await LoadLocation(locationId);
            await LoadMeasurements(locationId);
        }

        // load the data from the database based on the location id
        private async Task LoadLocation(int locationId)
        {

            try
            {
                // initaize connection with connection string
                SqlConnection connection = new SqlConnection(_connectionString);

                // open conection
                await connection.OpenAsync();

                // a query to pull all the data related to a specfic location
                var query = "SELECT * FROM Locations WHERE LocationId = @LocationId";

                // make a command query and connection
                using var command = new SqlCommand(query, connection);

                // bind parameters
                command.Parameters.AddWithValue("@LocationId", locationId);

                //
                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    // update properties
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
            // error handling display 
            catch (Exception ex)
            {
                Debug.WriteLine($"Location load error: {ex.Message}");
            }
        }

        private async Task LoadMeasurements(int locationId)
        {
            try
            {
                //initial connection with teh connection string 
                using var connection = new SqlConnection(_connectionString);

                // open the conenction
                await connection.OpenAsync();

                // select all from weathermeasurments from location id and order into realistic time 
                var query = "SELECT * FROM WeatherMeasurements WHERE LocationId = @LocationId ORDER BY Time DESC";

                // setup teh query
                using var command = new SqlCommand(query, connection);

                // data binding
                command.Parameters.AddWithValue("@LocationId", locationId);

                // fancy list
                var measurements = new ObservableCollection<WeatherMeasurementsModel>();

                // query executer 
                using var reader = await command.ExecuteReaderAsync();

                // loop till done
                while (await reader.ReadAsync())
                {
                    // update the collection list
                    measurements.Add(new WeatherMeasurementsModel
                    {
                        Time = reader.GetDateTime(reader.GetOrdinal("Time")),
                        Temperature = reader.GetDouble(reader.GetOrdinal("Temperature")),
                        Humidity = reader.GetInt32(reader.GetOrdinal("Humidity")),
                        WindSpeed = reader.GetDouble(reader.GetOrdinal("WindSpeed")),
                        WindDirection = reader.GetInt32(reader.GetOrdinal("WindDirection"))
                    });
                }

                // update the preperties with teh database query
                Measurements = measurements;
            }
            catch (Exception ex)
            {
                // simple error message to catch error and bugs
                Debug.WriteLine($"Measurements load error: {ex.Message}");
            }
        }

        // this method is to help dynamically count all the locations so the i can scroll throught teh records withput issues
        public async Task<int> CountLocationsAsync()
        {
            try
            {
                // setup a connection
                SqlConnection connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // count all from the  Locations table
                var query = "SELECT COUNT(*) FROM Locations";

                // query and connection
                using var command = new SqlCommand(query, connection);

                // Execute the query and get the result
                var result = await command.ExecuteScalarAsync();

                // data into a object
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                // Write a debug meassage on error
                Debug.WriteLine($"Count Locations error: {ex.Message}");

                // return fail
                return 0;
            }
        }




        // interpolated string with nullable checks regarding geo location data
        public string LocationSummary => $"" +
            $"{Location?.Timezone} ({Location?.Latitude:N2}°, {Location?.Longitude:N2}°)";

        // evnet 
        public event PropertyChangedEventHandler PropertyChanged;

        // magic property call methid that know who clal ed it, part of teh relection system I believe
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}


