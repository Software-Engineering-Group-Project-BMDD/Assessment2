using System;
using System.Windows.Input;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.UI.View;
using MauiApp1.UI.Model;
//using Kotlin.Properties;
using System.Security.Cryptography.X509Certificates;

namespace MauiApp1.UI.ViewModel;
public class SensorStatusViewModel : ObservableObject
{
	
    private SensorDatabase _database;

    public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

    public IAsyncRelayCommand ViewStatusCommand { get; }
    
    public string SensorCount { get; set;} = "Hello world";



	public SensorStatusViewModel(SensorDatabase sensorDatabase)
    {
        _database = sensorDatabase;
        ViewStatusCommand = new AsyncRelayCommand(ViewSensorStatus);
        // SensorCount = " The number of sensors here are " + Sensors.Count().ToString();

	}
	
	public async void Init()
    {
        // this fetches all the sensor database enrtries and localises them to a sensor list
        var sensors = await _database.GetSensorsAsync();

        foreach (var sensor in sensors)
        {
            Sensors.Add(sensor);
        }
        SensorCount = " The number of sensors here are " + Sensors.Count().ToString();
    }
    public async Task<int> ViewSensorStatus()
    {
        var sensors = await _database.GetSensorsAsync();

        foreach (var sensor in sensors)
        {
            Sensors.Add(sensor);
        }

        SensorCount = " The number of sensors here are up to " + Sensors.Count().ToString();
        return 1;
    }
	void showAirStatus(string row)
	{
   		// it stores these values Date,Time,Nitrogen dioxide,Sulphur dioxide,PM2.5 particulate matter (Hourly measured),PM10 particulate matter (Hourly measured)
        //                         0   1    2                 3                4                                          5
        var values = row.Split(',');

        string nitro = values[2]; // nitrogen reading
        string sulphur = values[3]; // sulphur dioxide reading
        string pmp2 = values[4]; // pm2.5 reading
        string pm10 = values[5]; // pm10 reading

		
	}
	void showWaterStatus(string row)
	{
        // it stores these values Date	Time	Nitrate (mg l-1)	Nitrite <mg l-1)	Phosphate (mg l-1)	EC (cfu/100ml)

        //                         0   1            2                 3                4                           5  
        var values = row.Split(',');

        string nitrate = values[2]; // nitrogen reading
        string nitrite = values[3]; // sulphur dioxide reading
        string phosphate = values[4]; // pm2.5 reading
        string Ec = values[5]; // pm10 reading

	}
	void showWeatherStatus(string row)
	{
   		// it stores these values time	temperature_2m (¬∞C)	relative_humidity_2m (%)	wind_speed_10m (m/s)	wind_direction_10m (¬∞)	
        //                         0        1                        2                              3                  4         
        var values = row.Split(',');

        string temp = values[2]; // temperature_2m reading
        string humi = values[3]; // relative_humidity_2m reading
        string speed = values[4]; // wind_speed_10m reading
        string dir = values[5]; // wind_direction_10m reading

		
	}
	/// <summary>
	/// will get a specific row from the meta data table
	/// 
	/// the meta data sample data has these fields - Category, Quantity, Symbol, Unit, Unit description, Measurement frequency, Safe level, Reference, Sensor, URL
	/// </summary>

	public static string[] GetSensorRow(int row)
	{
		string[] outputFields = new string[10];

		return outputFields;
	}

	public Sensor[] GetLastReading()
	{
		
		Sensor[] readings = new Sensor[3];
		if (readSampleData.dbAvailable)
		{
			// when the database is connected, it will use this


			

			//readings[0] = airRead;
			//readings[1] = waterRead;
			//readings[2] = weatherRead;
		}
		
		return readings;
	}
	static int lastIndex (List<string> readingList)
	{
		int lastReading = readingList.Count() -1;
		while (readingList.ElementAt(lastReading).Length < 9)
		{
			lastReading--;
		}
		return lastReading;
	}
}