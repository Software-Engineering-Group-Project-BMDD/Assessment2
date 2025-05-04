using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.UI.ViewModel;

public class AdminViewModel : ObservableObject
{

    private SensorDatabase _database;

    public AdminViewModel(SensorDatabase sensorDatabase)
    {
        _database = sensorDatabase;
        BackupCommand = new Command(Backup);
    }

    public ICommand BackupCommand { get; }

    private async void Backup()
    {
        await _database.Backup();
        MessagingCenter.Send(this, "ShowPopup", "Backup complete!");
    }

}