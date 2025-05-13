using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp1.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for managing sensor account operations, including firmware updates and navigation.
    /// </summary>
    public class SensorAccountViewModel : INotifyPropertyChanged
    {
        private double _firmwareUpdateProgress;
        private bool _isFirmwareUpdating;
        private string _firmwareUpdateStatus;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorAccountViewModel"/> class.
        /// </summary>
        public SensorAccountViewModel()
        {
            StartFirmwareUpdateCommand = new Command(async () => await StartFirmwareUpdateAsync());
            NavigateCommand = new Command<string>(async (pageName) => await NavigateToPageAsync(pageName));
            FirmwareUpdateStatus = "Idle";
        }

        /// <summary>
        /// Gets or sets the progress of the firmware update as a value between 0 and 1.
        /// </summary>
        public double FirmwareUpdateProgress
        {
            get => _firmwareUpdateProgress;
            set
            {
                _firmwareUpdateProgress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a firmware update is currently in progress.
        /// </summary>
        public bool IsFirmwareUpdating
        {
            get => _isFirmwareUpdating;
            set
            {
                _isFirmwareUpdating = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the status message of the firmware update process.
        /// </summary>
        public string FirmwareUpdateStatus
        {
            get => _firmwareUpdateStatus;
            set
            {
                _firmwareUpdateStatus = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the command to start the firmware update process.
        /// </summary>
        public ICommand StartFirmwareUpdateCommand { get; }

        /// <summary>
        /// Gets the command to navigate to a specified page.
        /// </summary>
        public ICommand NavigateCommand { get; }

        /// <summary>
        /// Starts the firmware update process asynchronously.
        /// </summary>
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

        /// <summary>
        /// Navigates to the specified page asynchronously.
        /// </summary>
        /// <param name="pageName">The name of the page to navigate to.</param>
        private async Task NavigateToPageAsync(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                await Shell.Current.GoToAsync(pageName);
            }
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
