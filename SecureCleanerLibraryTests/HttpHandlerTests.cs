using System.Collections.Generic;
using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class HttpHandlerTests
    {
        [Fact]
        public void Process_BookingcomHttpResult_ClearSecureData()
        {
            //Arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var requestBody = "http://test.com?user=max&pass=123456";
            var responseBody = "http://test.com?user=max&pass=123456";
            
            var expectedUrl = "http://test.com/users/XXX/info?pass=XXXXXX";
            var expectedRequestBody = "http://test.com?user=XXX&pass=XXXXXX";
            var expectedResponseBody = "http://test.com?user=XXX&pass=XXXXXX";
            
            var secureKey1 = "users";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "pass";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);
            
            var secureKey3 = "user";
            var locations3 = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest, SecureLocationType.UrlQuery};
            var properties3 = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings3 = new SecureSettings(secureKey3, locations3, properties3);
            
            var httpLogHandler = new HttpHandler();

            //Act
            var httpHandlerSettings = new List<SecureSettings>() {secureSettings1, secureSettings2, secureSettings3};
            httpLogHandler.Process(url, requestBody, responseBody, httpHandlerSettings);
            
            //Assert
            Assert.Equal(expectedUrl, httpLogHandler.CurrentLog.Url);
            Assert.Equal(expectedRequestBody, httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal(expectedResponseBody, httpLogHandler.CurrentLog.ResponseBody);
        }
        
        [Fact]
        public void Process_YandexHttpResult_ClearSecureData()
        {
            //Arrange
            var url = "http://test.com?user=max&pass=123456";
            var requestBody = "<auth><user>max</user><pass>123456</pass></auth>";
            var responseBody = "<auth user=\"max\" pass=\"123456\">";
            
            var expectedUrl = "http://test.com?user=XXX&pass=XXXXXX";
            var expectedRequestBody = "<auth><user>XXX</user><pass>XXXXXX</pass></auth>";
            var expectedResponseBody = "<auth user=\"XXX\" pass=\"XXXXXX\">";
            
            var secureKey1 = "user";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery, SecureLocationType.XmlAttribute, SecureLocationType.XmlElementValue};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "pass";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery, SecureLocationType.XmlAttribute, SecureLocationType.XmlElementValue};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);

            var httpLogHandler = new HttpHandler();

            var httpHandlerSettings = new List<SecureSettings>() {secureSettings1, secureSettings2};
            
            //Act
            httpLogHandler.Process(url, requestBody, responseBody, httpHandlerSettings);
 
            //Assert
            Assert.Equal(expectedUrl, httpLogHandler.CurrentLog.Url);
            Assert.Equal(expectedRequestBody, httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal(expectedResponseBody, httpLogHandler.CurrentLog.ResponseBody);
        }
        
        [Fact]
        public void Process_OstrovokHttpResult_ClearSecureData()
        {
            //Arrange
            var url = "http://test.com/users/max/info";
            var requestBody = "http://test.com/users/max/info";
            var responseBody = "{user:{value:\"max\"},pass:{value:\"123456\"}}";
            
            var expectedUrl = "http://test.com/users/XXX/info";
            var expectedRequestBody = "http://test.com/users/XXX/info";
            var expectedResponseBody = "{user:{value:\"XXX\"},pass:{value:\"XXXXXX\"}}";
            
            var secureKey1 = "users";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "user";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.JsonValueElement};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);
            
            var secureKey3 = "pass";
            var locations3 = new HashSet<SecureLocationType>() {SecureLocationType.JsonValueElement};
            var properties3 = new HashSet<SecurePropertyType>() {SecurePropertyType.ResponseBody};
            var secureSettings3 = new SecureSettings(secureKey3, locations3, properties3);

            var httpLogHandler = new HttpHandler();

            var httpHandlerSettings = new List<SecureSettings>() {secureSettings1, secureSettings2, secureSettings3};
            
            //Act
            httpLogHandler.Process(url, requestBody, responseBody, httpHandlerSettings);
 
            //Assert
            Assert.Equal(expectedUrl, httpLogHandler.CurrentLog.Url);
            Assert.Equal(expectedRequestBody, httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal(expectedResponseBody, httpLogHandler.CurrentLog.ResponseBody);
        }
        
        [Fact]
        public void Process_AgodaHttpResult_ClearSecureData()
        {
            //Arrange
            var url = "http://test.com?user=max&pass=123456";
            var requestBody = @"
<auth>
    <user>max</user>
    <pass>123456</pass>
</auth>";
            var responseBody = "<auth user=\"max\" pass=\"123456\">";
            
            var expectedUrl = "http://test.com?user=XXX&pass=XXXXXX";
            var expectedRequestBody = @"
<auth>
    <user>XXX</user>
    <pass>XXXXXX</pass>
</auth>";
            var expectedResponseBody = "<auth user=\"XXX\" pass=\"XXXXXX\">";
            
            var secureKey1 = "user";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery, SecureLocationType.XmlElementValue, SecureLocationType.XmlAttribute};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "pass";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery, SecureLocationType.XmlElementValue, SecureLocationType.XmlAttribute};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);

            var httpLogHandler = new HttpHandler();

            var httpHandlerSettings = new List<SecureSettings>() {secureSettings1, secureSettings2};
            
            //Act
            httpLogHandler.Process(url, requestBody, responseBody, httpHandlerSettings);
 
            //Assert
            Assert.Equal(expectedUrl, httpLogHandler.CurrentLog.Url);
            Assert.Equal(expectedRequestBody, httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal(expectedResponseBody, httpLogHandler.CurrentLog.ResponseBody);
        }
        
        [Fact]
        public void Process_GoogleHttpResult_ClearSecureData()
        {
            //Arrange
            var url =  "http://test.com?user=max&pass=123456";
            var requestBody = @"
<auth user=""max"">
                <pass>123456</pass>
                </auth>";
            var responseBody = @"
<auth pass=""123456"">
                <user>max</user>
                </auth>";
            
            var expectedUrl = "http://test.com?user=XXX&pass=XXXXXX";
            var expectedRequestBody = @"
<auth user=""XXX"">
                <pass>XXXXXX</pass>
                </auth>";
            var expectedResponseBody = @"
<auth pass=""XXXXXX"">
                <user>XXX</user>
                </auth>";
            
            var secureKey1 = "user";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery, SecureLocationType.XmlElementValue, SecureLocationType.XmlAttribute};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "pass";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.UrlQuery, SecureLocationType.XmlElementValue, SecureLocationType.XmlAttribute};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url, SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);

            var httpLogHandler = new HttpHandler();

            var httpHandlerSettings = new List<SecureSettings>() {secureSettings1, secureSettings2};
            
            //Act
            httpLogHandler.Process(url, requestBody, responseBody, httpHandlerSettings);
 
            //Assert
            Assert.Equal(expectedUrl, httpLogHandler.CurrentLog.Url);
            Assert.Equal(expectedRequestBody, httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal(expectedResponseBody, httpLogHandler.CurrentLog.ResponseBody);
        }
        
        [Fact]
        public void Process_ExpediaHttpResult_ClearSecureData()
        {
            //Arrange
            var url = "http://test.com/users/max/info";
            var requestBody = @"
{
       user : ""max"",
       pass : ""123456""
}
";
            var responseBody = @"
{
       user : {
             value : ""max""
       },
       pass : {
             value : ""123456""
       }
}";
            
            var expectedUrl = "http://test.com/users/XXX/info";
            var expectedRequestBody = @"
{
       user : ""XXX"",
       pass : ""XXXXXX""
}
";
            var expectedResponseBody = @"
{
       user : {
             value : ""XXX""
       },
       pass : {
             value : ""XXXXXX""
       }
}";
            
            var secureKey1 = "user";
            var locations1 = new HashSet<SecureLocationType>() {SecureLocationType.JsonValueElement, SecureLocationType.JsonElementAttribute};
            var properties1 = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings1 = new SecureSettings(secureKey1, locations1, properties1);
            
            var secureKey2 = "pass";
            var locations2 = new HashSet<SecureLocationType>() {SecureLocationType.JsonValueElement, SecureLocationType.JsonElementAttribute};
            var properties2 = new HashSet<SecurePropertyType>() {SecurePropertyType.RequestBody, SecurePropertyType.ResponseBody};
            var secureSettings2 = new SecureSettings(secureKey2, locations2, properties2);
            
            var secureKey3 = "users";
            var locations3 = new HashSet<SecureLocationType>() {SecureLocationType.UrlRest};
            var properties3 = new HashSet<SecurePropertyType>() {SecurePropertyType.Url};
            var secureSettings3 = new SecureSettings(secureKey3, locations3, properties3);

            var httpLogHandler = new HttpHandler();

            var httpHandlerSettings = new List<SecureSettings>() {secureSettings1, secureSettings2, secureSettings3};
            
            //Act
            httpLogHandler.Process(url, requestBody, responseBody, httpHandlerSettings);
 
            //Assert
            Assert.Equal(expectedUrl, httpLogHandler.CurrentLog.Url);
            Assert.Equal(expectedRequestBody, httpLogHandler.CurrentLog.RequestBody);
            Assert.Equal(expectedResponseBody, httpLogHandler.CurrentLog.ResponseBody);
        }
    }
}