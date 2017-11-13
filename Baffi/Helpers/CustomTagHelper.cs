using System;
using System.Linq;

namespace Baffi.Helpers
{
    internal static class CustomTagHelper
    {
        internal static string[] GetTagParameters(string customTag)
        {
            char[] splitChars = { '&' };
            var parameters = customTag.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            return parameters.ToArray();
        }
    }
}
