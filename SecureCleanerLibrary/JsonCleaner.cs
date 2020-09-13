using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("SecureCleanerLibraryTests")]

namespace SecureCleanerLibrary
{
    public class JsonCleaner: IJsonCleaner
    {
        private const string KeyGroupName = "Key";
        private const string ValueGroupName = "Value";
        private const string BeforeSecureGroupName = "BeforeSecure";
        private const string AfterSecureGroupName = "AfterSecure";
        private const char SecureSymbol = 'X';
        
        public string ClearSecureInElementAttributeLocation(string json, string secureKey)
        {
            var regex = new Regex($@"(?<{BeforeSecureGroupName}>{secureKey}\s*:\s*"")(?<{ValueGroupName}>[^""]*)(?<{AfterSecureGroupName}>"")");
            return regex.Replace(json, EncodeSecureData);
        }

        public string ClearSecureInValueElementLocation(string json, string secureKey)
        {
            var regex = new Regex(@$"(?<{BeforeSecureGroupName}>{secureKey}\s*:\s*{{\s*value\s*:\s*"")(?<{ValueGroupName}>[^""]*)(?<{AfterSecureGroupName}>""\s*}})");
            return regex.Replace(json, EncodeSecureData);
        }
        
        private string EncodeSecureData(Match match)
        {
            var beforeGroup = match.Groups[$"{BeforeSecureGroupName}"];
            var secure = match.Groups[$"{ValueGroupName}"];
            var afterGroup = match.Groups[$"{AfterSecureGroupName}"];
            return beforeGroup + new string(SecureSymbol, secure.Length) + afterGroup;
        }

    }
}