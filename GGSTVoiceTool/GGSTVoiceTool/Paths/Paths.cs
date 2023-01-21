using System;
using System.Reflection;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace GGSTVoiceTool
{
    public static partial class Paths
    {
		#region Properties

		public static bool Initialised { get; private set; }

		#endregion

		#region Methods

		public static void Initialise(string json)
		{
			Initialised = false;

			JObject state = JObject.Parse(json);
			RecursiveSetupPaths(typeof(Paths), state);

			Initialised = true;
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
