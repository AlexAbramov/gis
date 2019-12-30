using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	public partial class ColorUserControl : UserControl
	{
		public ColorUserControl()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();
		}

		private void ColorUserControl_Load(object sender, EventArgs e)
		{

		}
	}
}
