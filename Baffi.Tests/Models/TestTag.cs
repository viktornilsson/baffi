﻿using Baffi.Models;

namespace Baffi.Tests.Models
{
    public class TestTag : ICustomTag
    {
        public string Process(params string[] parameters)
        {
            var val1 = parameters.Length > 0 ? int.Parse(parameters[0]) : 0;
            var val2 = parameters.Length > 1 ? int.Parse(parameters[1]) : 0;

            return $"FOO BAR {val1 * val2}";
        }
    }
}