using System.Collections.Generic;

namespace SecureCleanerLibrary
{
    public class SecureSettings
    {
        public SecureSettings(string key, HashSet<SecureLocationType> locations, HashSet<SecurePropertyType> properties)
        {
            Key = key;
            Locations = locations;
            Properties = properties;
        }

        public string Key { get; }
        public HashSet<SecureLocationType> Locations { get; }
        public HashSet<SecurePropertyType> Properties { get; }
    }
}