namespace GGSTVoiceTool
{
	public static partial class Paths
	{
		public static class Generator
		{
			public class Module
			{ 
				public RefPath Unpack { get; private set; }
				public RefPath Pack   { get; private set; }
			}

			public static RefPath Temp { get; private set; }

			public static Module Voice     { get; private set; } = new();
			public static Module Narration { get; private set; } = new();
			public static Module Bundle    { get; private set; } = new();
		}
	}
}
