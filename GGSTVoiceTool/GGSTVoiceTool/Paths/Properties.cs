using System.IO;
using System.Reflection;

namespace GGSTVoiceTool
{
	public static partial class Paths
	{
		public static class Properties
		{
			// Just storing the executable directory at launch for use in paths later
			public static string ExeRoot { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			public static string Game    { get; set; } // The root directory for Guilty Gear -STRIVE-
			public static string Install { get; set; } // The root directory for installing mods to

			public static Language  Language  { get; set; } = Language .DEF; // The current active language  (e.g. ENG, JPN, etc.)
			public static Character Character { get; set; } = Character.DEF; // The current active character (e.g. RAM, ELP, etc.)
		}
	}
}
