using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingQuiz.Shared
{
    public interface IStorageService
    {
        ValueTask SetItem(string key, string value);
        ValueTask<string> GetItem(string key);

        ValueTask RemoveItem(string key);
    }
}
