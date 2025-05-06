using MauiApp1.UI.View;
using MauiApp1.UI.ViewModel;
using MauiApp1.UI.Model;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App(SensorDatabase sensorDatabase)
        {
            InitializeComponent();

            MainPage = new AppShell();

            Task.Run(async () => await sensorDatabase.Populate());
        }
    }
}
