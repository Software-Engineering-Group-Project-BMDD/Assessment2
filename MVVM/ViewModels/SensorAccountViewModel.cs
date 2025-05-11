using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.MVVM.ViewModels
{
    public class SensorAccountViewModel : INotifyPropertyChanged
    {
        private double _firmwareUpdateProgress;
        private bool _isFirmwareUpdating;
        private string _firmwareUpdateStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        public SensorAccountViewModel()
        {
            StartFirmwareUpdateCommand = new Command(async () => await StartFirmwareUpdateAsync());
            FirmwareUpdateStatus = "Idle";
        }

        public double FirmwareUpdateProgress
        {
            get => _firmwareUpdateProgress;
            set
            {
                _firmwareUpdateProgress = value;
                OnPropertyChanged();
            }
        }

        public bool IsFirmwareUpdating
        {
            get => _isFirmwareUpdating;
            set
            {
                _isFirmwareUpdating = value;
                OnPropertyChanged();
            }
        }

        public string FirmwareUpdateStatus
        {
            get => _firmwareUpdateStatus;
            set
            {
                _firmwareUpdateStatus = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartFirmwareUpdateCommand { get; }

        private async Task StartFirmwareUpdateAsync()
        {
            IsFirmwareUpdating = true;
            FirmwareUpdateStatus = "Checking for updates...";
            await Task.Delay(2000);

            bool isUpdateAvailable = new Random().Next(0, 2) == 1;

            if (isUpdateAvailable)
            {
                FirmwareUpdateStatus = "Updating firmware...";
                for (int i = 1; i <= 10; i++)
                {
                    FirmwareUpdateProgress = i / 10.0;
                    await Task.Delay(300); // Simulate progress
                }

                FirmwareUpdateStatus = "Update completed successfully!";
            }
            else
            {
                FirmwareUpdateStatus = "Firmware is already up to date.";
            }

            IsFirmwareUpdating = false;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
