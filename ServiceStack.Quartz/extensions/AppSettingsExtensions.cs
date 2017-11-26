namespace ServiceStack.Quartz
{
    using System.Collections.Generic;
    using System.Linq;
    using ServiceStack.Configuration;
    using ServiceStack.Text;

    public static class AppSettingsExtensions
    {
        /// <summary>
        /// Returns all app settings that start with a specific string
        /// </summary>
        /// <param name="settings">the appsettings</param>
        /// <param name="key">the partial key to find</param>
        /// <returns>a dictionary of matching settings</returns>
        public static Dictionary<string, string> GetAllKeysStartingWith(this IAppSettings settings, string key)
        {
            var keyValuePairs = settings.GetAll().Where(x => x.Key.StartsWithIgnoreCase(key)).ToStringDictionary();
            return keyValuePairs;
        }
    }
}