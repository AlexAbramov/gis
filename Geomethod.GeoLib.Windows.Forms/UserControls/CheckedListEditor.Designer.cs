using System;
using System.Globalization;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Geomethod.GeoLib.Windows.Forms
{
	partial class CheckedListUserControl
	{
		private System.Windows.Forms.CheckedListBox clbItems;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.clbItems = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// clbItems
			// 
			this.clbItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clbItems.Location = new System.Drawing.Point(0, 0);
			this.clbItems.Name = "clbItems";
			this.clbItems.Size = new System.Drawing.Size(100, 199);
			this.clbItems.TabIndex = 0;
			this.clbItems.SelectedIndexChanged += new System.EventHandler(this.clbItems_SelectedIndexChanged);
			// 
			// CheckedListUserControl
			// 
			this.Controls.Add(this.clbItems);
			this.Name = "CheckedListUserControl";
			this.Size = new System.Drawing.Size(100, 200);
			this.ResumeLayout(false);

		}
		#endregion
	}
}