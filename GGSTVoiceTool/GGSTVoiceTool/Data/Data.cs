using System.IO;

using Newtonsoft.Json;

namespace GGSTVoiceTool
{
	public static partial class Data
	{
		#region Properties

		public static bool Initialised { get; private set; }

		#endregion

		#region Methods

		public static void Initialise()
		{
			Initialised = false;

			if (!Paths.Initialised)
				return;

			Character = new CharacterData(File.ReadAllText(Paths.Data.Characters));
			Language  = new LanguageData (File.ReadAllText(Paths.Data.Languages ));

			Voice     = JsonConvert.DeserializeObject<VoiceData>    (File.ReadAllText(Paths.Data.Voice    ));
			Narration = JsonConvert.DeserializeObject<NarrationData>(File.ReadAllText(Paths.Data.Narration));
		}

		#endregion
	}
}
