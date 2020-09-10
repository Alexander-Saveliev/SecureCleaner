using System.Collections.Generic;

namespace SecureCleanerLibrary
{
    public interface ISecureCleaner
    {
        HttpResult CleanHttpResult(HttpResult httpResult);
    }
}