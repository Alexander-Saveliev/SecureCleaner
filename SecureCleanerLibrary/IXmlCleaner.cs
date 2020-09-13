namespace SecureCleanerLibrary
{
    public interface IXmlCleaner
    {
        string ClearSecureInElementValueLocation(string xml, string secureKey);
        string ClearSecureInAttributeLocation(string xml, string secureKey);
    }
}