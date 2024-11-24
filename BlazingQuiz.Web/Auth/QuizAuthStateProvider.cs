using BlazingQuiz.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace BlazingQuiz.Web.Auth
{
    public class QuizAuthStateProvider : AuthenticationStateProvider
    {
        private const string AuthType = "quiz-auth";
        private const string UserDataKey = "udata";
        private Task<AuthenticationState> _authStateTask;
        private readonly IJSRuntime _jsRuntime;
        public bool IsInitializing { get; private set; } = true;

        public QuizAuthStateProvider(IJSRuntime jsRuntime)
        {

            _jsRuntime = jsRuntime;
            SetAuthStateTask();
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authStateTask;


        public LoggedInUser User { get; private set; }
        public bool IsLoggedIn => User?.Id > 0;

        public async Task SetLoginAsync(LoggedInUser user)
        {
            User = user;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserDataKey, user.ToJson());
        }

        public async Task SetLogoutAsync()
        {
            User = null;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserDataKey);
        }

        public async Task InitializeAsync()
        {
            try
            {
                var udata = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", UserDataKey);
                if (string.IsNullOrWhiteSpace(udata))
                {
                    return;
                }

                var user = LoggedInUser.LoadFrom(udata);
                if (user == null || user.Id == 0)
                {
                    return;
                }

                await SetLoginAsync(user);
            }
            finally
            {
                IsInitializing = false;

            }

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




}
