using System.Collections.Generic;
using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class SecureCleanerTests
    {
        [Fact]
        public void ClearSecure_ClearOneSecureInUrl_OneSecureInUrlStringParameterCleared()
        {
            // arrange
            var secureKey = "users";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.Rest};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings = new SecureSettings(secureKey, locations, properties);
            
            var urlCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings});
            var httpResult = new HttpResult("http://test.com/users/max/info?pass=123456", "", "");
            var expectedResult = "http://test.com/users/XXX/info?pass=123456";
            
            // act
            var cleanedHttpResult = urlCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
        }
        
        [Fact]
        public void ClearSecure_ClearSecureInHtml_SecureInHtmlCleared()
        {
            // arrange
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.Rest, SecureLocationType.Query};
            
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
            
            var urlCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("", requestBody, "");
            
            // act
            var cleanedHttpResult = urlCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }
        
        [Fact]
        public void ClearSecure_ClearSecureInJson_SecureInJsonCleared()
        {
            // arrange
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.Rest, SecureLocationType.Query};
            
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
            
            var urlCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("", requestBody, "");
            
            // act
            var cleanedHttpResult = urlCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }
        
        [Fact]
        public void ClearSecure_ClearSecureInXml_SecureInXmlCleared()
        {
            // arrange
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.Rest, SecureLocationType.Query};
            
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
            
            var urlCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult("", requestBody, "");
            
            // act
            var cleanedHttpResult = urlCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
        }

        [Fact]
        public void ClearSecure_ClearSecureFromAllProperties_SecureFromAllPropertiesCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.Rest, SecureLocationType.Query};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var urlCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult(url, url, url);
            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";
            
            // act
            var cleanedHttpResult = urlCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
            Assert.Equal(expectedResult, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResult, cleanedHttpResult.ResponseBody);
        }
        
        [Fact]
        public void ClearSecure_SpecifyPropertyType_SecureFromNotSpecifieyPropertiesNotCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            
            var secureKey1 = "users";
            var secureKey2 = "pass";
            var locations = new HashSet<SecureLocationType>() {SecureLocationType.Rest, SecureLocationType.Query};
            
            var properties = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations, properties);
            var secureSettings2 = new SecureSettings(secureKey2, locations, properties);
            
            var urlCleaner = new SecureCleaner(new List<SecureSettings> {secureSettings1, secureSettings2});
            var httpResult = new HttpResult(url, url, url);
            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";
            
            // act
            var cleanedHttpResult = urlCleaner.CleanHttpResult(httpResult);
            
            // assert
            Assert.Equal(expectedResult, cleanedHttpResult.Url);
            Assert.Equal(url, cleanedHttpResult.RequestBody);
            Assert.Equal(expectedResult, cleanedHttpResult.ResponseBody);
        }
    }
}