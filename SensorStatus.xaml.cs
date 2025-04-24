namespace MauiApp1;

public partial class SensorStatus : ContentPage
{
	private readonly string _dbPath; // Declare the _dbPath field  

	public SensorStatus()
	{
		InitializeComponent();
		_dbPath = Path.Combine(FileSystem.AppDataDirectory, "Assessment2Db.db");

		var ia =  DatabaseSetup.initializeFullAirQuality();


		tester.Text = ia;

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

}