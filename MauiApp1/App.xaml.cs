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
