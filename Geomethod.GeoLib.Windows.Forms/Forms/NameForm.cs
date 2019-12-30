using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for NameForm.
	/// </summary>
	public partial class NameForm : System.Windows.Forms.Form
	{
		IEnumerable items;

		public string InputText{get{return tbName.Text;}set{tbName.Text=value;}}

		public NameForm(IEnumerable items)
		{
			this.items=items;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			
			if(items is Layers) Text="_layername";
			else if(items is Views) Text="_viewname";
		}


		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
			UpdateControls();
		}

		bool HasName()
		{
			string name=InputText;
			foreach(object item in items)
			{
				INamed n=item as INamed;
				if(n!=null)
				{
					if(string.Compare(n.Name,InputText,true)==0) return true;
				}
			}
			return false;
		}

		void UpdateControls()
		{
			btnOk.Enabled=!HasName();
		}

		private void NameForm_Load(object sender, System.EventArgs e)
		{
			GmApplication.Initialize(this);
			UpdateControls();
			MinimumSize=Size;
			MaximumSize=new Size(GConfig.Instance.geometry.maxFormWidth,Size.Height);
		}
	}
}
