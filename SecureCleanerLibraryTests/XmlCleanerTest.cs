using SecureCleanerLibrary;
using Xunit;

namespace SecureCleanerLibraryTests
{
    public class XmlCleanerTest
    {
        [Fact]
        public void ClearSecureInElementValueLocation_ClearOneSecureInXmlElementValue_OneSecureInXmlElementValueShouldBeCleared()
        {
            // arrange
            var xml = "<auth><user>max</user><pass>123456</pass></auth>";
            var secureKey = "user";
            
            IXmlCleaner xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth><user>XXX</user><pass>123456</pass></auth>";
            
            // act
            var clearedXml = xmlCleaner.ClearSecureInElementValueLocation(xml, secureKey);

            // assert
            Assert.Equal(expectedResult, clearedXml);
        }
        
        [Fact]
        public void ClearSecureInElementValueLocation_ClearTwoSecureInXmlElementValue_TwoSecureInXmlElementValueShouldBeCleared()
        {
            // arrange
            var xml = "<auth><user>max</user><pass>123456</pass></auth>";
            var secureKey1 = "user";
            var secureKey2 = "pass";
            
            IXmlCleaner xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth><user>XXX</user><pass>XXXXXX</pass></auth>";
            
            var clearedXml = xmlCleaner.ClearSecureInElementValueLocation(xml, secureKey1);
            
            // act
            clearedXml = xmlCleaner.ClearSecureInElementValueLocation(clearedXml, secureKey2);

            // assert
            Assert.Equal(expectedResult, clearedXml);
        }
        
        [Fact]
        public void ClearSecureInAttributeLocation_ClearOneSecureInXmlAttribute_OneSecureInXmlAttributeShouldBeCleared()
        {
            // arrange
            var xml = "<auth user=\"max\" pass=\"123456\">";
            var secureKey = "user";
            
            IXmlCleaner xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth user=\"XXX\" pass=\"123456\">";
            
            // act
            var clearedXml = xmlCleaner.ClearSecureInAttributeLocation(xml, secureKey);

            // assert
            Assert.Equal(expectedResult, clearedXml);
        }
        
        [Fact]
        public void ClearSecureInAttributeLocation_ClearTwoSecureInXmlAttribute_TwoSecureInXmlAttributeShouldBeCleared()
        {
            // arrange
            var xml = "<auth user=\"max\" pass=\"123456\">";
            var secureKey1 = "user";
            var secureKey2 = "pass";
            
            IXmlCleaner xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth user=\"XXX\" pass=\"XXXXXX\">";
            
            var clearedXml = xmlCleaner.ClearSecureInAttributeLocation(xml, secureKey1);
            
            // act
            clearedXml = xmlCleaner.ClearSecureInAttributeLocation(clearedXml, secureKey2);

            // assert
            Assert.Equal(expectedResult, clearedXml);
        }

        [Fact]
        public void
            ClearSecureInAttributeLocation_ClearAfterClearingInElementValueLocation_SecureInAttributeLocationShouldBeCleared()
        {
            // arrange
            var xml = "<auth><user name=\"max\"><pass>123456</pass></auth>";
            var secureKeyForAttributeLocation = "name";
            var secureKeyForElementValueLocation = "pass";
            
            IXmlCleaner xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth><user name=\"XXX\"><pass>XXXXXX</pass></auth>";
            
            var clearedXml = xmlCleaner.ClearSecureInElementValueLocation(xml, secureKeyForElementValueLocation);
            
            // act
            clearedXml = xmlCleaner.ClearSecureInAttributeLocation(clearedXml, secureKeyForAttributeLocation);

            // assert
            Assert.Equal(expectedResult, clearedXml);
        }
        
        [Fact]
        public void
            ClearSecureInAttributeLocation_ClearAfterClearingInAttributeLocation_SecureInElementValueLocationShouldBeCleared()
        {
            // arrange
            var xml = "<auth><user name=\"max\"><pass>123456</pass></auth>";
            var secureKeyForAttributeLocation = "name";
            var secureKeyForElementValueLocation = "pass";
            
            IXmlCleaner xmlCleaner = new XmlCleaner();

            var expectedResult = "<auth><user name=\"XXX\"><pass>XXXXXX</pass></auth>";
            
            var clearedXml = xmlCleaner.ClearSecureInAttributeLocation(xml, secureKeyForAttributeLocation);
            
            // act
            clearedXml = xmlCleaner.ClearSecureInElementValueLocation(clearedXml, secureKeyForElementValueLocation);

            // assert
            Assert.Equal(expectedResult, clearedXml);
        }
    }
}