using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for ScalesForm.
	/// </summary>
	public partial class ScalesForm : System.Windows.Forms.Form
	{
		GLib lib;
		List<int> scales;


		public ScalesForm(GLib lib)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.lib = lib;
			scales = new List<int>(lib.ScalesArray);
		}

		private void ScalesForm_Load(object sender, System.EventArgs e)
		{
			GmApplication.Initialize(this);
			foreach (int scale in scales)
			{
				listBox.Items.Add(scale);
			}
			UpdateControls();
			MinimumSize=Size;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			lib.Scales.Values = scales.ToArray();
			Close();
		}

		private void textBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

		private void listBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateControls();
		}

		void UpdateControls()
		{
			int sel=listBox.SelectedIndex;
			btnRemove.Enabled=sel>=0;
			addButton.Enabled=CanAdd();
		}

		bool CanAdd()
		{
  		    if(textBox.Text.Length==0) return false;
			try
			{
				int scale=int.Parse(textBox.Text);
				if(!lib.Scales.IsValid(scale)) return false;
				foreach(int i in listBox.Items)
				{
					if(scale==i) return false;
				}
				listBox.Items.Add(scale);
				return true;
			}
			catch
			{
				return false;
			}
		}
		public static string GetScale(int scale)
		{
			return scale == 0 ? "" : scale.ToString();
		}
		public static int GetScale(string scale)
		{
			scale = scale.Trim();
			if (scale.Length == 0) return 0;
			int res = int.Parse(scale);
			if (res < 0) res = 0;
			return res;
		}

		private void addButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				int scale=int.Parse(textBox.Text);
				scales.Add(scale);
			}
			catch
			{
			}
		}
	}
}
