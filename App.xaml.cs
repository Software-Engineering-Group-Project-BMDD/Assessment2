using MauiApp1.Views;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App(SensorDatabase sensorDatabase)
        {
            InitializeComponent();

<<<<<<< HEAD
            MainPage = new SensorStatus();
=======
            MainPage = new AppShell();

            Task.Run(async () => await sensorDatabase.Populate());
>>>>>>> fc07a982b49686ed4aeb55eb0431f7097b59f3c6
        }
    }
}
