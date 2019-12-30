using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	public partial class MapUserControl
	{
		private System.Windows.Forms.ContextMenu cmMap;
		private System.Windows.Forms.MenuItem miSaveAs;
		private System.Windows.Forms.SaveFileDialog dlgSaveFile;
		private System.Windows.Forms.MenuItem miEndEditing;
		private System.Windows.Forms.MenuItem miEditSelectedObject;
		private System.Windows.Forms.MenuItem miAddPointsMode;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cmMap = new System.Windows.Forms.ContextMenu();
			this.miSaveAs = new System.Windows.Forms.MenuItem();
			this.miEditSelectedObject = new System.Windows.Forms.MenuItem();
			this.miAddPointsMode = new System.Windows.Forms.MenuItem();
			this.miEndEditing = new System.Windows.Forms.MenuItem();
			this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// cmMap
			// 
			this.cmMap.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miSaveAs,
            this.miEditSelectedObject,
            this.miAddPointsMode,
            this.miEndEditing});
			this.cmMap.Popup += new System.EventHandler(this.cmMap_Popup);
			// 
			// miSaveAs
			// 
			this.miSaveAs.Index = 0;
			this.miSaveAs.Text = "_saveas";
			this.miSaveAs.Click += new System.EventHandler(this.miSaveAs_Click);
			// 
			// miEditSelectedObject
			// 
			this.miEditSelectedObject.Index = 1;
			this.miEditSelectedObject.Text = "_editselectedobject";
			this.miEditSelectedObject.Click += new System.EventHandler(this.miEditSelectedObject_Click);
			// 
			// miAddPointsMode
			// 
			this.miAddPointsMode.Checked = true;
			this.miAddPointsMode.Index = 2;
			this.miAddPointsMode.Text = "_addpointsmode";
			this.miAddPointsMode.Click += new System.EventHandler(this.miAddPointsMode_Click);
			// 
			// miEndEditing
			// 
			this.miEndEditing.Index = 3;
			this.miEndEditing.Text = "_endediting";
			this.miEndEditing.Click += new System.EventHandler(this.miEndEditing_Click);
			// 
			// MapUserControl
			// 
			this.ContextMenu = this.cmMap;
			this.MinimumSize = new System.Drawing.Size(1, 1);
			this.Name = "MapUserControl";
			this.Size = new System.Drawing.Size(376, 376);
			this.Load += new System.EventHandler(this.MapUserControl_Load);
			this.MouseLeave += new System.EventHandler(this.MapUserControl_MouseLeave);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapUserControl_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapUserControl_MouseMove);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapUserControl_MouseDown);
			this.Resize += new System.EventHandler(this.MapUserControl_Resize);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapUserControl_MouseUp);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapUserControl_KeyDown);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

	}
}