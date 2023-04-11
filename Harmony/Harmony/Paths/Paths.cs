using System;
using System.IO;
using System.Reflection;

namespace Harmony
{
    public static partial class Paths
    {
		//  The root directory of the tool executable
		public static string Working { get; } = Path.GetFullPath(
												Path.GetDirectoryName(
												Assembly.GetExecutingAssembly().Location));
		
		public static string Game    { get; set; } // The root directory for Guilty Gear -STRIVE-
		public static string Install { get; set; } // The root directory for installing mods to

		public static Language  Language  { get; set; } = Language .DEF; // The current active language  (e.g. ENG, JPN, etc.)
		public static Character Character { get; set; } = Character.DEF; // The current active character (e.g. RAM, ELP, etc.)
	}
}
