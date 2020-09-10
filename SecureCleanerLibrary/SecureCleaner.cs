using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SecureCleanerLibrary
{
    public class SecureCleaner: ISecureCleaner
    {

        private List<SecureSettings> _secures;
        private IUrlCleaner _urlCleaner = new UrlCleaner();

        public SecureCleaner(List<SecureSettings> secures)
        {
            _secures = secures;
        }
        
        public HttpResult CleanHttpResult(HttpResult httpResult)
        {
            const string httpPattern = "http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?";
            
            Regex regex = new Regex(httpPattern, RegexOptions.IgnoreCase);
            var url = regex.Replace(httpResult.Url, ReplaceUrl);
            var requestBody = regex.Replace(httpResult.RequestBody, ReplaceRequestBody);
            var responseBody = regex.Replace(httpResult.ResponseBody, ReplaceResponseBody);
            
            return new HttpResult(url, requestBody, responseBody);
        }

        private string ReplaceUrl(Match match)
        {
            return ReplaceSecureInUrl(match, SecurePropertyType.Url);
        }

        private string ReplaceRequestBody(Match match)
        {
            return ReplaceSecureInUrl(match, SecurePropertyType.RequestBody);
        }

        private string ReplaceResponseBody(Match match)
        {
            return ReplaceSecureInUrl(match, SecurePropertyType.ResponseBody); 
        }

        private string ReplaceSecureInUrl(Match match, SecurePropertyType propertyType)
        {
            var cleanedUrl = match.Value;
            
            foreach (var secure in _secures)
            {
                if (!secure.Properties.Contains(propertyType))
                {
                    continue;
                }
                
                foreach (var location in secure.Locations)
                {
                    cleanedUrl = _urlCleaner.ClearSecure(cleanedUrl, secure.Key, location);
                }
            }
            
            return cleanedUrl;
        }
    }
}