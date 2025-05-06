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

    }

    public ICommand NavigateToAdminCommand { get; }
    public ICommand NavigateToSensorLocation { get; }

    public ICommand ViewSensorStatusCommand { get; }

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
}