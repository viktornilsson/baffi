using System;
using System.Collections.Generic;

namespace Baffi.Configuration
{
    public class BaffiConfigurationExpression : IBaffiConfigurationExpression
    {
        public readonly List<Type> CustomTypes = new List<Type>();

        public BaffiConfigurationExpression AddTag<T>() where T : ICustomTag
        {
            var type = typeof(T);

            if (CustomTypes.Contains(type))
                return this;

            CustomTypes.Add(type);
            return this;
        }
    }
}
