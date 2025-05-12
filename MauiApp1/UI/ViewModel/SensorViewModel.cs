using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.UI.Model;

namespace MauiApp1.UI.ViewModel;

public class SensorViewModel : ObservableObject
{

    private SensorDatabase _database;

    public ObservableCollection<Sensor> Sensors { get; set; } = new ObservableCollection<Sensor>();

    public IAsyncRelayCommand<Sensor> NavigateCommand { get; }
    public IAsyncRelayCommand<Sensor> ReportCommand { get; }

    public SensorViewModel(SensorDatabase sensorDatabase)
    {
        _database = sensorDatabase;

        NavigateCommand = new AsyncRelayCommand<Sensor>(NavigateToLocation);
        ReportCommand = new AsyncRelayCommand<Sensor>(Report);
    }

    public async void Init()
    {
        var sensors = await _database.GetSensorsAsync();

        foreach (var sensor in sensors)
        {
            Sensors.Add(sensor);
        }
    }

    private async Task NavigateToLocation(Sensor sensor)
    {
        string url = $"https://www.google.com/maps?q={sensor.Latitude},{sensor.Longitude}";
        await Launcher.OpenAsync(new Uri(url));
    }
    private async Task Report(Sensor sensor)
    {
        sensor.Flagged = !sensor.Flagged;
        await _database.SaveItemAsync(sensor);
    }

}
