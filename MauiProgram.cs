using Microsoft.Extensions.Logging;

namespace MauiApp1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Platform guards to ensure compatibility
#if IOS || MACCATALYST || ANDROID
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
#endif

        // Register pages
        builder.Services.AddSingleton<MainPage>();

        // Register interfaces and their implementations
        builder.Services.AddSingleton<IDatabaseConnectionChecker, DatabaseConnectionChecker>();
        builder.Services.AddSingleton<IDatabaseConnectionManager, DatabaseConnectionManager>();
        builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
        builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
