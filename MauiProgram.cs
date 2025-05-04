using MauiApp1.UI.View;
using MauiApp1.UI.ViewModel;
using Microsoft.Extensions.Logging;

namespace MauiApp1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<SensorDatabase>();
		builder.Services.AddSingleton<App>();

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddTransient<AdminViewModel>();
		builder.Services.AddSingleton<AdminView>();

		return builder.Build();
	}
}
