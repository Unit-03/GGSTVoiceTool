﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eto.Forms;
using Eto.Drawing;

namespace GGSTVoiceTool
{
	public class PathText
	{
		#region Properties

		// Controls
		public Label    Title        { get; }
		public TextArea PathDisplay  { get; }
		public Button   BrowseButton { get; }

		// Config
		public bool AllowDirectEditing {
			get => PathDisplay.Enabled;
			set => PathDisplay.Enabled = value;
		}

		#endregion

		#region Constructor

		public PathText(DynamicLayout layout, string title)
		{
			Size defaultSize = new(70, 22);	

			Title        = new() { Size = defaultSize, Text = title, VerticalAlignment = VerticalAlignment.Center };
			PathDisplay  = new() { Size = defaultSize                                                             };
			BrowseButton = new() { Size = defaultSize, Text = "Browse"                                            };

			layout.BeginHorizontal(false);

			layout.Add(Title,        false, false);
			layout.Add(PathDisplay,  true,  false);
			layout.Add(BrowseButton, false, false);
			
			layout.EndHorizontal();
		}

		#endregion
	}
}
