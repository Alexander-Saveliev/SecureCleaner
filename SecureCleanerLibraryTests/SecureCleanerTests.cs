using System.Collections.Generic;
using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class SecureCleanerTests
    {
        [Fact]
        public void ClearSecure_ClearOneSecureInRestParameterOfUrl_OneSecureInRestParameterCleared()
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
    }
}