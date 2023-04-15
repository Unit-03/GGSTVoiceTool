namespace Harmony
{
	public static partial class Paths
	{
		public static class Data
		{
			public static string Root => $"{Working}\\data";
			public static string Characters => $"{Root}\\Characters.json";
			public static string Languages  => $"{Root}\\Languages.json";
            public static string Voice      => $"{Root}\\Voice.json";
            public static string Narration  => $"{Root}\\Narration.json";
        }
	}
}
