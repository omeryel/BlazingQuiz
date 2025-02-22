using BlazingQuiz.Shared;
using Microsoft.JSInterop;

namespace BlazingQuiz.Web.Services
{
    public class StorageService : IStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        public StorageService(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }

        public async ValueTask<string?> GetItem(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async ValueTask RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

        public async ValueTask SetItem(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
    }
}
