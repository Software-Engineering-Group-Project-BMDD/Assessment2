using System;
using SQLite;

namespace MauiApp1.UI.Model;

public class Sensor
{

    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

}
