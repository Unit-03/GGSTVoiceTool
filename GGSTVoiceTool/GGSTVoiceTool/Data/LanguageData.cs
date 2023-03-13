using System.Collections.Generic;

using Newtonsoft.Json;

namespace GGSTVoiceTool
{
	public static partial class Data
	{
		public static LanguageData Language { get; private set; }

		public class LanguageData
		{
			#region Constants

			private const string UNKNOWN_LANGUAGE = "UNKNOWN_LANGUAGE";

			#endregion

			#region Properties

			public string this[Language langId] => languages.TryGetValue(langId, out string name) ? name : UNKNOWN_LANGUAGE;

			#endregion

			#region Fields

			private readonly Dictionary<Language, string> languages;

			#endregion

			#region Constructor

			public LanguageData()
			{
				languages = new();
			}

			public LanguageData(string json)
			{
				languages = JsonConvert.DeserializeObject<Dictionary<Language, string>>(json);
			}

			#endregion
		}
	}
}
