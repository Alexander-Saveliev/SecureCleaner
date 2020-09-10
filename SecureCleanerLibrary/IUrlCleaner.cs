using System.Collections.Generic;

namespace SecureCleanerLibrary
{
    internal interface IUrlCleaner
    {
        string ClearSecure(string url, string secureKey, SecureLocationType locationType);
    }
}