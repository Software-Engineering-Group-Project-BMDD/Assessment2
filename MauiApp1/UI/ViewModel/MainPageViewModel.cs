using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.UI.View;

namespace MauiApp1.UI.ViewModel;

public class MainPageViewModel : ObservableObject
{
    public MainPageViewModel()
    {
        NavigateToAdminCommand = new Command(NavigateToAdmin);
        NavigateToSensorLocation = new Command(NavigateSensorMap);
        ViewSensorStatusCommand = new Command(ViewSensorStatus);
        NavigateToTrendsCommand = new Command(NavigateToTrends);
        NavigateToLoginCommand = new Command(NavigateToLogin);
        NavigateToIncidentCommand = new Command(NavigateToIncidents);
    }

    public ICommand NavigateToAdminCommand { get; }
    public ICommand NavigateToSensorLocation { get; }

    public ICommand ViewSensorStatusCommand { get; }

    public ICommand NavigateToTrendsCommand {get;}
    public ICommand NavigateToLoginCommand {get;}
    public ICommand NavigateToIncidentCommand {get;}

    
    private void NavigateToAdmin()
    {
        Shell.Current.GoToAsync("//AdminView");
    }

    private void NavigateSensorMap()
    {
        Shell.Current.GoToAsync("//SensorView");
    }
    private void ViewSensorStatus()
    {    
        Shell.Current.GoToAsync("//SensorStatus");
    }

    private void NavigateToTrends()
    {    
        Shell.Current.GoToAsync("//TrendsView");
    }
    private void NavigateToLogin()
    {    
        Shell.Current.GoToAsync("//LoginView");
    }
    private void NavigateToIncidents()
    {    
        Shell.Current.GoToAsync("//IncidentView");
    }
}