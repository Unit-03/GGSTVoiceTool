namespace GGSTVoiceTool
{
	public static partial class Paths
	{
		public static class Generator
		{
			public static string Temp => $"{Working}\\~temp";

			public static string Voice     => $"{Temp}\\VO_{Language}_{Character}.pak";
			public static string Narration => $"{Temp}\\NA_{Language}_{Character}.pak";
			public static string Bundle    => $"{Temp}\\Bundle.pak";
		}
	}
}
