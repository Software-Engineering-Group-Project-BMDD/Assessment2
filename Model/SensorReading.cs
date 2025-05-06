using System;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MauiApp1.Model;

public class SensorReading : ObservableObject
{

    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
   
    public string Sensor_Quantity { get; set; }
    public float  sensor_value { get; set; }
    public string  timestamp { get; set; }
    public float  sensor_setpoint { get; set; }


}
