using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class JsonCleanerTest
    {
        [Fact]
        public void ClearSecureInElementAttributeLocation_ClearOneSecureInElementAttributeLocation_OneSecureInElementAttributeLocationShouldBeCleared()
        {
            // arrange
            var json = "{ user: \"max\", pass:\"123456\" }";
            var secureKey = "user";
            
            IJsonCleaner jsonCleaner = new JsonCleaner();

            var expectedResult = "{ user: \"XXX\", pass:\"123456\" }";
            
            // act
            var clearedJson = jsonCleaner.ClearSecureInElementAttributeLocation(json, secureKey);

            // assert
            Assert.Equal(expectedResult, clearedJson);
        }

        [Theory]
        [InlineData("{ user: \"max\", pass:\"123456\" }", "{ user: \"XXX\", pass:\"XXXXXX\" }")]
        [InlineData("{user:\"max\", pass:\"123456\"}", "{user:\"XXX\", pass:\"XXXXXX\"}")]
        [InlineData("  {  user:  \"max\"  ,   pass:\"123456\"  }  ", "  {  user:  \"XXX\"  ,   pass:\"XXXXXX\"  }  ")]
        [InlineData("  {  \n\nuser:  \n\n\"max\"  \n\n,   \n\npass:\"123456\"  \n\n}  ", "  {  \n\nuser:  \n\n\"XXX\"  \n\n,   \n\npass:\"XXXXXX\"  \n\n}  ")]
        public void ClearSecureInElementAttributeLocation_ClearOneSecureInJsonWithDifferentFormats_OneSecureInJsonWithDifferentFormatsShouldBeCleared(string json, string result)
        {
            // arrange
            var secureKey1 = "user";
            var secureKey2 = "pass";
            
            IJsonCleaner jsonCleaner = new JsonCleaner();
            
            // act
            var clearedJson = jsonCleaner.ClearSecureInElementAttributeLocation(json, secureKey1);
            clearedJson = jsonCleaner.ClearSecureInElementAttributeLocation(clearedJson, secureKey2);

            // assert
            Assert.Equal(result, clearedJson);
        }
        
        [Fact]
        public void ClearSecureInValueElementLocation_ClearOneSecureInValueElementLocation_OneSecureInValueElementLocationShouldBeCleared()
        {
            // arrange
            var json = "{user: {value:\"max\"}, pass: {value:\"123456\"}}";
            var secureKey = "user";
            
            IJsonCleaner jsonCleaner = new JsonCleaner();

            var expectedResult = "{user: {value:\"XXX\"}, pass: {value:\"123456\"}}";
            
            // act
            var clearedJson = jsonCleaner.ClearSecureInValueElementLocation(json, secureKey);

            // assert
            Assert.Equal(expectedResult, clearedJson);
        }
        
        [Theory]
        [InlineData("{user: {value:\"max\"}, pass: {value: \"123456\"}}", "{user: {value:\"XXX\"}, pass: {value: \"XXXXXX\"}}")]
        [InlineData("{user:{value:\"max\"},pass:{value:\"123456\"}}", "{user:{value:\"XXX\"},pass:{value:\"XXXXXX\"}}")]
        [InlineData("{user \n : \n { \n value \n : \n \"max\" \n } \n , \n pass \n : \n { \n value \n : \n \"123456\" \n } \n } \n ", "{user \n : \n { \n value \n : \n \"XXX\" \n } \n , \n pass \n : \n { \n value \n : \n \"XXXXXX\" \n } \n } \n ")]
        public void ClearSecureInValueElementLocation_ClearOneSecureInJsonWithDifferentFormats_OneSecureInJsonWithDifferentFormatsShouldBeCleared(string json, string result)
        {
            // arrange
            var secureKey1 = "user";
            var secureKey2 = "pass";
            
            IJsonCleaner jsonCleaner = new JsonCleaner();
            
            // act
            var clearedJson = jsonCleaner.ClearSecureInValueElementLocation(json, secureKey1);
            clearedJson = jsonCleaner.ClearSecureInValueElementLocation(clearedJson, secureKey2);

            // assert
            Assert.Equal(result, clearedJson);
        }

        [Fact]
        public void ClearSecureInElementAttributeLocation_ClearSecureAfterClearingInValueElementLocation_ShouldClearSecureInElementAttributeLocationAfterClearingInValueElementLocation()
        {
            // arrange
            var secureKey1 = "user";
            var secureKey2 = "pass";

            var json = "{user: \"max\", pass: {value:\"123456\"}}";
            var expectedResult = "{user: \"XXX\", pass: {value:\"XXXXXX\"}}";
            
            IJsonCleaner jsonCleaner = new JsonCleaner();
            
            var clearedJson = jsonCleaner.ClearSecureInValueElementLocation(json, secureKey2);
            
            // act
            clearedJson = jsonCleaner.ClearSecureInElementAttributeLocation(clearedJson, secureKey1);

            // assert
            Assert.Equal(expectedResult, clearedJson);
        }
        
        [Fact]
        public void ClearSecureInValueElementLocation_ClearSecureAfterClearingInElementAttributeLocation_ShouldClearSecureInValueElementLocationAfterClearingInElementAttributeLocation()
        {
            // arrange
            var secureKey1 = "user";
            var secureKey2 = "pass";

            var json = "{user: \"max\", pass: {value:\"123456\"}}";
            var expectedResult = "{user: \"XXX\", pass: {value:\"XXXXXX\"}}";
            
            IJsonCleaner jsonCleaner = new JsonCleaner();
            
            var clearedJson = jsonCleaner.ClearSecureInElementAttributeLocation(json, secureKey1);
            
            // act
            clearedJson = jsonCleaner.ClearSecureInValueElementLocation(clearedJson, secureKey2);

            // assert
            Assert.Equal(expectedResult, clearedJson);
        }
    }
}