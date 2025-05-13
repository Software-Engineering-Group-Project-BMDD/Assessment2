using System.Windows.Input;
using Microsoft.Maui.Controls.Compat;

namespace MauiApp1.MVVM.ViewModels
{
    /// <summary>
    /// ViewModel for the Activity Page, providing commands for navigation and refresh functionality.
    /// </summary>
    public class ActivityPageViewModel
    {
        /// <summary>
        /// Gets the command used to navigate to a specified page.
        /// </summary>
        public ICommand NavigateCommand { get; }

        /// <summary>
        /// Gets the command used to refresh the page (currently not implemented).
        /// </summary>
        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityPageViewModel"/> class.
        /// </summary>
        public ActivityPageViewModel()
        {
            // Single command for navigation
            NavigateCommand = new Command<string>(async (pageName) => await NavigateToPageAsync(pageName));
        }

        /// <summary>
        /// Navigates to the specified page asynchronously.
        /// </summary>
        /// <param name="pageName">The name of the page to navigate to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task NavigateToPageAsync(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                await Shell.Current.GoToAsync(pageName);
            }
        }
    }
}
