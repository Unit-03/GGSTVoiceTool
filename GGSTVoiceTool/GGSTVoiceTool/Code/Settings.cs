using System;
using System.IO;
using System.Collections.Generic;

namespace GGSTVoiceTool
{
    public static class Settings
    {
        #region Constants

        public const string CACHE_ID   = "cache";
        public const string BUNDLE_ID  = "bundle";
        public const string GAME_ID    = "gameRoot";
        public const string INSTALL_ID = "installRoot";

        #endregion

        #region Properties

        public static bool? UseCache {
            get => false;
            set => _useCache = value;
        }

        public static bool? BundleMods {
            get => _bundleMods;
            set => _bundleMods = value;
        }

        public static string GamePath {
            get => _gamePath;
            set {
                _gamePath = value;
                Paths.Properties.Game = Path.GetDirectoryName(value);
            }
        }

        public static string InstallPath {
            get => _installPath;
            set {
                string dir = (value == Paths.Config.Install) ? null : Path.GetDirectoryName(value);

                _installPath = dir;
                Paths.Properties.Install = dir;
            }
        }

        #endregion

        #region Fields

        // Some of these could just be auto-properties but for consistency and potential changes to their functionality later I'm keeping them like this
        private static bool? _useCache;
        private static bool? _bundleMods;
        private static string _gamePath;
        private static string _installPath;

        private static Dictionary<string, object> settings  = new Dictionary<string, object>();
        private static Dictionary<string, Action<object, object>> callbacks = new Dictionary<string, Action<object, object>>();

        #endregion

        #region Methods

        // File Handling

        public static void Load()
        {
            if (!File.Exists(Paths.Config.Settings))
                return;

            string[] lines = File.ReadAllLines(Paths.Config.Settings);

            // This is a pretty simple and loose "ini" style settings format, nothing fancy just basic variables
            // It will attempt for interpret anything in the format "[name]=[value]", extra '=' are ignored and improperly formatted lines are skipped 
            for (int i = 0; i < lines.Length; ++i)
            {
                string[] parts = lines[i].Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length < 2)
                    continue;

                string name  = parts[0].Trim();
                string value = parts[1].Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
                    continue;

                settings.Add(name, value);

                switch (name)
                {
                    case CACHE_ID:
                        if (bool.TryParse(value, out bool cache))
                            UseCache = cache;
                        break;
                    case BUNDLE_ID:
                        if (bool.TryParse(value, out bool bundle))
                            BundleMods = bundle;
                        break;
                    case GAME_ID:
                        if (File.Exists(value))
                            GamePath = value;
                        break;
                    case INSTALL_ID:
                        if (Path.IsPathFullyQualified(value))
                            InstallPath = value;
                        break;
                }
            }
        }

        public static void Save()
        {
            using StreamWriter writer = File.CreateText(Paths.Config.Settings);

            foreach (var pair in settings)
                writer.WriteLine($"{pair.Key}={pair.Value}");
        }

        // Callbacks

        public static void AddCallback(string key, Action<object, object> callback)
        {
            if (!callbacks.ContainsKey(key))
                callbacks.Add(key, callback);
            else
                callbacks[key] += callback;
        }

        public static void RemoveCallback(string key, Action<object, object> callback)
        {
            if (callbacks.ContainsKey(key))
                callbacks[key] -= callback;
        }

        // Accessors

        public static void SetValue(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value?.ToString()))
                return;

            object newVal = value;
            object oldVal = settings.ContainsKey(key) ? settings[key] : null;

            if (newVal != oldVal)
            {
                if (value == null)
                    settings.Remove(key);
                else
                    settings[key] = value;
            }

            if (callbacks.ContainsKey(key))
                callbacks[key]?.Invoke(newVal, oldVal);
        }

        public static object GetValue(string key)
        {
            return settings.ContainsKey(key) ? settings[key] : null;
        }

        public static bool TryGetValue(string key, out object value)
        {
            bool hasKey = settings.ContainsKey(key);

            value = hasKey ? settings[key] : null;
            return hasKey;
        }

        public static T GetValue<T>(string key)
        {
            return settings.ContainsKey(key) && settings[key] is T tVal ? tVal : default;
        }

        public static bool TryGetValue<T>(string key, out T value)
        {
            bool hasKey = settings.ContainsKey(key);

            value = hasKey && settings[key] is T tVal ? tVal : default;
            return hasKey;
        }

        private static object EvaluateString(string value)
        {
            if (bool.TryParse(value, out bool boolVal))
                return boolVal;
            if (int.TryParse(value, out int intVal))
                return intVal;

			return null;
        }

        private static T EvaluateString<T>(string value)
        {
            Type tType = typeof(T);

            if (tType == typeof(bool))
            {
                if (bool.TryParse(value, out bool boolValue))
                    return (T)(object)boolValue;
                else
                    return (T)(object)false;
            }

			return default;
        }

        #endregion
    }
}
