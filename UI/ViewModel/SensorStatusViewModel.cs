using System;
using System.Windows.Input;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.UI.View;
using MauiApp1.UI.Model;

namespace MauiApp1.UI.ViewModel;
public class SensorStatusViewModel : ObservableObject
{
	
    private SensorDatabase _database;

    public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

	public SensorStatusViewModel(SensorDatabase sensorDatabase)
    {
        _database = sensorDatabase;

	}
	
	public async void Init()
    {
        var sensors = await _database.GetSensorsAsync();

        foreach (var sensor in sensors)
        {
            Sensors.Add(sensor);
        }
    }
}