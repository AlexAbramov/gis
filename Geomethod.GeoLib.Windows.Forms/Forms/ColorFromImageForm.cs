using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Geomethod;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms.Forms
{
	public partial class ColorFromImageForm : Form
	{
		Bitmap bitmap = null;
		public ColorFromImageForm()
		{
			InitializeComponent();
		}

		private void btnLoadImage_Click(object sender, EventArgs e)
		{
			try
			{
				if (dlgOpenFile.ShowDialog() == DialogResult.OK)
				{
//					LoadImage(dlgOpenFile.FileName);
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

/*		private void LoadImage(string filePath)
		{
			try
			{
				bitmap = new Bitmap(filePath);
				pictureBox.Image = bitmap;
				palette.imageFilePath = filePath;
				lblImagePath.Text = filePath;
				if (tbPaletteName.Text.Trim().Length == 0)
				{
					tbPaletteName.Text = Path.GetFileNameWithoutExtension(filePath);
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}*/

		private void pictureBox_Click(object sender, EventArgs e)
		{
		}

		private void pictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (bitmap != null)
			{
				int x = e.X;
				int y = e.Y;
				if (x >= 0 && y >= 0 && x < bitmap.Width && y < bitmap.Height)
				{
					Color c = bitmap.GetPixel(x, y);
//					lblColor.BackColor = c;
//					UpdateControls();
				}
			}
		}

		private void ColorFromImageForm_Load(object sender, EventArgs e)
		{
			GmApplication.Initialize(this);
			MinimumSize = Size;
			lblImagePath.Text = "";
/*			if (File.Exists(palette.imageFilePath))
			{
				LoadImage(palette.imageFilePath);
			}*/
		}


	}
}