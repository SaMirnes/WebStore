using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStoreProject
{
    public static class ExtentionMethods
    {
        public static bool ContainsCaseInsensitive(this string source, string substring)
        {
            return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}
