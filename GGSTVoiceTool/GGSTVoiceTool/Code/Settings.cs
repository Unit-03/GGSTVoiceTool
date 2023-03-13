using System;
using System.IO;
using System.Collections.Generic;

namespace GGSTVoiceTool
{
    public static class Settings
    {
        #region Constants

        public const string KEY_CACHE   = "cache";
        public const string KEY_BUNDLE  = "bundle";
        public const string KEY_GAME    = "gameRoot";
        public const string KEY_INSTALL = "installRoot";

        #endregion

        #region Properties

        public static bool? UseCache {
            get => _useCache;
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
                Paths.Game = Path.GetDirectoryName(value);
            }
        }

        public static string InstallPath {
            get => _installPath ?? Paths.Config.DefaultInstall;
            set {
                string dir = Path.GetDirectoryName(value);
                
                _installPath  = PathEx.Equivalent(value, Paths.Config.DefaultInstall) ? null : dir;
                Paths.Install = dir;
            }
        }

        #endregion

        #region Fields

        private static bool?  _useCache;
        private static bool?  _bundleMods;
        private static string _gamePath;
        private static string _installPath;

        #endregion

        #region Methods

        public static void Load()
        {
            if (!File.Exists(Paths.Config.Settings))
                return;

            IniFile file = new(Paths.Config.Settings);

            _useCache    = file.Read(KEY_CACHE,  new bool?());
            _bundleMods  = file.Read(KEY_BUNDLE, new bool?());
            _gamePath    = file.Read(KEY_GAME);
            _installPath = file.Read(KEY_INSTALL);
        }

        public static void Save()
        {
            using StreamWriter writer = File.CreateText(Paths.Config.Settings);

            foreach (var pair in settings)
                writer.WriteLine($"{pair.Key}={pair.Value}");
        }
    }
}
