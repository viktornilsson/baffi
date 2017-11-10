using Baffi.Configuration;

namespace Baffi
{
    public interface IBaffiConfigurationExpression
    {       
        BaffiConfigurationExpression AddTag<T>() where T : ICustomTag;
    }
}
