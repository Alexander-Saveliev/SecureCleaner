using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SecureCleanerLibrary
{
    public class SecureCleaner: ISecureCleaner
    {

        private List<SecureSettings> _secures;
        private IUrlCleaner _urlCleaner = new UrlCleaner();
        private IXmlCleaner _xmlCleaner = new XmlCleaner();
        private IJsonCleaner _jsonCleaner = new JsonCleaner();

        public SecureCleaner(List<SecureSettings> secures)
        {
            _secures = secures;
        }
        
        public HttpResult CleanHttpResult(HttpResult httpResult)
        {
            const string httpPattern = "http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?";
            const string xmlPattern = "<[^>]*>(<[^>]*/>)*";
            const string jsonPattern = "{[^}]*}";
            
            Regex httpRegex = new Regex(httpPattern, RegexOptions.IgnoreCase);
            Regex xmlRegex = new Regex(xmlPattern, RegexOptions.IgnoreCase);
            Regex jsonRegex = new Regex(jsonPattern, RegexOptions.IgnoreCase);
            
            var url = httpRegex.Replace(httpResult.Url, ReplaceUrlInUrl);
            var requestBody = httpRegex.Replace(httpResult.RequestBody, ReplaceUrlInRequestBody);
            var responseBody = httpRegex.Replace(httpResult.ResponseBody, ReplaceUrlInResponseBody);
            
            url = xmlRegex.Replace(url, ReplaceXmlInUrl);
            requestBody = xmlRegex.Replace(requestBody, ReplaceXmlInRequestBody);
            responseBody = xmlRegex.Replace(responseBody, ReplaceXmlInResponseBody);
            
            url = jsonRegex.Replace(url, ReplaceJsonInUrl);
            requestBody = jsonRegex.Replace(requestBody, ReplaceJsonInRequestBody);
            responseBody = jsonRegex.Replace(responseBody, ReplaceJsonInResponseBody);
            
            return new HttpResult(url, requestBody, responseBody);
        }

        private string ReplaceUrlInUrl(Match match)
        {
            return ReplaceSecureInUrl(match, SecurePropertyType.Url);
        }

        private string ReplaceUrlInRequestBody(Match match)
        {
            return ReplaceSecureInUrl(match, SecurePropertyType.RequestBody);
        }

        private string ReplaceUrlInResponseBody(Match match)
        {
            return ReplaceSecureInUrl(match, SecurePropertyType.ResponseBody); 
        }
        
        private string ReplaceXmlInUrl(Match match)
        {
            return ReplaceSecureInXml(match, SecurePropertyType.Url);
        }

        private string ReplaceXmlInRequestBody(Match match)
        {
            return ReplaceSecureInXml(match, SecurePropertyType.RequestBody);
        }

        private string ReplaceXmlInResponseBody(Match match)
        {
            return ReplaceSecureInXml(match, SecurePropertyType.ResponseBody); 
        }
        
        private string ReplaceJsonInUrl(Match match)
        {
            return ReplaceSecureInJson(match, SecurePropertyType.Url);
        }

        private string ReplaceJsonInRequestBody(Match match)
        {
            return ReplaceSecureInJson(match, SecurePropertyType.RequestBody);
        }

        private string ReplaceJsonInResponseBody(Match match)
        {
            return ReplaceSecureInJson(match, SecurePropertyType.ResponseBody); 
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
                    switch (location)
                    {
                        case SecureLocationType.UrlRest:
                            cleanedUrl = _urlCleaner.ClearSecureInRestLocation(cleanedUrl, secure.Key);
                            break;
                        case SecureLocationType.UrlQuery:
                            cleanedUrl = _urlCleaner.ClearSecureInQueryLocation(cleanedUrl, secure.Key);
                            break;
                    }
                }
            }
            
            return cleanedUrl;
        }

        private string ReplaceSecureInXml(Match match, SecurePropertyType propertyType)
        {
            var cleanedXml = match.Value;
            foreach (var secure in _secures)
            {
                if (!secure.Properties.Contains(propertyType))
                {
                    continue;
                }

                foreach (var location in secure.Locations)
                {
                    switch (location)
                    {
                        case SecureLocationType.XmlElementValue:
                            cleanedXml = _xmlCleaner.ClearSecureInElementValueLocation(cleanedXml, secure.Key);
                            break;   
                        case SecureLocationType.XmlAttribute:
                            cleanedXml = _xmlCleaner.ClearSecureInAttributeLocation(cleanedXml, secure.Key);
                            break;
                    }
                }
            }

            return cleanedXml;
        }
        
        private string ReplaceSecureInJson(Match match, SecurePropertyType propertyType)
        {
            var cleanedJson = match.Value;
            foreach (var secure in _secures)
            {
                if (!secure.Properties.Contains(propertyType))
                {
                    continue;
                }

                foreach (var location in secure.Locations)
                {
                    switch (location)
                    {
                        case SecureLocationType.JsonElementAttribute:
                            cleanedJson = _jsonCleaner.ClearSecureInElementAttributeLocation(cleanedJson, secure.Key);
                            break;   
                        case SecureLocationType.XmlAttribute:
                            cleanedJson = _jsonCleaner.ClearSecureInElementAttributeLocation(cleanedJson, secure.Key);
                            break;
                    }
                }
            }

            return cleanedJson;
        }
    }
}