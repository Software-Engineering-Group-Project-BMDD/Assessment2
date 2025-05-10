using System;
using System.Globalization;
using System.Net.Sockets;
using MauiApp1.Model;
using SQLite;


namespace MauiApp1.UI.Model;

public class readSampleData
{
    public static bool dbAvailable = true;
    private SensorDatabase _database;

    public readSampleData(SensorDatabase sd)
    {
        _database = sd;
    }

    public static async Task<List<string>> readCSV(string DataFile)
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

    public async Task metaToDatabaseSensor(List<string> metadata, int index, double longitude, double latitude, string type)
    {
        

        var line = metadata.ElementAt(index);

        var values = line.Split(',');

        string rQuantity = values[1];
        string rSymbol = values[2];
        string rUnit = values[3];
        string ruDesc = values[4];
        string rMeasurementFrequency = values[5];
        string rSafeLevel = values[6];
        //string rSensor = values[8]; 

        // this counts how many sensors there are
        //var sensors = await _database.GetSensorsAsync();
        //if (sensors.Count() > 0)
        //{
        //    return;
        //}

        //
        //(Quantity,Symbol, Unit, uDesc, MeasurementFrequency, SafeLevel, longitude, latitude, Sensor); 
        // checks if the sensor is already in the database 
        bool exists = await _database.DoesSensorExist(rQuantity);

        
        // adds if not
        if(!exists)
        {
            Sensor sen = new Sensor { Sensor_Quantity=rQuantity, Symbol = rSymbol, Unit = rUnit, 
            Unit_Desc=ruDesc, Frequency=rMeasurementFrequency, SafeLevel=rSafeLevel,  Type = type, Latitude = latitude, Longitude = longitude};
            await _database.SaveItemAsync(sen);

        }
        else
        {
            Sensor sen = await _database.GetSpecificSensor(rQuantity);
            sen.Sensor_Quantity = rQuantity;
            sen.Symbol = rSymbol;
            sen.Unit = rUnit;
            sen.Unit_Desc = ruDesc;
            sen.Frequency = rMeasurementFrequency;
            sen.SafeLevel = rSafeLevel;
            sen.Type = type;
            sen.Latitude = latitude;
            sen.Latitude = latitude;
            await _database.UpdateSensorDataAsync(sen);

        }
          
    }
    public float smartParse (string value)
    {
        // this will return -1 is failed the parse
        // else just return regular val
        if(float.TryParse(value, out float resultA))
            return  float.Parse(value);
        else
            return  -1;
    }
    public async Task<string> initializeFullAirQuality()
    {
            // get air quality data from sample data
            //string path = FileSystem.AppDataDirectory;
            var DataFile = @"Data/Air_quality.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // the first parts of the sample data are data about the sensor itself
            //var siteName = lines.ElementAt(2).Split(',')[2];
            var latitude = lines.ElementAt(3).Split(',')[2];
            var longitude = lines.ElementAt(4).Split(',')[2];
            //var siteType = lines.ElementAt(5).Split(',')[2];
            //var zone = lines.ElementAt(6).Split(',')[2];
            //var agglomeration = lines.ElementAt(7).Split(',')[2];
            //var localAuthority = lines.ElementAt(8).Split(',')[2];

            List<string> Metadata = readMeta();
            // lines 2 - 5 / index 1 - 4 are about air quality sensors

            await  metaToDatabaseSensor(Metadata, 1, double.Parse(longitude),double.Parse(latitude) , "Air");// nitrogen sensor
            await  metaToDatabaseSensor(Metadata, 2, double.Parse(longitude),double.Parse(latitude) , "Air"); // sulphur dioxide sensor
            await  metaToDatabaseSensor(Metadata, 3, double.Parse(longitude),double.Parse(latitude) , "Air"); // pm2.5 sensor
            await  metaToDatabaseSensor(Metadata, 4, double.Parse(longitude),double.Parse(latitude) , "Air"); // pm10 sensor

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
                    // we check if there is a reading from these sensors at this time

                    bool checkReadingExists = await _database.DoesSensorReadingExist(dateString, "Nitrogen dioxide");

                    if (!checkReadingExists && dateString != "")
                    {
                        try
                        {
                            await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "Nitrogen dioxide", sensor_value=smartParse(values[2]), timestamp = dateString});
                            await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "Sulphur dioxide", sensor_value=smartParse(values[3]), timestamp = dateString});
                            await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "PM2.5 particulate matter (Hourly measured)", sensor_value=smartParse(values[4]), timestamp = dateString});
                            await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "PM10 particulate matter (Hourly measured)", sensor_value=smartParse(values[5]), timestamp = dateString});
                            
                        }
                        catch
                        {
                            throw new ArgumentOutOfRangeException("readings insert issue", " " + values[2]);
                        }

                    }
                }
                count++;
            }
            return lines.ElementAt(10);
    }
    public async Task<string> initializeFullWaterQuality()
    {
            // get air quality data from sample data
            //string path = FileSystem.AppDataDirectory; Resources\Raw\Data\Water_quality.csv
            var DataFile = @"Data\Water_quality.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // the first parts of the sample data are data about the sensor itself
            //var siteName = lines.ElementAt(0).Split(',')[1];
            //var samplePeriod = lines.ElementAt(1).Split(',')[1];
            var location = lines.ElementAt(2).Split(',')[1]; // water quality has one location value,

            // 55°51′40″N 3°15′14″W
            // into 55.94476 format
            string latV = location.Split(' ')[0];
            string longV = location.Split(' ')[1];

            // 55°51′40″N 
            var latitude = "";
            var longitude = "";

            int failslat = 0;
            // loops throgh the string and turns it into a sing double, by only adding the numbers to the new string, and identifing the location of the .
            for (int i = 0; i < latV.Length; i++)
            {
                string toCheck = "" + latV[i];

                if(int.TryParse(toCheck, out int result))
                {
                    latitude+=toCheck;
                }
                else
                {
                    if(failslat==0)
                        latitude += ".";
                    failslat++;
                }
            }

            int failslon =0;
            // loops throgh the string and turns it into a sing double, by only adding the numbers to the new string, and identifing the location of the .

            for (int i = 0; i < longV.Length; i++)
            {
                string toCheck = "" + longV[i];

                if(int.TryParse(toCheck, out int result))
                {
                    longitude+=toCheck;
                }
                else
                {
                    if(failslon==0)
                        longitude += ".";
                    failslon++;
                }
            }

            List<string> Metadata = readMeta();
            // lines 6 - 10 / index 5 - 9 are about water quality sensors

            await  metaToDatabaseSensor(Metadata, 5, double.Parse(longitude),double.Parse(latitude), "Water");// Nitrate (mg l-1)
            await  metaToDatabaseSensor(Metadata, 6, double.Parse(longitude),double.Parse(latitude), "Water");// Nitrite <mg l-1)
            await  metaToDatabaseSensor(Metadata, 7, double.Parse(longitude),double.Parse(latitude), "Water");// Phosphate (mg l-1)
            await  metaToDatabaseSensor(Metadata, 8, double.Parse(longitude),double.Parse(latitude), "Water");// EC (cfu/100ml)
            await  metaToDatabaseSensor(Metadata, 9, double.Parse(longitude),double.Parse(latitude), "Water");// intestinal enterococci

            // this produces an sensor reading for the sensor reading table
            // the water quality sample data has its actual readings start at line 6, or index 5
            // it stores these values Date	Time	Nitrate (mg l-1)	Nitrite <mg l-1)	Phosphate (mg l-1)	EC (cfu/100ml)

            //                         0   1            2                 3                4                           5
            int count = 0;
            foreach (var line in lines)
            {
                if (count >= 5)
                {
                    var values = line.Split(',');

                    string dateString = values[0] + " " + values[1];
                    // here is where we add the reading to the reading table in the database

                    // we check if there is a reading from these sensors at this time

                    bool checkReadingExists = await _database.DoesSensorReadingExist(dateString, "Nitrate (mg l-1)");

                    if (!checkReadingExists)
                    {
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "Nitrate (mg l-1)", sensor_value=smartParse(values[2]), timestamp = dateString});
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "Nitrite <mg l-1)", sensor_value=smartParse(values[3]), timestamp = dateString});
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "Phosphate (mg l-1)", sensor_value=smartParse(values[4]), timestamp = dateString});
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "EC (cfu/100ml)", sensor_value=smartParse(values[5]), timestamp = dateString});

                    }
                }
                count++;
            }
            return lines.ElementAt(10);
    }
    public async Task<string> initializeFullWeatherData()
    {
            // get air quality data from sample data
            //string path = FileSystem.AppDataDirectory;
            var DataFile = @"Data/Weather.csv";


            Task<List<string>> task = readCSV(DataFile);
            List<string> lines = task.Result;

            // the first parts of the sample data are data about the sensor itself
            var latitude = lines.ElementAt(1).Split(',')[0];
            var longitude = lines.ElementAt(1).Split(',')[1];


            List<string> Metadata = readMeta();
            // lines 11 - 14 / index 10 - 13 are about weather sensors

            await  metaToDatabaseSensor(Metadata, 10, double.Parse(longitude),double.Parse(latitude), "Weather");// air temp
            await  metaToDatabaseSensor(Metadata, 11, double.Parse(longitude),double.Parse(latitude), "Weather");// humidity
            await  metaToDatabaseSensor(Metadata, 12, double.Parse(longitude),double.Parse(latitude), "Weather");// wind speed
            await  metaToDatabaseSensor(Metadata, 13, double.Parse(longitude),double.Parse(latitude), "Weather");// wind direction


            // this produces an sensor reading for the sensor reading table
            // the weather sample data has its actual readings start at line 5, or index 4
            // it stores these values time	temperature_2m (¬∞C)	relative_humidity_2m (%)	wind_speed_10m (m/s)	wind_direction_10m (¬∞)	

            //                         0        1                        2                              3                  4  
            int count = 0;
            foreach (var line in lines)
            {
                if (count >= 4)
                {
                    // here is where we add the reading to the reading table in the database

                    var values = line.Split(',');

                    string dateString = values[0];

                    // we check if there is a reading from these sensors at this time
                    bool checkReadingExists = await _database.DoesSensorReadingExist(dateString, "temperature_2m (¬∞C)");

                    if (!checkReadingExists)
                    {
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "temperature_2m (¬∞C)", sensor_value=smartParse(values[1]), timestamp = dateString});
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "relative_humidity_2m (%)", sensor_value=smartParse(values[2]), timestamp = dateString});
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "wind_speed_10m (m/s)", sensor_value=smartParse(values[3]), timestamp = dateString});
                        await _database.SaveReadingAsync(new SensorReading { Sensor_Quantity = "wind_direction_10m (¬∞)", sensor_value=smartParse(values[4]), timestamp = dateString});

                    }


                }
                count++;
            }
            return lines.ElementAt(10);
    }
}


