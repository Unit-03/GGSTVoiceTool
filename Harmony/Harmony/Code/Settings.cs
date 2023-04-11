using System.IO;

namespace Harmony
{
    public static class Settings
    {
        #region Constants

        private const string KEY_CACHE   = "cache";
        private const string KEY_BUNDLE  = "bundle";
        private const string KEY_GAME    = "gameRoot";
        private const string KEY_INSTALL = "installRoot";

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
            IniFile file = new(Paths.Config.Settings);

			if (_useCache.HasValue)   file.Write(KEY_CACHE,  _useCache  .Value);
			if (_bundleMods.HasValue) file.Write(KEY_BUNDLE, _bundleMods.Value);

			if (!string.IsNullOrEmpty(_gamePath))    file.Write(KEY_GAME,    _gamePath);
			if (!string.IsNullOrEmpty(_installPath)) file.Write(KEY_INSTALL, _installPath);
		}

		#endregion
	}
}
