namespace Harmony
{
	public static partial class Paths
	{
		public static class Config
		{
			public static string Settings => $"{Working}\\settings.json";
			public static string GamePaks => $"{Game}\\RED\\Content\\Paks";
			public static string GameSig  => $"{GamePaks}\\pakchunk0-WindowsNoEditor.sig";

			public static string DefaultInstall => $"{Game}\\~mods\\IVOMod";
			public static string Manifest      => $"{Install}\\manifest.json";
		}
	}
}
