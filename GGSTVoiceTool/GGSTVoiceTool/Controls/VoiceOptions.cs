using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eto.Forms;
using Eto.Drawing;

namespace GGSTVoiceTool
{
	public class VoiceOptions
	{
		#region Properties

		public StackLayout Layout { get; }
		public Label Title { get; }

		public DropDown this[Language lang] => languageDrops[lang];

		#endregion

		#region Fields

		private Dictionary<Language, DropDown> languageDrops;

		#endregion

		#region Constructor

		public VoiceOptions(string title)
		{
			Layout = new() { 
				Orientation = Orientation.Horizontal, 
				Height = 28 
			};

			Title = new() { Text = title, VerticalAlignment = VerticalAlignment.Center };

			Language[] langs = Enum.GetValues<Language>();
			languageDrops = new();

			for (int i = 0; i < langs.Length; ++i)
			{
				DropDown drop = new();
				
				for (int u = 0; u < langs.Length; ++u)
					drop.Items.Add(Data.Language[langs[u]], langs[u].ToString());

				drop.SelectedIndex = i;

				languageDrops.Add(langs[i], drop);
				Layout.Items.Add(drop);
			}
		}

		#endregion

		#region Methods

		#endregion
	}
}
