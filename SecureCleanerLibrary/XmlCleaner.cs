using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("SecureCleanerLibraryTests")]

namespace SecureCleanerLibrary
{
    public class XmlCleaner: IXmlCleaner
    {
        private const string KeyGroupName = "Key";
        private const string ValueGroupName = "Value";
        private const char SecureSymbol = 'X';
        
        public string ClearSecureInElementValueLocation(string xml, string secureKey)
        {
            var regex = new Regex($@"<(?<{KeyGroupName}>{secureKey})>(?<{ValueGroupName}>[^<]*)(</{secureKey}>)");
            return regex.Replace(xml, EncodeSecureDataForElementValueLocation);
        }

        public string ClearSecureInAttributeLocation(string xml, string secureKey)
        {
            var regex = new Regex($@"(?<{KeyGroupName}>{secureKey})=""(?<{ValueGroupName}>[^""]*)""");
            return regex.Replace(xml, EncodeSecureDataForAttributeLocation);
        }
        
        private string EncodeSecureDataForElementValueLocation(Match match)
        {
            var key = match.Groups[$"{KeyGroupName}"];
            var secure = match.Groups[$"{ValueGroupName}"];
            return $"<{key}>" + new string(SecureSymbol, secure.Length) + $"</{key}>";
        }
        
        private string EncodeSecureDataForAttributeLocation(Match match)
        {
            var key = match.Groups[$"{KeyGroupName}"];
            var secure = match.Groups[$"{ValueGroupName}"];
            return $"{key}=\"" + new string(SecureSymbol, secure.Length) + "\"";
        }
    }
}