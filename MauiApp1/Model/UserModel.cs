using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace MauiApp1.Model;

public class UserModel : ObservableObject
{
    [PrimaryKey]
    public string UserName {get; set;}
    public string Password {get;set;}
    public string UserType {get;set;}
}
