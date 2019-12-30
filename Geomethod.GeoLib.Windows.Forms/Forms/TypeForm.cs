using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for TypeForm.
	/// </summary>
	public partial class TypeForm : System.Windows.Forms.Form
	{
  	public object geomType=null;

		public TypeForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


		private void TypeForm_Load(object sender, System.EventArgs e)
		{
			try
			{
                GmApplication.Initialize(this);
				btnOk.Enabled=false;
				foreach(GeomType gt in Enum.GetValues(typeof(GeomType)))
				{
					string s=GeoLib.GeoLibUtils.GetLocalizedName(gt);
					ListViewItem lvi=lvTypes.Items.Add(s);
					lvi.Tag=gt;
				}
				Resize();
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			this.MinimumSize=this.Size;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			OnOk();
		}

		void OnOk()
		{
			if(lvTypes.SelectedItems.Count>0)
			{
				geomType=(GeomType)lvTypes.SelectedItems[0].Tag;
			}
		}

		private void lvTypes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			btnOk.Enabled=lvTypes.SelectedItems.Count>0;
		}

		private void lvTypes_DoubleClick(object sender, System.EventArgs e)
		{
			OnOk();
		}

		private void lvTypes_Resize(object sender, System.EventArgs e)
		{
			Resize();
    }

		new void Resize()
		{
			chName.Width=lvTypes.Width-30;
		}
	}
}
