using System;

using Eto.Drawing;
using Eto.Forms;

namespace Harmony
{
	internal class Program
	{
		public static readonly Version Version = new(0, 1, 0);

		[STAThread]
		static void Main(string[] args)
		{
			new Application(Eto.Platform.Detect).Run(new MainForm());
		}
	}
}
