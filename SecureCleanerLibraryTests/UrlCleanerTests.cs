using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class UrlCleanerTests
    {
        [Fact]
        public void ClearSecureInRestLocation_ClearOneSecureInRestParameter_OneSecureInRestParameterCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey = "users";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX/info?pass=123456";

            // act
            var cleanedUrl = urlCleaner.ClearSecureInRestLocation(url, secureKey);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }

        [Fact]
        public void ClearSecureInQueryLocation_ClearOneSecureInQueryParameter_OneSecureInQueryParameterCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey = "pass";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/max/info?pass=XXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecureInQueryLocation(url, secureKey);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecureInRestLocation_ClearSecuresInRestAndQueryParameter_SecureInQueryAndRestParameterCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey1 = "pass";
            var secureKey2 = "users";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/users/XXX/info?pass=XXXXXX";

            var cleanedUrl = urlCleaner.ClearSecureInQueryLocation(url, secureKey1);
            
            // act
            cleanedUrl = urlCleaner.ClearSecureInRestLocation(cleanedUrl, secureKey2);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecureInRestLocation_ClearSameSecuresInRestAndQueryParameter_SameSecureInQueryAndRestParameterCleared()
        {
            // arrange
            var url = "http://test.com/pass/123/info?pass=123456";
            var secureKey = "pass";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/XXX/info?pass=XXXXXX";
            var cleanedUrl = urlCleaner.ClearSecureInQueryLocation(url, secureKey);
            
            // act
            cleanedUrl = urlCleaner.ClearSecureInRestLocation(cleanedUrl, secureKey);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecureInRestLocation_ClearSameInRestParameterWithTheSameNameInQuery_SecureInRestClearedNotInQuery()
        {
            // arrange
            var url = "http://test.com/pass/123/info?pass=123456";
            var secureKey = "pass";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/XXX/info?pass=123456";

            // act
            var cleanedUrl = urlCleaner.ClearSecureInRestLocation(url, secureKey);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecureInQueryLocation_ClearSameInQueryParameterWithTheSameNameInRest_SecureInQueryClearedNotInRest()
        {
            // arrange
            var url = "http://test.com/pass/123/info?pass=123456";
            var secureKey = "pass";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/123/info?pass=XXXXXX";

            // act
            var cleanedUrl = urlCleaner.ClearSecureInQueryLocation(url, secureKey);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }
        
        [Fact]
        public void ClearSecureInQueryLocation_ClearMultipleSecuresInRestAndQuery_MultipleSecuresInRestAndQueryCleared()
        {
            // arrange
            var url = "http://test.com/pass/123/info/456?pass=123456&email=cheese_invader@icloud.com";
            var secureKey1 = "pass";
            var secureKey2 = "info";
            var secureKey3 = "pass";
            var secureKey4 = "email";
            
            IUrlCleaner urlCleaner = new UrlCleaner();

            var expectedResult = "http://test.com/pass/XXX/info/XXX?pass=XXXXXX&email=XXXXXXXXXXXXXXXXXXXXXXXXX";

            var cleanedUrl = urlCleaner.ClearSecureInRestLocation(url, secureKey1);
            cleanedUrl = urlCleaner.ClearSecureInRestLocation(cleanedUrl, secureKey2);
            cleanedUrl = urlCleaner.ClearSecureInQueryLocation(cleanedUrl, secureKey3);
            
            // act
            cleanedUrl = urlCleaner.ClearSecureInQueryLocation(cleanedUrl, secureKey4);
            
            // assert
            Assert.Equal(expectedResult, cleanedUrl);
        }

        [Fact]
        public void ClearSecureInRestLocation_ClearSecureWithWrongLocation_SecureNotCleared()
        {
            // arrange
            var url = "http://test.com/users/max/info?pass=123456";
            var secureKey1 = "users";
            var secureKey2 = "info";
            
            IUrlCleaner urlCleaner = new UrlCleaner();
            var expectedResult = "http://test.com/users/max/info?pass=123456";
            
            var clearedUrl = urlCleaner.ClearSecureInQueryLocation(url, secureKey1);
            
            // act
            clearedUrl = urlCleaner.ClearSecureInRestLocation(clearedUrl, secureKey2);
            
            // assert
            Assert.Equal(expectedResult, clearedUrl);
        }
    }
}