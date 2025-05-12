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

public class TrendReportViewModel : ObservableObject
{
	public ObservableCollection<Analysis> AnalysisCollection { get; set; } = new ObservableCollection<Analysis>();
	private SensorDatabase _database;

	public ICommand ReturnCommand { get; set; }

	public TrendReportViewModel(SensorDatabase sensorDatabase)
	{
		_database = sensorDatabase;
        ReturnCommand = new Command(Back);

	}

	 private void Back()
    {
        Shell.Current.GoToAsync("//MainPage");
    }

	public async void Init()
    {
		List<Sensor> sensors = await _database.GetSensorsAsync();

		foreach (Sensor sen in sensors)
		{
			// we get the readings for each sensor, and "perform analysis"
			List<SensorReading> readings = await _database.GetAllReadingsOfQuantity(sen.Sensor_Quantity);
			Analysis result = performAnalysis(readings).Result;

			result.Quantity = sen.Sensor_Quantity;

			AnalysisCollection.Add(result);
		}
    }
	public async Task<Analysis> performAnalysis(List<SensorReading> readings)
	{
		Analysis result = new Analysis();

		// get average
		if(readings.Count > 0)
		{

			float total = 0;
			float count = 0;
			float lowest = 1000000000000000000;
			float highest = 0;
			result.TotalChange = readings.ElementAt(readings.Count - 1).sensor_value -  readings.ElementAt(0).sensor_value; // gets the total change

			foreach (SensorReading read in readings)
			{
				float curVal = read.sensor_value;
				total += curVal;
				count++;

				if(curVal > highest)
					highest = curVal;
				if(curVal< lowest && curVal != -1)
					lowest = curVal;

			}
			result.AverageChange = total / count;
			result.Lowest = lowest;
			result.Highest = highest;
			
		}

		
		return result;
	}
	public struct Analysis
	{

		public string Quantity {get; set;}
		public float AverageChange {get; set;}

		public float TotalChange {get; set;}
		public float Highest {get; set;}
		public float Lowest {get; set;}
	}
}