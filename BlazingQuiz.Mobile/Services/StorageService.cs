using BlazingQuiz.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Mobile.Services
{
    public class StorageService : IStorageService
    {
        public async ValueTask<string?> GetItem(string key)
        {
            return await ValueTask.FromResult(Preferences.Default.Get<string?>(key, null));
        }

        public ValueTask RemoveItem(string key)
        {
            Preferences.Default.Remove(key);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetItem(string key, string value)
        {
            Preferences.Default.Set<string>(key, value);
            return ValueTask.CompletedTask;
        }
    }
}
