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
    }

    public ICommand NavigateToAdminCommand { get; }

    private void NavigateToAdmin()
    {
        Shell.Current.GoToAsync("//AdminView");
    }

}