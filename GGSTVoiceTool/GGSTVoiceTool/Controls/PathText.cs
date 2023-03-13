using Eto.Forms;
using Eto.Drawing;

namespace GGSTVoiceTool
{
	public class PathText
	{
		#region Properties

		public Label    Title        { get; }
		public TextArea PathDisplay  { get; }
		public Button   BrowseButton { get; }

		public bool Enabled {
			get => PathDisplay.Enabled;
			set => PathDisplay.Enabled = value;
		}

		#endregion

		#region Constructor

		public PathText(DynamicLayout layout, string title)
		{
			Size defaultSize = new(70, 22);	

			Title        = new() { Size = defaultSize, Text = title, VerticalAlignment = VerticalAlignment.Center };
			PathDisplay  = new() { Size = defaultSize };
			BrowseButton = new() { Size = defaultSize, Text = "Browse" };

			layout.BeginHorizontal(false);

			layout.Add(Title,        false, false);
			layout.Add(PathDisplay,  true,  false);
			layout.Add(BrowseButton, false, false);
			
			layout.EndHorizontal();
		}

		#endregion
	}
}
