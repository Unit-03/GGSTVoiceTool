using System;
using System.Collections.Generic;
using System.IO;

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

			public string Name { get; set; }

			public string ShortName {
				get => _shortName ?? Name;
				set => _shortName = value;
			}

			#endregion

			#region Fields

			private string _shortName = null;

			#endregion

			#region Constructor

			public CharacterName(string name, string shortName = null)
			{
				Name = name;
				ShortName = shortName;
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

			private Dictionary<Character, CharacterName> names;

			#endregion

			#region Constructor

			public CharacterData()
			{
				names = new();
			}

			public CharacterData(string json)
			{
				names = JsonConvert.DeserializeObject<Dictionary<Character, CharacterName>>(json);
			}

			#endregion
		}
	}
}
