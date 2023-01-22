using System;
using System.IO;
using System.Reflection;

using Newtonsoft.Json.Linq;

namespace GGSTVoiceTool
{
    public static partial class Paths
    {
		#region Properties

		public static bool Initialised { get; private set; }

		//  The root directory of the tool executable
		public static string ExeRoot { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		
		public static string Game    { get; set; } // The root directory for Guilty Gear -STRIVE-
		public static string Install { get; set; } // The root directory for installing mods to

		public static Language  Language  { get; set; } = Language .DEF; // The current active language  (e.g. ENG, JPN, etc.)
		public static Character Character { get; set; } = Character.DEF; // The current active character (e.g. RAM, ELP, etc.)

		#endregion

		#region Methods

		public static bool Initialise(string json)
		{
			if (Initialised)
				return true;

			try
			{
				JObject state = JObject.Parse(json);
				RecursiveSetupPaths(typeof(Paths), state);
			}
			catch (Exception ex)
			{

			}

			Initialised = true;
			return true;
		}

		private static void RecursiveSetupPaths(Type parent, JObject state)
		{
			foreach (var pair in state)
			{
				if (pair.Value.Type == JTokenType.Object)
				{
					Type child = parent.GetNestedType(pair.Key);

					if (child != null)
						RecursiveSetupPaths(child, pair.Value as JObject);
				}
				else
				{
					SetupPath(parent, pair.Key, pair.Value.ToString());
				}
			}
		}

		private static void SetupPath(Type parent, string property, string path)
		{
			PropertyInfo info = parent.GetProperty(property);

			if (info == null)
				return;

			info.SetValue(null, new RefPath(path, parent));
		}

		#endregion
	}
}
