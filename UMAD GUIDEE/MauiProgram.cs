using Microsoft.Extensions.Logging;
using UMAD_GUIDEE.Services;
using UMAD_GUIDEE.ViewModels;
using UMAD_GUIDEE.Views;

namespace UMAD_GUIDEE;

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
			}).Services.AddSingleton<HttpClient>(client =>
			{
				// Cambiar la IP - ipconfig
				return new HttpClient { BaseAddress = new Uri("http://172.16.4.157:7184/api/") };
			});

            builder.Services.AddSingleton<HttpService>()
            .AddTransient<LogInViewModel>()
			.AddTransient<NotesViewModel>()
			.AddTransient<NoteViewModel>()
			.AddTransient<LogInView>()
			.AddTransient<NotesView>()
			.AddTransient<NoteView>()
			.AddTransient<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
