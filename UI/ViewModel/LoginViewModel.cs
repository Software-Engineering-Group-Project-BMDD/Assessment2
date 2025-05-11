using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Model;
using Windows.System;

namespace MauiApp1.UI.ViewModel;

public partial class LoginViewModel : ObservableObject
{
	private SensorDatabase _database;
	public string Username{get;set;}
	public string Password{get;set;}
	[ObservableProperty]
	string warningText = "warn";

	public ICommand LoginCommand {get;}

	private List<UserModel> Users = new List<UserModel>();

	public LoginViewModel(SensorDatabase sensorDatabase)
	{
		_database = sensorDatabase;
		LoginCommand = new Command(loginAttempt);
	}

	public async void Init()
    {
        var users = await _database.GetAllUsers();

        foreach (UserModel um in users)
        {
            Users.Add(um);
        }
    }
	private async void loginAttempt()
	{
        WarningText = "Attempting Login " + Username + " " + Password;
		
		bool found = false;
		foreach (UserModel um in Users)
		{
			if (um.UserName == Username && um.Password == Password)
				found=true;
		}

		if( found)
		{
			await Shell.Current.GoToAsync("//MainPage");
		}
	}
}