using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for PropertiesUserControl.
	/// </summary>
	public partial class PropertiesUserControl : System.Windows.Forms.UserControl
	{
        IGeoApp app = null;

        public void InitControl(IGeoApp app) { this.app = app; }
        public event EventHandler<System.Windows.Forms.PropertyValueChangedEventArgs> OnPropertyValueChanged;
		
		#region Properties
		public object SelectedObject
		{
			get
			{
				return this.propertyGrid.SelectedObject;
			}
			set
			{
				propertyGrid.SelectedObject=value;
			}
		}
		#endregion

		public PropertiesUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// TODO: Add any initialization after the InitializeComponent call
		}


		private void PropertiesUserControl_Load(object sender, System.EventArgs e)
		{
		}

		private void propertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			if(OnPropertyValueChanged!=null) OnPropertyValueChanged(this, e);
            object selObj = SelectedObject;
            if (selObj != null)
            {
                LocalizedObject lo = selObj as LocalizedObject;
                if (lo != null)
                {
                    app.DataChanged(lo.Object);
                }
            }
        }

		private void btnApply_Click(object sender, System.EventArgs e)
		{
		}

        private void propertyGrid_Click(object sender, EventArgs e)
        {

        }
	}
}
