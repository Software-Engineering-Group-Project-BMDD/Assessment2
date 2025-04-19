namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnNewPage1Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("NewPage1");
        }

        private async void OnScheduleMaintenanceClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ScheduleMaintenance");
        }

        private async void OnManageAccessClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ManageAccess");
        }
    }

}
