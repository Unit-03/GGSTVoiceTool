namespace Harmony
{
	public static partial class Paths
	{
		public static class GitHub
		{
			public static string URL       => $"https://github.com";
			public static string User      => $"Unit-03";
			public static string Repo      => $"GGSTVoiceMod";
			public static string RepoURL   => $"{URL}/{User}/{Repo}";
			public static string Latest    => $"{RepoURL}/releases/latest";
			public static string Narration => $"{RepoURL}/raw/main/Assets/Narration/{Language}/{Character}.zip";
		}
	}
}
