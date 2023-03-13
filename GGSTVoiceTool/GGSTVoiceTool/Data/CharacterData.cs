using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GGSTVoiceTool
{
	public static partial class Data
	{
		public static CharacterData Character { get; private set; }

		[Serializable]
		public class CharacterName
		{
            #region Properties

            public string Name => _name;
            public string ShortName => _shortName ?? Name;

            #endregion

            #region Fields

            [JsonProperty("Name")]
			private string _name = null;
			[JsonProperty("Short", NullValueHandling = NullValueHandling.Ignore)]
			private string _shortName = null;

			#endregion

			#region Constructor

			public CharacterName(string name, string shortName = null)
			{
				_name = name;
				_shortName = shortName;
			}

			#endregion
		}

		public class CharacterData
		{
			#region Constants

			private static readonly CharacterName UNKNOWN_CHARACTER = new("UNKNOWN_CHARACTER", "UNKNOWN");

			#endregion

			#region Properties

			public CharacterName this[Character charId] => names.TryGetValue(charId, out CharacterName name) ? name : UNKNOWN_CHARACTER;

			#endregion

			#region Fields

			private readonly Dictionary<Character, CharacterName> names;

			#endregion

			#region Constructor

			public CharacterData(string json)
			{
				names = JsonConvert.DeserializeObject<Dictionary<Character, CharacterName>>(json);
			}

			#endregion
		}
	}
}
