using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Baffi.Models;
using Microsoft.Extensions.DependencyModel;

namespace Baffi.Helpers
{
    public static class CustomTagHelper
    {
        public static List<Type> GetAllCustomTagTypes()
        {
            var asmNames = DependencyContext.Default.GetDefaultAssemblyNames();
            var type = typeof(ICustomTag);

            var allTypes = asmNames.Select(Assembly.Load)
                .SelectMany(t => t.GetTypes())
                .Where(p => p.GetTypeInfo().ImplementedInterfaces.Contains(type)).ToList();

            return allTypes;
        }

        public static string[] GetTagParameters(string customTag)
        {
            char[] splitChars = { '=', '-', '&' };
            var parameters = customTag.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

            return parameters.ToArray();
        }
    }
}
