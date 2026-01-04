using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConfigLib
{
    public static class Config
    {
        private static bool _set = false;
        private static bool _unsaved = false;

        private static string _filePath = "config.json";
        private static Dictionary<string, string> _config = new Dictionary<string, string>();

        private static string _setExMsg = "No current value set is set. Call Set() before making changes.";
        private static string _savedExMsg = "There are unsaved changes that would be lost. Call Save() first.";

        private static void EnsureSet()
        {
            if (!_set)
            {
                throw new Exception(_setExMsg);
            }
        }

        private static void EnsureSaved()
        {
            if (_unsaved)
            {
                throw new Exception(_savedExMsg);
            }
        }

        /// <summary>
        /// The file path of the value set
        /// </summary>
        public static string FilePath => _filePath;

        /// <summary>
        /// Check if a value set file exists
        /// </summary>
        /// <param name="filePath">Value set file path</param>
        /// <returns>True, if the value set file exists</returns>
        public static bool ConfigExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// Set a new or existing value set to use
        /// </summary>
        /// <param name="filePath">Value set file path</param>
        /// <exception cref="Exception">asd</exception>
        public static void Set(string filePath)
        {
            EnsureSaved();

            if (ConfigExists(filePath))
            {
                // Existing
                try
                {
                    string json = File.ReadAllText(filePath);
                    _config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error loading config file {filePath}.", ex);
                }
            }

            // Existing or new
            _filePath = filePath;
            _set = true;
            _unsaved = false;
        }

        /// <summary>
        /// Check if a key exists
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>True, if the key exists</returns>
        public static bool KeyExists(string key)
        {
            EnsureSet();

            return _config.ContainsKey(key);
        }

        /// <summary>
        /// Get the current value for a key
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <returns>Current value for the key or null</returns>
        public static string GetValue(string key)
        {
            EnsureSet();

            if (KeyExists(key))
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
            EnsureSet();

            if (KeyExists(key))
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
            EnsureSet();

            return new Dictionary<string, string>(_config);
        }

        /// <summary>
        /// Set a new value for a key or create a new key with a value
        /// </summary>
        /// <param name="key">Unique key</param>
        /// <param name="value">New value</param>
        public static void SetValue(string key, string value)
        {
            EnsureSet();

            _config[key] = value;
            _unsaved = true;
        }

        /// <summary>
        /// Overwrite the entire value set
        /// </summary>
        /// <param name="values">New value set</param>
        public static void SetAllValues(Dictionary<string, string> values)
        {
            EnsureSet();

            _config = new Dictionary<string, string>(values);
            _unsaved = true;
        }

        /// <summary>
        /// Save the current value set
        /// </summary>
        public static void Save()
        {
            EnsureSet();

            try
            {
                string json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                _unsaved = false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving config file {_filePath}.", ex);
            }
        }
    }
}
