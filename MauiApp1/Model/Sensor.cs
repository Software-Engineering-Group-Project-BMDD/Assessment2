using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MauiApp1.UI.Model;

public class Sensor : ObservableObject
{

    //string insertQuery = 
    //        "INSERT INTO Sensor (Sensor_Quantity, Symbol, Unit, Unit_Desc, Frequency, SafeLevel, Longitude,Latitude,sensor_type) " +
    //        "VALUES (@Sensor_Quantity, @Symbol, @Unit, @Unit_Desc, @Frequency, @SafeLevel, @Longitude, @Latitude, @sensor_type)";    
    private bool _flagged;

    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string Sensor_Quantity { get; set; }
    public string Symbol { get; set; }
    public string Unit { get; set; }
    public string Unit_Desc { get; set; }
    public string Frequency { get; set; }
    public string SafeLevel { get; set; }
    public string Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool Flagged
    {
        get => _flagged;
        set => SetProperty(ref _flagged, value);
    }
}
