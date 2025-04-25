using System;
using Java.Sql;
using System.Globalization;
using Xamarin.Google.ErrorProne.Annotations;
using System.Net.Sockets;

namespace MauiApp1;

public class readSampleData
{
    	
    static async Task<List<string>> readCSV(string DataFile)
        {
            List<string> lines = new List<string>();
            
            try
            {
                // Open the source file
                using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(DataFile);
                using StreamReader sr = new(fileStream);

                    string line;  
                    
                    while ((line = sr.ReadLine()) != null)  
                    {  
                        Console.WriteLine(line);  
                        lines.Add(line);
            
             
                    }  

                    return lines;  
            }  
            catch (Exception ex)  
            { 
                Console.WriteLine(ex.Message); 
            }  
            return lines;  
    } 
        
    public static List<string> readMeta()
    {
            // the point of this is to read the metadata sample data
            var DataFile = @"Data/Metadata.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // Category	Quantity, Symbol, Unit, Unit description, Measurement frequency, Safe level, Reference, Sensor, url
            // 0        1           2       3   4                    5                       6        7          8       9 
            // actual enteries start at index 1

            int count = 0;

            foreach (var line in lines)
            {
                if(count>=1)
                {
                    var values = line.Split(',');

                    string Quantity = values[1];
                    string Symbol = values[2];
                    string Unit = values[3];
                    string uDesc = values[4];
                    string MeasurementFrequency = values[5];
                    string SafeLevel = values[6];
                    string Sensor = values[8]; 
                }
                count++;
            }
            return lines;
    }

    public static void metaToDatabaseSensor(List<string> metadata, int index, double longitude, double latitude)
    {
        

        var line = metadata.ElementAt(index);

        var values = line.Split(',');

        string Quantity = values[1];
        string Symbol = values[2];
        string Unit = values[3];
        string uDesc = values[4];
        string MeasurementFrequency = values[5];
        double SafeLevel = double.Parse(values[6]);
        string Sensor = values[8]; 


        DatabaseRepository.AddSensor(Quantity,Symbol, Unit, uDesc, MeasurementFrequency, SafeLevel, longitude, latitude, Sensor); 

    }

    public static string initializeFullAirQuality()
    {
            // get air quality data from sample data
            string path = FileSystem.AppDataDirectory;
            var DataFile = @"Data/Air_quality.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // the first parts of the sample data are data about the sensor itself
            var siteName = lines.ElementAt(2).Split(',')[1];
            var latitude = lines.ElementAt(3).Split(',')[1];
            var longitude = lines.ElementAt(4).Split(',')[1];
            var siteType = lines.ElementAt(5).Split(',')[1];
            var zone = lines.ElementAt(6).Split(',')[1];
            var agglomeration = lines.ElementAt(7).Split(',')[1];
            var localAuthority = lines.ElementAt(8).Split(',')[1];

            List<string> Metadata = readMeta();
            // lines 2 - 5 / index 1 - 4 are about air quality sensors

            
            metaToDatabaseSensor(Metadata, 1, double.Parse(longitude),double.Parse(latitude)); // nitrogen sensor
            metaToDatabaseSensor(Metadata, 2, double.Parse(longitude),double.Parse(latitude)); // sulphur dioxide sensor
            metaToDatabaseSensor(Metadata, 3, double.Parse(longitude),double.Parse(latitude)); // pm2.5 sensor
            metaToDatabaseSensor(Metadata, 4, double.Parse(longitude),double.Parse(latitude)); // pm10 sensor


            // this produces an sensor reading for the sensor reading table
            // the air quality sample data has its actual readings start at line 11, or index 10
            // it stores these values Date,Time,Nitrogen dioxide,Sulphur dioxide,PM2.5 particulate matter (Hourly measured),PM10 particulate matter (Hourly measured)
            //                         0   1    2                 3                4                                          5
            int count = 0;
            foreach (var line in lines)
            {
                if (count >= 10)
                {
                    var values = line.Split(',');

                    string dateString = values[0] + " " + values[1];
                    string format = "dd/MM/yyyy HH:mm:ss"; // day/month/year 24-hour format
                    CultureInfo provider = CultureInfo.InvariantCulture;

                    DateTime parsedDate = DateTime.ParseExact(dateString, format, provider);
                    // here is where we add the reading to the reading table in the database
                    DatabaseRepository.AddSensorReading(0, double.Parse(values[2]), parsedDate, 0); // nitrogen reading
                    DatabaseRepository.AddSensorReading(1, double.Parse(values[3]), parsedDate, 0); // sulphur dioxide reading
                    DatabaseRepository.AddSensorReading(2, double.Parse(values[4]), parsedDate, 0); // pm2.5 reading
                    DatabaseRepository.AddSensorReading(3, double.Parse(values[5]), parsedDate, 0); // pm10 reading


                }
            }
            return lines.ElementAt(10);
    }
    public static string initializeFullWaterQuality()
    {
            // get air quality data from sample data
            string path = FileSystem.AppDataDirectory;
            var DataFile = @"Data/Water_Quality.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // the first parts of the sample data are data about the sensor itself
            var siteName = lines.ElementAt(0).Split(',')[1];
            var samplePeriod = lines.ElementAt(1).Split(',')[1];
            var location = lines.ElementAt(3).Split(',')[1]; // water quality has one location value,
            var latitude = "";
            var longitude = "";


            List<string> Metadata = readMeta();
            // lines 6 - 10 / index 5 - 9 are about water quality sensors

            
            metaToDatabaseSensor(Metadata, 5, double.Parse(longitude),double.Parse(latitude)); // Nitrate2
            metaToDatabaseSensor(Metadata, 6, double.Parse(longitude),double.Parse(latitude)); // Nitrate3
            metaToDatabaseSensor(Metadata, 7, double.Parse(longitude),double.Parse(latitude)); // Phosphate
            metaToDatabaseSensor(Metadata, 8, double.Parse(longitude),double.Parse(latitude)); // Escherichia
            metaToDatabaseSensor(Metadata, 9, double.Parse(longitude),double.Parse(latitude)); // intestinal enterococci


            // this produces an sensor reading for the sensor reading table
            // the air quality sample data has its actual readings start at line 11, or index 10
            // it stores these values Date	Time	Nitrate (mg l-1)	Nitrite <mg l-1)	Phosphate (mg l-1)	EC (cfu/100ml)

            //                         0   1            2                 3                4                           5
            int count = 0;
            foreach (var line in lines)
            {
                if (count >= 10)
                {
                    var values = line.Split(',');

                    string dateString = values[0] + " " + values[1];
                    string format = "dd/MM/yyyy HH:mm:ss"; // day/month/year 24-hour format
                    CultureInfo provider = CultureInfo.InvariantCulture;

                    DateTime parsedDate = DateTime.ParseExact(dateString, format, provider);
                    // here is where we add the reading to the reading table in the database
                    DatabaseRepository.AddSensorReading(4, double.Parse(values[2]), parsedDate, 0); // Nitrate2
                    DatabaseRepository.AddSensorReading(5, double.Parse(values[3]), parsedDate, 0); // Nitrate3
                    DatabaseRepository.AddSensorReading(6, double.Parse(values[4]), parsedDate, 0); // Phos
                    DatabaseRepository.AddSensorReading(7, double.Parse(values[5]), parsedDate, 0); // Esch
                    // DatabaseRepository.AddSensorReading(8, double.Parse(values[5]), parsedDate, 0); // Intestinal... though the sample data doesnt seem to actually have data for this


                }
            }
            return lines.ElementAt(10);
    }
    public static string initializeFullWeatherData()
    {
            // get air quality data from sample data
            string path = FileSystem.AppDataDirectory;
            var DataFile = @"Data/Weather.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // the first parts of the sample data are data about the sensor itself
            var latitude = lines.ElementAt(1).Split(',')[0];
            var longitude = lines.ElementAt(1).Split(',')[1];


            List<string> Metadata = readMeta();
            // lines 11 - 14 / index 10 - 13 are about weather sensors

            
            metaToDatabaseSensor(Metadata, 10, double.Parse(longitude),double.Parse(latitude)); // air temp
            metaToDatabaseSensor(Metadata, 11, double.Parse(longitude),double.Parse(latitude)); // humidity
            metaToDatabaseSensor(Metadata, 12, double.Parse(longitude),double.Parse(latitude)); // wind speed
            metaToDatabaseSensor(Metadata, 13, double.Parse(longitude),double.Parse(latitude)); // wind direction


            // this produces an sensor reading for the sensor reading table
            // the air quality sample data has its actual readings start at line 11, or index 10
            // it stores these values time	temperature_2m (¬∞C)	relative_humidity_2m (%)	wind_speed_10m (m/s)	wind_direction_10m (¬∞)	

            //                         0        1                        2                              3                  4  
            int count = 0;
            foreach (var line in lines)
            {
                if (count >= 10)
                {
                    var values = line.Split(',');

                    string dateString = values[0];
                    string format = "yyyy-MM-ddTHH:mm"; 
                    CultureInfo provider = CultureInfo.InvariantCulture;

                    DateTime parsedDate = DateTime.ParseExact(dateString, format, provider);
                    // here is where we add the reading to the reading table in the database
                    DatabaseRepository.AddSensorReading(9, double.Parse(values[1]), parsedDate, 0); // temp
                    DatabaseRepository.AddSensorReading(10, double.Parse(values[2]), parsedDate, 0); // humid
                    DatabaseRepository.AddSensorReading(11, double.Parse(values[3]), parsedDate, 0); // wind speed
                    DatabaseRepository.AddSensorReading(12, double.Parse(values[4]), parsedDate, 0); // wind dir

                }
            }
            return lines.ElementAt(10);
    }
}


