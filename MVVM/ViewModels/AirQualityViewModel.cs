using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp1.MVVM.ViewModels
{
    public partial class AirQualityViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isRefreshing;

        public ObservableCollection<AirQualityData> AirQualityItems { get; } = new();

        public AirQualityViewModel()
        {
            RefreshCommand = new RelayCommand(RefreshData);
            LoadInitialData();
        }

        public ICommand RefreshCommand { get; }

        private void LoadInitialData()
        {
            // Simulate loading initial data
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "10:00", PM25 = 12.5, PM10 = 25.0, CO = 0.8 });
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "11:00", PM25 = 15.0, PM10 = 30.0, CO = 1.0 });
        }

        private void RefreshData()
        {
            IsRefreshing = true;

            // Simulate refreshing data
            AirQualityItems.Clear();
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "12:00", PM25 = 10.0, PM10 = 20.0, CO = 0.7 });
            AirQualityItems.Add(new AirQualityData { Date = "2025-05-11", Time = "13:00", PM25 = 11.5, PM10 = 22.0, CO = 0.9 });

            IsRefreshing = false;
        }
    }

    public class AirQualityData
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public double PM25 { get; set; }
        public double PM10 { get; set; }
        public double CO { get; set; }
    }
}
