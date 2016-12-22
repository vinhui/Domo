using Domo.Debug;
using Domo.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Domo.Main
{
    public static class Config
    {
        /// <summary>
        /// Name of the config file
        /// </summary>
        internal const string fileName = "config.json";

        /// <summary>
        /// Path to the config file
        /// </summary>
        internal static string path { get; private set; }

        /// <summary>
        /// The default settings
        /// </summary>
        private static Dictionary<string, object> defaultData = new Dictionary<string, object>()
        { };

        /// <summary>
        /// All the settings that are actually used are stored here
        /// </summary>
        private static Dictionary<string, object> data = new Dictionary<string, object>();

        /// <summary>
        /// Initialize the config when some script wants something from it
        /// </summary>
        static Config()
        {
            // Set the path property to the full path to the config file
            path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), fileName);

            // Check if the config file exist, if not, write the defaults to the file, otherwise load it from the file
            if (!File.Exists(path))
            {
                Log.Info("Config file does not exist yet, writing default config settings");
                data = defaultData;
                WriteToFile();
            }
            else
            {
                Log.Info("Loading config settings from file (" + path + ")");
                LoadFromFile();
            }

            // Just a simple check if the config really had all data and isnt missing something
            if (data.Count < defaultData.Count)
            {
                Log.Info("There are more default settings than there are loaded settings, this probably means there are some keys missing. Fixing it!");
                data = data.Concat(defaultData).GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First().Value);
                WriteToFile();
            }
        }

        /// <summary>
        /// Load the config from file
        /// </summary>
        public static void LoadFromFile()
        {
            if (File.Exists(path))
            {
                data = Serializer.instance.Deserialize<Dictionary<string, object>>(File.ReadAllText(path));
            }
            else
            {
                Log.Warning("Config file does not exist!");
            }
        }

        /// <summary>
        /// Write all the settings to the config file
        /// </summary>
        public static void WriteToFile()
        {
            string json = Serializer.instance.Serialize(data);

            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Get a value from the config
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="key">The keys to get the value from</param>
        /// <returns>Returns the object cast to <paramref name="T"/></returns>
        public static T GetValue<T>(params string[] keys)
        {
            return GetValue<T>(data, 0, keys);
        }

        private static T GetValue<T>(Dictionary<string, object> dict, int currIndex, params string[] keys)
        {
            if (currIndex == keys.Length - 1)
                return GetValue<T>(dict, keys[currIndex]);
            else
            {
                if (dict[keys[currIndex]].GetType() == typeof(Dictionary<string, object>))
                {
                    return GetValue<T>(
                        (Dictionary<string, object>)dict[keys[currIndex]],
                        ++currIndex,
                        keys);
                }
                else
                {
                    Log.Error("The value of '" + keys[currIndex] + "' is not a dictionary");
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Get a value from a given dictionary
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dict">The dictionary to search in</param>
        /// <param name="key">The key to get the value from</param>
        /// <returns>Returns the object cast to <paramref name="T"/></returns>
        private static T GetValue<T>(Dictionary<string, object> dict, string key)
        {
            // Check if the key is in the dictionary
            if (dict.ContainsKey(key))
            {
                // If the object is of type T, we can just cast it
                if (dict[key] is T)
                    return (T)dict[key];
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(dict[key], typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        Log.Error("Failed to cast the value of '" + key + "' to " + typeof(T));
                    }
                }
            }
            else
            {
                Log.Error("There is no key '" + key + "' in the dictionary");
            }
            return default(T);
        }

        /// <summary>
        /// Set a value
        /// </summary>
        /// <param name="key">The key for which the value should be set</param>
        /// <param name="value">The thing you want to set it to</param>
        /// <param name="write">Do you want to write the changes to the config file?</param>
        public static void SetValue(string key, object value, bool write = true)
        {
            data[key] = value;

            if (write)
                WriteToFile();
        }

        /// <summary>
        /// Check if the config has a certain key
        /// </summary>
        /// <param name="key">Name to look for</param>
        public static bool ContainsKey(string key)
        {
            return data.ContainsKey(key);
        }
    }
}