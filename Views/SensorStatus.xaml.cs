using System.Globalization;
using System.Data.SqlClient;

namespace MauiApp1.Views;

public partial class SensorStatus : ContentPage
{
	private readonly string _dbPath; // Declare the _dbPath field  

 	public SensorStatus()
	{
		InitializeComponent();

		if(DatabaseConnectionManager.isDatabaseAvailable)
		{
			// this is for when we can 
			_dbPath = Path.Combine(FileSystem.AppDataDirectory, "Assessment2Db.db");

			var ia =  readSampleData.initializeFullAirQuality();


		}
		else
		{
			
		}

	}

	public void Review_Btn_Clicked(object sender, EventArgs e)
	{
		int selectionIndex = sensorPicker.SelectedIndex;
		
		string[] readings = GetLastReading();
		string readingToShow="";

		switch(selectionIndex)
		{
			case 0:SensorsSelectedLabel.Text="Air"; showAirStatus(readings[0]); break;
			case 1:SensorsSelectedLabel.Text="Water"; showWaterStatus(readings[1]);  break;
			case 2:SensorsSelectedLabel.Text="Weather"; showWeatherStatus(readings[2]);  break;
			default: SensorsSelectedLabel.Text="not selected";break;
		}



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

		if(nitro != "")
			nitro = "Active";
		else
			nitro = "Inactive";
		s1Label.Text = "Nitrogen Sensor";
		s1Value.Text = nitro;

		if(sulphur != "")
			sulphur = "Active";
		else
			sulphur = "Inactive";
		s2Label.Text = "Sulpher Sensor";
		s2Value.Text = sulphur;

		if(pmp2 != "")
			pmp2 = "Active";
		else
			pmp2 = "Inactive";
		s3Label.Text = "PM2.5 particulate matter Sensor";
		s3Value.Text = pmp2;

		if(pm10 != "")
			pm10 = "Active";
		else
			pm10 = "Inactive";
		s4Label.Text = "PM10 particulate matter Sensor";
		s4Value.Text = pm10;

		s5Label.Text="";
		s5Value.Text="";

		s6Label.Text="";
		s6Value.Text="";
		s7Label.Text="";
		s7Value.Text="";
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

		if(nitrate != "")
			nitrate = "Active";
		else
			nitrate = "Inactive";
		s1Label.Text = "Nitrogen Sensor";
		s1Value.Text = nitrate;

		if(nitrite != "")
			nitrite = "Active";
		else
			nitrite = "Inactive";
		s2Label.Text = "Sulpher Sensor";
		s2Value.Text = nitrite;

		if(phosphate != "")
			phosphate = "Active";
		else
			phosphate = "Inactive";
		s3Label.Text = "PM2.5 particulate matter Sensor";
		s3Value.Text = phosphate;

		if(Ec != "")
			Ec = "Active";
		else
			Ec = "Inactive";
		s4Label.Text = "PM10 particulate matter Sensor";
		s4Value.Text = Ec;

		s5Label.Text="";
		s5Value.Text="";

		s6Label.Text="";
		s6Value.Text="";
		s7Label.Text="";
		s7Value.Text="";
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

		if(temp != "")
			temp = "Active";
		else
			temp = "Inactive";
		s1Label.Text = "Temperature 2m Sensor";
		s1Value.Text = temp;

		if(humi != "")
			humi = "Active";
		else
			humi = "Inactive";
		s2Label.Text = "Relative humidity 2m Sensor";
		s2Value.Text = humi;

		if(speed != "")
			speed = "Active";
		else
			speed = "Inactive";
		s3Label.Text = "Wind speed 10m Sensor";
		s3Value.Text = speed;

		if(dir != "")
			dir = "Active";
		else
			dir = "Inactive";
		s4Label.Text = "Wind direction 10m Sensor";
		s4Value.Text = dir;

		s5Label.Text="";
		s5Value.Text="";

		s6Label.Text="";
		s6Value.Text="";
		s7Label.Text="";
		s7Value.Text="";
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

	public string[] GetLastReading()
	{
		bool useDatabse = true;
		string[] readings = new string[3];
		if (useDatabse)
		{
			// when the database is connected, it will use this
			string airRead = ",,";
			string waterRead = ",,";
			string weatherRead = ",";

			var connectionString = DatabaseConnectionManager.GetDatabasePath();
			using var connection = new SqlConnection(connectionString);
			connection.Open();
			Console.WriteLine("Connection successful!");

			readings[0] = airRead;
			readings[1] = waterRead;
			readings[2] = weatherRead;
		}
		else
		{
			// for testing, we just use the sample csv
			List<string> airCSV = readSampleData.readCSV(@"Data/Air_quality.csv").Result;
			List<string> waterCSV = readSampleData.readCSV(@"Data/Water_quality.csv").Result;
			List<string> weatherCSV = readSampleData.readCSV(@"Data/Weather.csv").Result;

			readings[0] = airCSV.ElementAt(lastIndex(airCSV));
			readings[1] = waterCSV.ElementAt(lastIndex(waterCSV));
			readings[2] = weatherCSV.ElementAt(lastIndex(weatherCSV));

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