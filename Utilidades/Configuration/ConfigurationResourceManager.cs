using System.Collections.Generic;
using ServiceStack.Text;

namespace Utilidades.Configuration
{
    /// <summary>
    /// Configuration Resource Manager
    /// </summary>
    public class ConfigurationResourceManager : IResourceManager
    {
        /// <summary>
        /// Gets a string by key
        /// </summary>
        public string GetString(string name)
        {
            return ConfigUtils.GetNullableAppSetting(name);
        }

        /// <summary>
        /// Gets a list by key
        /// </summary>
        public IList<string> GetList(string key)
        {
            return ConfigUtils.GetListFromAppSetting(key);
        }

        /// <summary>
        /// Gets a dictionary by key
        /// </summary>
        public IDictionary<string, string> GetDictionary(string key)
        {
            return ConfigUtils.GetDictionaryFromAppSetting(key);
        }

        /// <summary>
        /// Gets an object of T type by name
        /// </summary>
        public T Get<T>(string name, T defaultValue)
        {
            var stringValue = ConfigUtils.GetNullableAppSetting(name);

            return stringValue != null ? TypeSerializer.DeserializeFromString<T>(stringValue) : defaultValue;
        }
    }
}
