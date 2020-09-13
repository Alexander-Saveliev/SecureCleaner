using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("SecureCleanerLibraryTests")]

namespace SecureCleanerLibrary
{
    internal class UrlCleaner: IUrlCleaner
    {
        private const string KeyGroupName = "Key";
        private const string ValueGroupName = "Value";
        private const char SecureSymbol = 'X';
        

        public string ClearSecureInRestLocation(string url, string secureKey)
        {
            var regex = new Regex(@$"(?<{KeyGroupName}>/{secureKey}/)(?<{ValueGroupName}>[^/?]*)");
            return regex.Replace(url, EncodeSecureData);
        }
        
        public string ClearSecureInQueryLocation(string url, string secureKey)
        {
            var regex = new Regex($@"(?<{KeyGroupName}>(\?|\&){secureKey}=)(?<{ValueGroupName}>[^\&]*)");
            return regex.Replace(url, EncodeSecureData);
        }

        private string EncodeSecureData(Match match)
        {
            var key = match.Groups[$"{KeyGroupName}"];
            var secure = match.Groups[$"{ValueGroupName}"];
            return key + new string(SecureSymbol, secure.Length);
        }
    }
}