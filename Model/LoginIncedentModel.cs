using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MauiApp1.UI.Model;
public class LoginIncedentModel
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    public string User {get; set;}
    public string timeStamp{get;set;}
}
