using MauiApp1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.MVVM.ViewModels
{
    class SensorViewModel : BaseViewModel
    {
        private ObservableCollection<SensorViewModel> _sensors;
        public ObservableCollection<SensorViewModel> Sensors
        {
            get { return _sensors; }
            set { SetProperty(ref _sensors, value); }
        }

        public ICommand RefreshCommnand { get; } // Command to refresh the sensor data
        public string Name { get; private set; }
        public string Value { get; private set; }

        public SensorViewModel()
        {
            Sensors = new ObservableCollection<SensorViewModel>();
            Command RefreshCommand = new(RefreshSensors);
            RefreshCommnand = RefreshCommand;
        }

        private void LoadingSensor()
        {
            Sensors.Clear();
            Sensors.Add(new SensorViewModel { Name = "Sensor 1", Value = "Value 1" });
            Sensors.Add(new SensorViewModel { Name = "Sensor 2", Value = "Value 2" });
        }

        private void RefreshSensors() // Add this method to fix CS0103
        {
            // Logic to refresh sensor data
        }
    }
}
