using Domo.Misc.Debug;
using Domo.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Domo.Misc
{
    public static class Config
    {
        /// <summary>
        /// Name of the config file
        /// </summary>
        public const string fileName = "config.json";

        /// <summary>
        /// Path to the config file, including the filename
        /// </summary>
        public static string path { get; private set; }

        /// <summary>
        /// The default settings
        /// </summary>
        private static readonly Dictionary<string, object> defaultData = new Dictionary<string, object>()
        {
            { "minLogLevel", "-1" },
            {
                "API",
                new Dictionary<string, object>()
                {
                    {
                        "web",
                        new Dictionary<string, object>()
                        {
                            {
                                "bindurls",
                                new ArrayList {
                                    "http://localhost:80/",
                                }
                            },
                        }
                    }
                }
            },
            {
                "packaging",
                new Dictionary<string, object>()
                {
                    {
                        "path",
                        "Packages/"
                    },
                    {
                        "manifestName",
                        "Manifest.json"
                    },
                    {
                        "scriptExtension",
                        "py"
                    }
                }
            },
        };

        /// <summary>
        /// All the settings that are actually used are stored here
        /// </summary>
        private static Dictionary<string, object> data = new Dictionary<string, object>();

        /// <summary>
        /// Initialize the config when some script wants something from it
        /// </summary>
        static Config()
        {
            Log.Debug("Initializing config");

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
        /// Load all the data from the file
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
        /// Write all the data to the config file
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
        /// <param name="keys">The keys to get the value from</param>
        /// <returns>Returns the object cast to <paramref name="T"/></returns>
        /// <exception cref="InvalidCastException">When it failed to cast the value to <paramref name="T"/></exception>
        /// <exception cref="KeyNotFoundException">Gets thrown when the requested key chain does not exist</exception>
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
                if (dict.ContainsKey(keys[currIndex]))
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
                        throw new InvalidCastException(string.Format("The value of '{0}' is not a dictionary, the type is '{1}'", string.Join(".", keys.Take(currIndex + 1)), dict[keys[currIndex]].GetType().FullName));
                    }
                }
                else
                {
                    throw new KeyNotFoundException(string.Format("There is no key chain '{0}' in the config", string.Join(".", keys)));
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
        /// <exception cref="InvalidCastException">When it failed to cast the value to <paramref name="T"/></exception>
        /// <exception cref="KeyNotFoundException">Gets thrown when the key does not exist in <paramref name="dict"/></exception>
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
                        throw new InvalidCastException(string.Format("Failed to cast the value of '{0}' to {1}", key, typeof(T)));
                    }
                }
            }
            else
            {
                throw new KeyNotFoundException(string.Format("There is no key '{0}' in the dictionary", key));
            }
        }

        /// <summary>
        /// Check if the config has a certain key chain
        /// </summary>
        /// <param name="keys">Name to look for</param>
        public static bool ContainsKey(params string[] keys)
        {
            return ContainsKey(data, keys, 0);
        }

        private static bool ContainsKey(Dictionary<string, object> dict, string[] keys, int currIndex)
        {
            if (dict != null)
            {
                if (dict.ContainsKey(keys[currIndex]))
                {
                    if (currIndex == keys.Length - 1)
                    {
                        return true;
                    }
                    else
                    {
                        if (dict[keys[currIndex]].GetType() == typeof(Dictionary<string, object>))
                        {
                            return ContainsKey(
                            dict[keys[currIndex]] as Dictionary<string, object>,
                            keys,
                            ++currIndex);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Set a value in the config
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="keys">The key chain of which to set the value of</param>
        public static void SetValue(object value, params string[] keys)
        {
            SetValue(value, true, keys);
        }

        /// <summary>
        /// Set a value in the config
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="write">Should the config be written to disk after it successfully set the value</param>
        /// <param name="keys">The key chain of which to set the value of</param>
        public static void SetValue(object value, bool write, params string[] keys)
        {
            SetValue(data, keys, value, 0);
            Log.Debug("Set the value of '" + string.Join(".", keys) + "' in the config");

            if (write)
                WriteToFile();
        }

        /// <summary>
        /// Recursively go through the chain of keys and finally set the value
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="keys"></param>
        /// <param name="value"></param>
        /// <param name="keyIndexes"></param>
        /// <param name="currIndex"></param>
        /// <exception cref="InvalidCastException">When the value of one of the keys (not the last one) is not a dictionary</exception>
        private static void SetValue(Dictionary<string, object> dict, string[] keys, object value, int currIndex)
        {
            if (dict == null)
            {
                Log.Error("Failed to set a value in the config");
                return;
            }

            if (currIndex == keys.Length - 1)
            {
                dict[keys[currIndex]] = value;
            }
            else
            {
                if (!dict.ContainsKey(keys[currIndex]))
                    dict[keys[currIndex]] = new Dictionary<string, object>();

                if (dict[keys[currIndex]].GetType() == typeof(Dictionary<string, object>))
                {
                    SetValue(
                        dict[keys[currIndex]] as Dictionary<string, object>,
                        keys,
                        value,
                        ++currIndex);
                }
                else
                {
                    throw new InvalidCastException(string.Format("The value of '{0}' is not a dictionary", string.Join(".", keys.Take(currIndex + 1))));
                }
            }
        }
    }
}