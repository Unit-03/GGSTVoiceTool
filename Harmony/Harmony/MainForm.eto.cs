using System;
using System.IO;

using Eto.Drawing;
using Eto.Forms;

namespace Harmony
{
	public partial class MainForm : Form
	{
		#region Constants

		private const string PATHS_DATA = "/data/Paths.json";

		#endregion

		#region Fields

		private PathText GamePath;
		private PathText InstallPath;

		private Scrollable voiceScroll;

		#endregion

		#region Methods

		private void InitializeComponent()
		{
			Data.Initialise();
			Console.WriteLine($"Initialised - Data: {Data.Initialised}");

			Title = "GGST Voice Tool";
			MinimumSize = new Size(480, 720);
			Padding = 10;

			DynamicLayout layout = new() {
				DefaultSpacing = new(8, 8)
			};

			SetupRootPaths(layout);

			layout.Add(new Scrollable(), true, true);

			Content = layout;

			// create a few commands that can be used for the menu and toolbar
			var clickMe = new Command { MenuText = "Click Me!", ToolBarText = "Click Me!" };
			clickMe.Executed += (sender, e) => MessageBox.Show(this, "I was clicked!");

			var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
			quitCommand.Executed += (sender, e) => Application.Instance.Quit();

			var aboutCommand = new Command { MenuText = "About..." };
			aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);

			// create menu
			Menu = new MenuBar {
				Items =
				{
					// File submenu
					new SubMenuItem { Text = "&File", Items = { clickMe } },
					// new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
					// new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
				},
				ApplicationItems =
				{
					// application (OS X) or file menu (others)
					new ButtonMenuItem { Text = "&Preferences..." },
				},
				QuitItem = quitCommand,
				AboutItem = aboutCommand
			};

			// create toolbar			
			//ToolBar = new ToolBar { Items = { clickMe } };
		}

		private void SetupRootPaths(DynamicLayout layout)
		{
			layout.BeginVertical();

			GamePath    = new PathText(layout, "Game Root");
			InstallPath = new PathText(layout, "Install Root");

			GamePath   .BrowseButton.Click += OnGameRootBrowse;
			InstallPath.BrowseButton.Click += OnInstallRootBrowse;

			layout.EndVertical();
		}

		#endregion
	}
}
