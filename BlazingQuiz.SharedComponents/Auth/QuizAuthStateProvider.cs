using BlazingQuiz.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace BlazingQuiz.Shared.Components.Auth;

public class QuizAuthStateProvider : AuthenticationStateProvider
{
    private const string AuthType = "quiz-auth";
    private const string UserDataKey = "udata";
    private Task<AuthenticationState> _authStateTask;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly IStorageService _storageService;
    public bool IsInitializing { get; private set; } = true;

    public QuizAuthStateProvider(IJSRuntime jsRuntime, NavigationManager navigationManager, IStorageService storageService)
    {

        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        SetAuthStateTask();
        _storageService = storageService;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authStateTask;


    public LoggedInUser User { get; private set; }
    public bool IsLoggedIn => User?.Id > 0;

    public async Task SetLoginAsync(LoggedInUser user)
    {
        User = user;
        SetAuthStateTask();
        NotifyAuthenticationStateChanged(_authStateTask);
        await _storageService.SetItem(UserDataKey, user.ToJson());

    }

    public async Task SetLogoutAsync()
    {
        User = null;
        SetAuthStateTask();
        NotifyAuthenticationStateChanged(_authStateTask);
        await _storageService.RemoveItem(UserDataKey);
    }

    public async Task InitializeAsync()
    {
        await InitializeAsync(redirectToLogin: true);
    }

    public async Task<bool> InitializeAsync(bool redirectToLogin = true)
    {
        try
        {
            var udata = await _storageService.GetItem(UserDataKey);
            if (string.IsNullOrWhiteSpace(udata))
            {
                if (redirectToLogin)
                {
                    RedirectLogin();
                }
                return false;
            }

            var user = LoggedInUser.LoadFrom(udata);
            if (user == null || user.Id == 0)
            {
                if (redirectToLogin)
                {
                    RedirectLogin();
                }
                return false;
            }

            if (!IsTokenValid(user.Token))
            {
                if (redirectToLogin)
                {
                    RedirectLogin();
                }
                return false;
            }


            await SetLoginAsync(user);

            return true;
        }
        catch (Exception ex)
        {

        }
        finally
        {
            IsInitializing = false;
        }
        return false;
    }

    private void RedirectLogin()
    {
        _navigationManager.NavigateTo("auth/login");
    }

    private static bool IsTokenValid(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token)) // Invalid format
            return false;

        var jwt = handler.ReadJwtToken(token);
        var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
        if (expClaim == null)
            return false;

        var exp = long.Parse(expClaim.Value);
        var expUTCdatetime = DateTimeOffset.FromUnixTimeSeconds(exp).UtcDateTime;
        if (expUTCdatetime < DateTime.UtcNow)
            return false;

        return true;
    }


    private void SetAuthStateTask()
    {
        if (IsLoggedIn)
        {
            var identity = new ClaimsIdentity(User.ToClaims(), AuthType);
            var principal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(principal);

            _authStateTask = Task.FromResult(authState);
        }
        else
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(principal);
            _authStateTask = Task.FromResult(authState);
        }

    }

}
