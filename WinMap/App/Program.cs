using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Geomethod;
using Geomethod.Windows.Forms;
using WinMap.Forms;

namespace WinMap
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			try
			{
				Application.EnableVisualStyles();
				MainForm.splashForm.Show();
				Application.DoEvents();
				Application.Run(new MainForm());
			}
			catch (Exception ex)
			{
				try
				{
					Log.Exception(ex);
				}
				catch
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

	}
}
