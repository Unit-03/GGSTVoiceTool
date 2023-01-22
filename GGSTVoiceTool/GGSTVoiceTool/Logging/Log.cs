using System;
using System.IO;

namespace GGSTVoiceTool
{
	public enum LogLevel
	{
		DEBUG,
		INFO,
		WARNING,
		ERROR,
	}

	public static class Log
	{
		#region Properties

		public static bool Initiliased { get; private set; }

		public static int Indent     { get; set; }
		public static int IndentSize { get; set; }

		public static LogLevel MinLevel { get; set; }
#if LOG_DEBUG || DEBUG
			= LogLevel.DEBUG;
#else
			= LogLevel.INFO;
#endif

		#endregion

		#region Fields

		private static StreamWriter stream;

		#endregion

		#region Methods

		public static bool Initialise()
		{
			if (Initiliased)
				return true;

			if (!Paths.Initialised)
				return false;
		}

		public static void Write(object message, LogLevel level = LogLevel.INFO)
		{
			if ((int)level < (int)MinLevel)
				return;


		}

		#endregion
	}
}
