using DataAccess;

namespace Services
{
    public static class SettingsService
    {
        /// <summary>
        /// Given a settings key, return the value for said key.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKeyValue(ServiceMode mode, string key)
        {
            return "";
        }

        public static string GetLocalDatabaseSource()
        {
            return DataAccess.DataConnection.GetDefaultDatabaseLocation();
        }

    }
}