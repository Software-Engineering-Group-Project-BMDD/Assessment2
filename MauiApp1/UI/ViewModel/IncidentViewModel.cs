using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.UI.Model;

namespace MauiApp1.UI.ViewModel;

public class IncidentViewModel : ObservableObject
{

  private SensorDatabase _database;

    public ObservableCollection<LoginIncedentModel> Incidents { get; set; } = new ObservableCollection<LoginIncedentModel>();

    public ICommand ReturnCommand { get; set; }
    public ICommand CheckCommand { get; set; }

	public IncidentViewModel(SensorDatabase sensorDatabase)
    {
        _database = sensorDatabase;
		ReturnCommand = new Command(Back);
		CheckCommand = new Command(Check);

	}
	public async void Init()
    {
        
    }
	private void Back()
    {
        Shell.Current.GoToAsync("//MainPage");
    }
	private async void Check()
	{
		Incidents.Clear();
		var incidents = await _database.GetAllIncidentsAsync();

		for (int i = incidents.Count - 1; i > 0; i--)
		{
			Incidents.Add(incidents.ElementAt(i));
		}
	}
}