using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.UI.Model;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.ComponentModel;
using MauiApp1.Model;

namespace MauiApp1.UI.ViewModel;
public partial class SensorStatusViewModel : ObservableObject
{
	
    private SensorDatabase _database;


    public List<Sensor> allSensors {get; set;}
    
    
    public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

    public ICommand ViewStatusCommand { get; set; }
    public ICommand ReturnCommand { get; set; }

    public sensorPickable SensorType {get; set;} // This will get selectedItem instead of just a Text
    public List<sensorPickable> SensorTypes {get; set;} = new()
    {new sensorPickable("Air"), 
    new sensorPickable("Water"), 
    new sensorPickable("Weather")};

    public struct sensorPickable
    {
        public sensorPickable(string text)
        {
            DisplayText = text;
        }
        public string DisplayText {get; set;}
    }

    [ObservableProperty]
    string sensorsSelectedLabel = "selected";
    //public string SensorsSelectedLabel {get; set;} = "Select";


    

	public SensorStatusViewModel(SensorDatabase sensorDatabase)
    {
        _database = sensorDatabase;
        ViewStatusCommand = new Command(ViewSensorStatus);
        ReturnCommand = new Command(Back);
        //SensorCount = " The number of sensors here are " + Sensors.Count().ToString();

	}
	public async void Init()
    {
        var sensors = await _database.GetSensorsAsync();
        allSensors = new List<Sensor>();

        foreach (Sensor sensor in sensors)
        {
            allSensors.Add(sensor);
        }

    }
    private void  ViewSensorStatus()
    {
        //var sensors = await _database.GetSensorsAsync();
        SensorsSelectedLabel = SensorType.DisplayText;

        switch(SensorType.DisplayText)
        {
            case "Air": showAirStatus(); break;
            case "Water": showWaterStatus(); break;
            case "Weather": showWeatherStatus(); break;
            default: break;
        }
    }
    private void Back()
    {
        Shell.Current.GoToAsync("//MainPage");
    }

	async void showAirStatus()
	{
   		// it stores these values Date,Time,Nitrogen dioxide,Sulphur dioxide,PM2.5 particulate matter (Hourly measured),PM10 particulate matter (Hourly measured)
        //                         0   1    2                 3                4                                          5
        Sensors.Clear();


        SensorReading a = await _database.GetFinalSensorReadingAsync("Nitrogen dioxide");
        SensorReading b = await _database.GetFinalSensorReadingAsync("Sulphur dioxide");
        SensorReading c = await _database.GetFinalSensorReadingAsync("PM2.5 particulate matter (Hourly measured)");
        SensorReading d = await _database.GetFinalSensorReadingAsync("PM10 particulate matter (Hourly measured)");


        SensorReading[] readings = [a,b,c,d];
        int count=0;
        foreach (Sensor sensor in allSensors)
        {
            if(sensor.Type == "Air")
            {
                if (readings[count].sensor_value == 0)
               {
                   sensor.Flagged = true;
               }
               count++;
               Sensors.Add(sensor);

           }
            SensorsSelectedLabel =count.ToString();
        }
	}   
	async void showWaterStatus()
	{
        // it stores these values Date	Time	Nitrate (mg l-1)	Nitrite <mg l-1)	Phosphate (mg l-1)	EC (cfu/100ml)

        //                         0   1            2                 3                4                           5  
    


        Sensors.Clear();


        SensorReading a = await _database.GetFinalSensorReadingAsync("Nitrogen dioxide");
        SensorReading b = await _database.GetFinalSensorReadingAsync("Sulphur dioxide");
        SensorReading c = await _database.GetFinalSensorReadingAsync("PM2.5 particulate matter (Hourly measured)");
        SensorReading d = await _database.GetFinalSensorReadingAsync("PM10 particulate matter (Hourly measured)");


		SensorReading[] readings = new SensorReading[]{a,b,c,d};
        int count=0;
        foreach (Sensor sensor in allSensors)
        {
            if(sensor.Type == "Water" && count < 4)
            {
                if (readings[count].sensor_value == 0)
                {
                    sensor.Flagged = true;
                }
                count++;
                Sensors.Add(sensor);

            }
            
        }
	}
	async void showWeatherStatus()
	{
   		// it stores these values time	temperature_2m (¬∞C)	relative_humidity_2m (%)	wind_speed_10m (m/s)	wind_direction_10m (¬∞)	
        //                         0        1                        2                              3                  4         
    
        Sensors.Clear();

        SensorReading a = await _database.GetFinalSensorReadingAsync("temperature_2m (¬∞C)");
        SensorReading b = await _database.GetFinalSensorReadingAsync("relative_humidity_2m (%)");
        SensorReading c = await _database.GetFinalSensorReadingAsync("wind_speed_10m (m/s)");
        SensorReading d = await _database.GetFinalSensorReadingAsync("wind_direction_10m (¬∞)");


		SensorReading[] readings = new SensorReading[]{a,b,c,d};
        int count=0;
        foreach (Sensor sensor in allSensors)
        {
            if(sensor.Type == "Weather")
            {
                if (readings[count].sensor_value == 0)
                {
                    sensor.Flagged = true;
                }
                count++;
                Sensors.Add(sensor);

            }
            
        }
		
    }
}