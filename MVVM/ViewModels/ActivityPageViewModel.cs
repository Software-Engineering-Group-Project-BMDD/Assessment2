using System.Windows.Input;
using Microsoft.Maui.Controls.Compat;

namespace MauiApp1.MVVM.ViewModels
{
    public class ActivityPageViewModel
    {
        public ICommand NavigateCommand { get; }
        public ICommand RefreshCommand { get; }

        public ActivityPageViewModel()
        {
            // Single command for navigation
            NavigateCommand = new Command<string>(async (pageName) => await NavigateToPageAsync(pageName));
        }

        private async Task NavigateToPageAsync(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                await Shell.Current.GoToAsync(pageName);
            }
        }

        
    }
}
