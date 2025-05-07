using MauiApp1.MVVM.ViewModels;
using Microsoft.Logging;

namespace MauiApp1;

public static class MauiProgram
{
    public static MauiApp CreateMauiProgram() // Corrected return type to MauiApp
    {
        var builder = MauiApp.CreateBuilder(); // Corrected to MauiApp.CreateBuilder()
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<SensorViewModel>();
        builder.Services.AddSingleton<SensorAccountPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
