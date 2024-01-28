using DataAccess;
using Models;
using System.Threading;
using DataAccess.Local;

namespace Services
{
    public class SettingsService
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
            //try getting the location from file
            
            return DataAccess.DataConnection.GetDefaultDatabaseLocation();
        }
        
        public static string GetDefaultDatabaseSource(ServiceMode mode)
        {
            if (mode == ServiceMode.Local)
                return DataAccess.DataConnection.GetDefaultDatabaseLocation();
            else
                return DataAccess.DataConnection.GetServerConnectionString();
        }

        public IReadOnlyDictionary<(string, string), City> GetCities()
        {
            var _citiesDictionary = Task.Run(async() => await CreateAndFillCitiesDictionary()).Result;

            return _citiesDictionary;
        }
        
        public async Task<IReadOnlyDictionary<(string, string), City>> CreateAndFillCitiesDictionary()
        {
            string FilePath = "Assets/worldcities.csv";
            const int cityNameKey = 0;
            const int idKey = 10;

            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException($"File with path \"{FilePath}\" doesnt exist");
            }

            var dictionary = new Dictionary<(string, string), City>();

            using var sr = new StreamReader(FilePath);

            while (!sr.EndOfStream)
            {
                var line = await sr.ReadLineAsync();

                if (line is null)
                {
                    throw new ArgumentNullException(nameof(line), "Error with reading line from file");
                }

                var lines = line.Split(',');

                dictionary[(lines[cityNameKey], lines[idKey])] = City.Parse(line);
            }
                
            return dictionary;
        }

        public static Dictionary<string, string> GetAllSettings(ServiceMode mode)
        {
            if (mode == ServiceMode.Local)
            {
                return SettingsRepository.GetAll(DataConnection.GetLocalDataSource());
            }
            else
            {
                return DataAccess.Server.SettingsRepository.GetAll(DataConnection.GetServerConnectionString());
            }
        }

        public static string GetSettingsForCode(string code, ServiceMode mode)
        {
            Dictionary<string, string> settings;

            if (mode == ServiceMode.Local)
            {
                settings = SettingsRepository.GetAll(DataConnection.GetLocalDataSource());
            }
            else
            {
                settings = DataAccess.Server.SettingsRepository.GetAll(DataConnection.GetServerConnectionString());
            }

            if (settings.ContainsKey(code))
            {
                return settings[code].ToString();
            }
            else
            {
                return "";
            }
        }

        public static bool UpdateSetting(string code, string description, ServiceMode mode)
        {
            if (mode == ServiceMode.Local)
            {
                return SettingsRepository.Update(code, description);
            }
            else
            {
                return DataAccess.Server.SettingsRepository.Update(code, description);
            }
        }
    }
}