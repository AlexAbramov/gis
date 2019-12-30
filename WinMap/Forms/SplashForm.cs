using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod;

namespace WinMap.Forms
{
	/// <summary>
	/// Summary description for SplashForm.
	/// </summary>
	public partial class SplashForm : System.Windows.Forms.Form
	{

		public SplashForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


		private void SplashForm_Load(object sender, System.EventArgs e)
		{
			lblProgName.Text = Application.ProductName + " " + StringUtils.TrimVersion(Application.ProductVersion);		
//			lblProgName.Visible=true;
		}

		private void SplashForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle r=lblProgName.Bounds;
			e.Graphics.DrawString(lblProgName.Text,lblProgName.Font,Brushes.White,RectangleF.FromLTRB(r.Left,r.Top,r.Right,r.Bottom));
		}

		public void Phase(bool dir)
		{
			for(double d=0.01;d<1;d+=0.01)
			{
				base.Opacity=dir?d:1-d;
				System.Threading.Thread.Sleep(10);
			}
		}
	}
}
