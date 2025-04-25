using System;

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



                    Console.WriteLine($"Column1: {values[0]}, Column2: {values[1]}");
                }
            }
            return lines.ElementAt(10);
    }
}


