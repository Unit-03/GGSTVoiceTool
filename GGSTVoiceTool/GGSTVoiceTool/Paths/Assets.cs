namespace GGSTVoiceTool
{
	public static partial class Paths
	{
		public static class Assets
		{
			public class Module
			{ 
				public RefPath Path  { get; private set; }
				public RefPath URL   { get; private set; }
				public RefPath Cache { get; private set; }
			}

			public static RefPath URL   { get; private set; }
			public static RefPath Cache { get; private set; }

			public static Module Voice     { get; private set; } = new();
			public static Module Narration { get; private set; } = new();
		}
	}
}
