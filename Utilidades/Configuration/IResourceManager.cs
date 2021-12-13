using System.Collections.Generic;

namespace Utilidades.Configuration
{
    /// <summary>
    /// Resource manager interface
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// Gets a string by key
        /// </summary>
        string GetString(string name);

        /// <summary>
        /// Gets a list by key
        /// </summary>
        IList<string> GetList(string key);

        /// <summary>
        /// Gets a dictionary by key
        /// </summary>
        IDictionary<string, string> GetDictionary(string key);

        /// <summary>
        /// Gets an object of T type by name
        /// </summary>
        T Get<T>(string name, T defaultValue);
    }
}
