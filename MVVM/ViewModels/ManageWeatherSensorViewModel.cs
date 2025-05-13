using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.MVVM.ViewModels
{
    public partial class ManageWeatherSensorViewModel : ObservableObject
    {
        public ObservableCollection<int> NumberOptions { get; } = new() { 1, 2, 3, 4, 5 };

        [ObservableProperty]
        private int selectedSensitivity1;

        [ObservableProperty]
        private int selectedSensitivity2;

        public ICommand SaveCommand { get; }

        public ManageWeatherSensorViewModel()
        {
            SelectedSensitivity1 = 1;
            SelectedSensitivity2 = 1;
            SaveCommand = new RelayCommand(SaveConfig);
        }

        private void SaveConfig()
        {
            // Save to Assessment2Db.db (ensure the table exists)
            using var conn = DatabaseConnectionManager.GetConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO WeatherSensorConfig (Sensitivity1, Sensitivity2) VALUES (@s1, @s2)";
            cmd.Parameters.AddWithValue("@s1", SelectedSensitivity1);
            cmd.Parameters.AddWithValue("@s2", SelectedSensitivity2);
            cmd.ExecuteNonQuery();
        }
    }
}
