using BlazingQuiz.Shared.Components.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BlazingQuiz.Mobile
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddSingleton<QuizAuthStateProvider>();
            builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<QuizAuthStateProvider>());
            builder.Services.AddAuthorizationCore();

            return builder.Build();
        }
    }
}
