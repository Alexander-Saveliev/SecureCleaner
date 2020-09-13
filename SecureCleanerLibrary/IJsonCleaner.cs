namespace SecureCleanerLibrary
{
    public interface IJsonCleaner
    {
        string ClearSecureInElementAttributeLocation(string json, string secureKey);
        string ClearSecureInValueElementLocation(string json, string secureKey);
    }
}