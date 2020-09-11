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
        public string ClearSecure(string url, string secureKey, SecureLocationType locationType)
        {
            var regex = BuildRegexByLocationWithKey(locationType, secureKey);
            return regex.Replace(url, EncodeSecureData);
        }

        private Regex BuildRegexByLocationWithKey(SecureLocationType locationType, string secureKey)
        {
            switch (locationType)
            {
                case SecureLocationType.Rest:
                    return new Regex(@$"(?<{KeyGroupName}>/{secureKey}/)(?<{ValueGroupName}>[^/?]*)");
                case SecureLocationType.Query:
                    return new Regex($@"(?<{KeyGroupName}>(\?|\&){secureKey}=)(?<{ValueGroupName}>[^\&]*)");
                default:
                    throw new NotImplementedException($"SecureLocation: {locationType.ToString()}");
            }
        }
  
        private string EncodeSecureData(Match match)
        {
            var key = match.Groups[$"{KeyGroupName}"];
            var secure = match.Groups[$"{ValueGroupName}"];
            return key + new string(SecureSymbol, secure.Length);
        }
    }
}