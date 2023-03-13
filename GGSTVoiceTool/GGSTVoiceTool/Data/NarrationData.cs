using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GGSTVoiceTool
{
	public static partial class Data
	{
		public static NarrationData Narration { get; private set; }

		public class NarrationData
		{
			#region Properties

			public Language[] Languages => languages;
			public Character[] this[Language langId] => characters.TryGetValue(langId, out Character[] chars) ? chars : Array.Empty<Character>();

			#endregion

			#region Fields

			[JsonProperty("Languages")]
			private Language[] languages;
			[JsonProperty("Characters")]
			private Dictionary<Language, Character[]> characters;

			#endregion
		}
	}
}
