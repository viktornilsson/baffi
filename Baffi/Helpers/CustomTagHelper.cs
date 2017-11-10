using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Baffi.Helpers
{
    internal static class CustomTagHelper
    {
        internal static List<Type> GetAllCustomTagTypes()
        {
            var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            var type = typeof(ICustomTag);

            var allTypes = assemblyNames.Select(Assembly.Load)
                .SelectMany(t => t.GetTypes())
                .Where(p => p.GetTypeInfo().ImplementedInterfaces.Contains(type)).ToList();

            return allTypes;
        }

        internal static string[] GetTagParameters(string customTag)
        {
            char[] splitChars = { '=', '-', '&' };
            var parameters = customTag.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            return parameters.ToArray();
        }
    }
}
