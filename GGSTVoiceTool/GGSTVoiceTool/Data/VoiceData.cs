using Newtonsoft.Json;

namespace GGSTVoiceTool
{
	public static partial class Data
	{
		public static VoiceData Voice { get; private set; }

		public class VoiceData
		{
			#region Properties

			public Language [] Languages  => languages;
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
