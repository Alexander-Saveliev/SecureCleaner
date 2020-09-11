using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class UrlCleanerTests
    {
        [Fact]
        public void ClearSecure_ClearOneSecureInRestParameter_OneSecureInRestParameterCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey = "users";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX/info?pass=123456";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey, SecureLocationType.Rest);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }

        [Fact]
        public void ClearSecure_ClearOneSecureInQueryParameter_OneSecureInQueryParameterCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey = "pass";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/max/info?pass=XXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey, SecureLocationType.Query);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecure_ClearSecuresInRestAndQueryParameter_SecureInQueryAndRestParameterCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey1 = "pass";
            var secureKey2 = "users";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey1, SecureLocationType.Query);
            cleanedUrl = urlCleaner.ClearSecure(cleanedUrl, secureKey2, SecureLocationType.Rest);
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecure_ClearSameSecuresInRestAndQueryParameter_SameSecureInQueryAndRestParameterCleared()
        {
            // arrange
            var url = "http://test.com/pass/123/info?pass=123456";
            var secureKey = "pass";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/XXX/info?pass=XXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey, SecureLocationType.Query);
            cleanedUrl = urlCleaner.ClearSecure(cleanedUrl, secureKey, SecureLocationType.Rest);
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecure_ClearSameInRestParameterWithTheSameNameInQuery_SecureInRestClearedNotInQuery()
        {
            // arrange
            var url = "http://test.com/pass/123/info?pass=123456";
            var secureKey = "pass";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/XXX/info?pass=123456";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey, SecureLocationType.Rest);
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecure_ClearSameInQueryParameterWithTheSameNameInRest_SecureInQueryClearedNotInRest()
        {
            // arrange
            var url = "http://test.com/pass/123/info?pass=123456";
            var secureKey = "pass";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/123/info?pass=XXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey, SecureLocationType.Query);
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecure_ClearMultipleSecuresInRestAndQuery_MultipleSecuresInRestAndQueryCleared()
        {
            // arrange
            var url = "http://test.com/pass/123/info/456?pass=123456&email=cheese_invader@icloud.com";
            var secureKey1 = "pass";
            var secureKey2 = "info";
            var secureKey3 = "pass";
            var secureKey4 = "email";
            
            var urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/XXX/info/XXX?pass=XXXXXX&email=XXXXXXXXXXXXXXXXXXXXXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecure(url, secureKey1, SecureLocationType.Rest);
            cleanedUrl = urlCleaner.ClearSecure(cleanedUrl, secureKey2, SecureLocationType.Rest);
            cleanedUrl = urlCleaner.ClearSecure(cleanedUrl, secureKey3, SecureLocationType.Query);
            cleanedUrl = urlCleaner.ClearSecure(cleanedUrl, secureKey4, SecureLocationType.Query);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }

        [Fact]
        public void ClearSecure_ClearSecureWithWrongLocation_SecureNotCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey1 = "users";
            var secureKey2 = "info";
            
            var urlCleaner = new UrlCleaner();
            var expectedResult = "http://test.com/users/max/info?pass=123456";
            
            // act
            var clearedUrl = urlCleaner.ClearSecure(url, secureKey1, SecureLocationType.Query);
            clearedUrl = urlCleaner.ClearSecure(clearedUrl, secureKey2, SecureLocationType.Rest);
            
            // assert
            Assert.Equal(expectedResult, clearedUrl);
            
        }
    }
}