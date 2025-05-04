using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MauiApp1.UI.Model;

public class Sensor : ObservableObject
{

    private bool _flagged;

    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool Flagged
    {
        get => _flagged;
        set => SetProperty(ref _flagged, value);
    }

}
