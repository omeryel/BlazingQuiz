using BlazingQuiz.Mobile.Services;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.Components.Auth;
using BlazingQuiz.Web.Apis;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Refit;

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

            builder.Services.AddSingleton<IStorageService, StorageService>()
                .AddSingleton<IAppState, AppState>();

            ConfigureRefit(builder.Services);

            return builder.Build();
        }


        static void ConfigureRefit(IServiceCollection services)
        {
            const string ApiBaseUrl = "https://localhost:7292";

            services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(SetHttpClient);


            static void SetHttpClient(HttpClient httpClient) =>
                httpClient.BaseAddress = new Uri(ApiBaseUrl);



            static RefitSettings GetRefitSettings(IServiceProvider sp)
            {
                var authStateProvider = sp.GetRequiredService<QuizAuthStateProvider>();

                return new RefitSettings
                {
                    AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(authStateProvider.User?.Token ?? string.Empty)
                };
            }
        }
    }
}
