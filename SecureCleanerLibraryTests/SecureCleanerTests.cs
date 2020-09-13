using System.Collections.Generic;
using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class SecureCleanerTests
    {
        [Fact]
        public void CleanHttpResult_ClearOneSecureInUrl_OneSecureInUrlStringParameterCleared()
        {
            // arrange
            var secureKey = "users";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings = new SecureSettings(secureKey, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings});
            var httpResult = new HttpResult("http://test.com/users/max/info?pass=123456", "", "");
            var expectedResult = "http://test.com/users/XXX/info?pass=123456";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }
        
        [Fact]
        public void CleanHttpResult_ClearSecureInHtml_SecureInHtmlCleared()
        {
            // arrange
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var requestBody = 
                "<!DOCTYPE HTML>" + 
                "<html>" +
                "<head>" + 
                "<meta charset=\"utf-8\">" +
                "<title>Тег LINK</title>" +
                "<link rel=\"stylesheet\" href=\"ie.css\">" +
                "<link rel=\"alternate\" type=\"application/rss+xml\"" +
                "title=\"Статьи с сайта htmlbook.ru\" href=\"http://htmlbook.ru/rss.xml\">" + 
                "<link rel=\"shortcut icon\" href=\"http://test.com/users/max/info?pass=123456\">" +
                "</head>" +
                "<body>" +
                "<p>...</p>" +
                "</body>" +
                "</html>";
            
            var expectedResult = 
                "<!DOCTYPE HTML>" + 
                "<html>" +
                "<head>" + 
                "<meta charset=\"utf-8\">" +
                "<title>Тег LINK</title>" +
                "<link rel=\"stylesheet\" href=\"ie.css\">" +
                "<link rel=\"alternate\" type=\"application/rss+xml\"" +
                "title=\"Статьи с сайта htmlbook.ru\" href=\"http://htmlbook.ru/rss.xml\">" + 
                "<link rel=\"shortcut icon\" href=\"http://test.com/users/XXX/info?pass=XXXXXX\">" +
                "</head>" +
                "<body>" +
                "<p>...</p>" +
                "</body>" +
                "</html>";
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("", requestBody, "");
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }
        
        [Fact]
        public void CleanHttpResult_ClearSecureInJson_SecureInJsonCleared()
        {
            // arrange
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var requestBody = 
            "{" +
                "\"glossary\": {" +
                "\"title\": \"example glossary\"," +
                "\"GlossDiv\": {" +
                "\"title\": \"S\"," +
                "\"GlossList\": {" +
                "\"GlossEntry\": {" +
                "\"ID\": \"SGML\"," +
                "\"SortAs\": \"SGML\"," +
                "\"GlossTerm\": \"Standard Generalized Markup Language\"," +
                "\"Acronym\": \"SGML\"," +
                "\"Abbrev\": \"ISO 8879:1986\"," +
                "\"GlossDef\": {" +
                "\"para\": \"A meta-markup language, used to create markup languages such as DocBook.\"," +
                "\"Requests\": [\"http://test.com/users/max/info?pass=123456\", \"http://test.com/users/max/info?pass=123456\"]" +
                "}," +
                "\"GlossSee\": \"markup\"" +
                "}" +
                "}" +
                "}" +
                "}" +
            "}";
            
            var expectedResult = 
                "{" +
                "\"glossary\": {" +
                "\"title\": \"example glossary\"," +
                "\"GlossDiv\": {" +
                "\"title\": \"S\"," +
                "\"GlossList\": {" +
                "\"GlossEntry\": {" +
                "\"ID\": \"SGML\"," +
                "\"SortAs\": \"SGML\"," +
                "\"GlossTerm\": \"Standard Generalized Markup Language\"," +
                "\"Acronym\": \"SGML\"," +
                "\"Abbrev\": \"ISO 8879:1986\"," +
                "\"GlossDef\": {" +
                "\"para\": \"A meta-markup language, used to create markup languages such as DocBook.\"," +
                "\"Requests\": [\"http://test.com/users/XXX/info?pass=XXXXXX\", \"http://test.com/users/XXX/info?pass=XXXXXX\"]" +
                "}," +
                "\"GlossSee\": \"markup\"" +
                "}" +
                "}" +
                "}" +
                "}" +
                "}";
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("", requestBody, "");
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }
        
        [Fact]
        public void CleanHttpResult_ClearSecureInXml_SecureInXmlCleared()
        {
            // arrange
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);

            var requestBody =
                "<?xml version=\"1.0\"?>" +
                "<PurchaseOrder PurchaseOrderNumber=\"99503\" OrderDate=\"1999-10-20\">" +
                "<Address Type=\"Shipping\">" +
                "<Name>Ellen Adams</Name>" +
                "<Street>123 Maple Street</Street>" +
                "<City>Mill Valley</City>" +
                "<State>CA</State>" +
                "<Zip>10999</Zip>" +
                "<Country>USA</Country>" +
                "</Address>" +
                "<Address Type=\"Billing\">" +
                "<Name>Tai Yee</Name>" +
                "<Street>8 Oak Avenue</Street>" +
                "<City>Old Town</City>" +
                "<State>PA</State>" +
                "<Zip>95819</Zip>" +
                "<Country>USA</Country>" +
                "</Address>" +
                "<DeliveryNotes>Please leave packages in shed by driveway.</DeliveryNotes>" +
                "<Items>" +
                "<Item PartNumber=\"872-AA\">" +
                "<ProductName>Lawnmower</ProductName>" +
                "<Quantity>1</Quantity>" +
                "<USPrice>148.95</USPrice>" +
                "<Comment>Confirm this is electric</Comment>" +
                "</Item>" +
                "<Item PartNumber=\"926-AA\">" +
                "<ProductName>http://test.com/users/max/info?pass=123456</ProductName>" +
                "<Quantity>2</Quantity>" +
                "<USPrice>39.98</USPrice>" +
                "<ShipDate>1999-05-21</ShipDate>" +
                "</Item>" +
                "</Items>" +
                "</PurchaseOrder>";
            
            var expectedResult = 
                "<?xml version=\"1.0\"?>" +
                "<PurchaseOrder PurchaseOrderNumber=\"99503\" OrderDate=\"1999-10-20\">" +
                "<Address Type=\"Shipping\">" +
                "<Name>Ellen Adams</Name>" +
                "<Street>123 Maple Street</Street>" +
                "<City>Mill Valley</City>" +
                "<State>CA</State>" +
                "<Zip>10999</Zip>" +
                "<Country>USA</Country>" +
                "</Address>" +
                "<Address Type=\"Billing\">" +
                "<Name>Tai Yee</Name>" +
                "<Street>8 Oak Avenue</Street>" +
                "<City>Old Town</City>" +
                "<State>PA</State>" +
                "<Zip>95819</Zip>" +
                "<Country>USA</Country>" +
                "</Address>" +
                "<DeliveryNotes>Please leave packages in shed by driveway.</DeliveryNotes>" +
                "<Items>" +
                "<Item PartNumber=\"872-AA\">" +
                "<ProductName>Lawnmower</ProductName>" +
                "<Quantity>1</Quantity>" +
                "<USPrice>148.95</USPrice>" +
                "<Comment>Confirm this is electric</Comment>" +
                "</Item>" +
                "<Item PartNumber=\"926-AA\">" +
                "<ProductName>http://test.com/users/XXX/info?pass=XXXXXX</ProductName>" +
                "<Quantity>2</Quantity>" +
                "<USPrice>39.98</USPrice>" +
                "<ShipDate>1999-05-21</ShipDate>" +
                "</Item>" +
                "</Items>" +
                "</PurchaseOrder>";
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("", requestBody, "");
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }

        [Fact]
        public void CleanHttpResult_ClearSecureFromAllProperties_SecureFromAllPropertiesCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult(url, url, url);
            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResult, cleanedHttpResult.ResponseBody);
        }
        
        [Fact]
        public void CleanHttpResult_SpecifyPropertyType_SecureFromNotSpecifieyPropertiesNotCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult(url, url, url);
            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
            Assert.Equal(url, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResult, cleanedHttpResult.ResponseBody);
        }
        
        [Fact]
        public void CleanHttpResult_ClearHttpResultWithDifferentProperties_HttpResultCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var requestBody = "http://test.com?user=max&pass=123456";
            var responseBody = "http://test.com?user=max&pass=123456";

            var expectedUrlResult = "http://test.com/users/XXX/info?pass=XXXXXX";
            var expectedRequestBody = "http://test.com?user=XXX&pass=XXXXXX";
            var expectedResponseBody = "http://test.com?user=XXX&pass=XXXXXX";
            
            var secureKey1 = "user";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "pass";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);
            
            var secureKey3 = "users";
            var locations3 = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest};
            var properties3 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings3 = new SecureSettings(secureKey3, locations3, properties3);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2, secureSettings3});
            var httpResult = new HttpResult(url, requestBody, responseBody);
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedUrlResult, cleanedHttpResult.Url);
            Assert.Equal(expectedRequestBody, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResponseBody, cleanedHttpResult.ResponseBody);
        }

        [Fact]
        public void CleanHttpResult_ClearHttpResultWithXml_HttpResultWithXmlShouldBeCleared()
        {
            // arrange
            var secureKey = "users";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.XmlAttribute};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings = new SecureSettings(secureKey, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings});
            var httpResult = new HttpResult("Hello <auth user=\"max\" pass=\"123456\"> World", "", "");
            var expectedResult = "Hello <auth user=\"max\" pass=\"123456\"> World";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }
        
        [Fact]
        public void CleanHttpResult_ClearHttpResultInXmlAndUrl_HttpResultWithXmlAndUrlShouldBeCleared()
        {
            // arrange
            var secureKey1 = "user";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.XmlAttribute, SecureLocationType.UrlQuery};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("Hello <auth user=\"max\" pass=\"123456\" link=\"http://test.com?user=max&pass=123456\"> World", "", "");
            var expectedResult = "Hello <auth user=\"XXX\" pass=\"XXXXXX\" link=\"http://test.com?user=XXX&pass=XXXXXX\"> World";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }

        [Fact]
        public void CleanHttpResult_ClearHttpResultInJson_HttpResultInJsonShoudBeCleared()
        {
            var secureKey1 = "user";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.JsonElementAttribute};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("Hello {user: \"max\", pass: \"123456\"} World", "", "");
            var expectedResult = "Hello {user: \"XXX\", pass: \"XXXXXX\"} World";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }
        
        [Fact]
        public void CleanHttpResult_ClearHttpResultInXmlAndJsonAndUrl_HttpResultWithXmlAndJsonAndUrlShouldBeCleared()
        {
            // arrange
            var secureKey1 = "user";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.XmlAttribute, SecureLocationType.UrlQuery, SecureLocationType.JsonElementAttribute};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var secureCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("Hello <auth user=\"max\" pass=\"123456\" link=\"http://test.com?user=max&pass=123456\" info=\"{user: \"max\", pass: \"123456\"}\"> World", "", "");
            var expectedResult = "Hello <auth user=\"XXX\" pass=\"XXXXXX\" link=\"http://test.com?user=XXX&pass=XXXXXX\" info=\"{user: \"XXX\", pass: \"XXXXXX\"}\"> World";
            
            // act
            var cleanedHttpResult = secureCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }
    }
}