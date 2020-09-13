using System.Collections.Generic;

namespace SecureCleanerLibrary
{
    internal interface IUrlCleaner
    {
        string ClearSecureInRestLocation(string url, string secureKey);
        string ClearSecureInQueryLocation(string url, string secureKey);
    }
}