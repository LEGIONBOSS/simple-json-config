using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Config
{
    public class Config
    {
        private static bool _loaded = false;

        private static bool _filePathSet = false;

        private static bool _unsaved = false;

        private static string _filePath = "config.json";

        private static Dictionary<string, string> _config = new Dictionary<string, string>();

        /// <summary>
        /// The file path of the value set
        /// </summary>
        public static string FilePath
        {
            get => _filePath;
            set
            {
                EnsureFilePathNotSet();
                if (!value.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    value += ".json";
                }
                _filePath = value;
                _filePathSet = true;
            }
        }

        private static void EnsureLoaded()
        {
            if (!_loaded)
            {
                Load();
            }
        }

        private static void EnsureFilePathNotSet()
        {
            if (_filePathSet)
            {
                throw new Exception("The file path has already been set.");
            }
        }

        private static void EnsureFilePathSet()
        {
            if (!_filePathSet)
            {
                throw new Exception("The file path has not been set. Set it with FilePath first.");
            }
        }

        /// <summary>
        /// Load the value set from the set file path
        /// </summary>
        public static void Load()
        {
            EnsureFilePathSet();
            if (_unsaved)
            {
                throw new Exception("There are unsaved changes that would be lost. Call Save() first.");
            }
            if (System.IO.File.Exists(_filePath))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(_filePath);
                    _config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error loading config file.", ex);
                }
            }
            _loaded = true;
        }

        /// <summary>
        /// Check if a key exists
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>True, if the key exists</returns>
        public static bool KeyExists(string key)
        {
            EnsureFilePathSet();
            EnsureLoaded();
            return _config.ContainsKey(key);
        }

        /// <summary>
        /// Get the current value for a key
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>Current value for the key or null</returns>
        public static string GetValue(string key)
        {
            EnsureFilePathSet();
            EnsureLoaded();
            if (_config.ContainsKey(key))
            {
                return _config[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the current value or a default value for a key
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="defaultValue">Value to return if the key does not exist</param>
        /// <returns>Current value for the key or defaultValue</returns>
        public static string GetValue(string key, string defaultValue)
        {
            EnsureFilePathSet();
            EnsureLoaded();
            if (_config.ContainsKey(key))
            {
                return _config[key];
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get the entire value set
        /// </summary>
        /// <returns>Current value set</returns>
        public static Dictionary<string, string> GetAllValues()
        {
            EnsureFilePathSet();
            EnsureLoaded();
            return new Dictionary<string, string>(_config);
        }

        /// <summary>
        /// Set a new value for a key or create a new key with a value
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="value">New value</param>
        public static void SetValue(string key, string value)
        {
            EnsureFilePathSet();
            EnsureLoaded();
            _config[key] = value;
            _unsaved = true;
        }

        /// <summary>
        /// Overwrite the entire value set
        /// </summary>
        /// <param name="values">New value set</param>
        public static void SetAllValues(Dictionary<string, string> values)
        {
            EnsureFilePathSet();
            EnsureLoaded();
            _config = new Dictionary<string, string>(values);
            _unsaved = true;
        }

        /// <summary>
        /// Save the current value set
        /// </summary>
        public static void Save()
        {
            EnsureFilePathSet();
            EnsureLoaded();
            try
            {
                string json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(_filePath, json);
                _unsaved = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving config file.", ex);
            }
        }
    }
}
