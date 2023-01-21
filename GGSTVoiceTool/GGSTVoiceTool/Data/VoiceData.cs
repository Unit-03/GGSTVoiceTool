using Newtonsoft.Json;

namespace GGSTVoiceTool
{
	public static partial class Data
	{
		public static VoiceData Voice { get; private set; }

		public class VoiceData
		{
			#region Properties

			[JsonIgnore]
			public Language[] Languages => languages;
			[JsonIgnore]
			public Character[] Characters => characters;

			#endregion

			#region Fields

			[JsonProperty("Languages")]
			private Language[] languages;
			[JsonProperty("Characters")]
			private Character[] characters;

			#endregion
		}
	}
}
