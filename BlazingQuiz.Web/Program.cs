using BlazingQuiz.Shared;
using BlazingQuiz.Shared.Components.Auth;
using BlazingQuiz.Web;
using BlazingQuiz.Web.Apis;
using BlazingQuiz.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<QuizAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<QuizAuthStateProvider>());
builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<IAppState, AppState>();
builder.Services.AddSingleton<QuizState>()
    .AddSingleton<IStorageService, StorageService>();


ConfigureRefit(builder.Services);


await builder.Build().RunAsync();

static void ConfigureRefit(IServiceCollection services)
{
    const string ApiBaseUrl = "https://localhost:7292";

    services.AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<ICategoryApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IQuizApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IAdminApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IStudentQuizApi>(GetRefitSettings)
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