using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{

	partial class ViewsUserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox lbViews;
		private System.Windows.Forms.ContextMenu cmViews;
		private System.Windows.Forms.MenuItem miAdd;
		private System.Windows.Forms.MenuItem miUpdate;
		private System.Windows.Forms.MenuItem miRename;
		private System.Windows.Forms.MenuItem miRemove;
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
			this.lbViews = new System.Windows.Forms.ListBox();
			this.cmViews = new System.Windows.Forms.ContextMenu();
			this.miAdd = new System.Windows.Forms.MenuItem();
			this.miUpdate = new System.Windows.Forms.MenuItem();
			this.miRename = new System.Windows.Forms.MenuItem();
			this.miRemove = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// lbViews
			// 
			this.lbViews.ContextMenu = this.cmViews;
			this.lbViews.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbViews.Location = new System.Drawing.Point(0, 0);
			this.lbViews.Name = "lbViews";
			this.lbViews.Size = new System.Drawing.Size(224, 433);
			this.lbViews.TabIndex = 1;
			this.lbViews.SelectedIndexChanged += new System.EventHandler(this.lbViews_SelectedIndexChanged);
			this.lbViews.SelectedValueChanged += new System.EventHandler(this.lbViews_SelectedValueChanged);
			this.lbViews.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbViews_KeyDown);
			// 
			// cmViews
			// 
			this.cmViews.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miAdd,
            this.miUpdate,
            this.miRename,
            this.miRemove});
			this.cmViews.Popup += new System.EventHandler(this.cmViews_Popup);
			// 
			// miAdd
			// 
			this.miAdd.Index = 0;
			this.miAdd.Text = "_add";
			this.miAdd.Click += new System.EventHandler(this.miAdd_Click);
			// 
			// miUpdate
			// 
			this.miUpdate.Index = 1;
			this.miUpdate.Text = "_update";
			this.miUpdate.Click += new System.EventHandler(this.miUpdate_Click);
			// 
			// miRename
			// 
			this.miRename.Index = 2;
			this.miRename.Text = "_rename";
			this.miRename.Click += new System.EventHandler(this.miRename_Click);
			// 
			// miRemove
			// 
			this.miRemove.Index = 3;
			this.miRemove.Text = "_remove";
			this.miRemove.Click += new System.EventHandler(this.miRemove_Click);
			// 
			// ViewsUserControl
			// 
			this.Controls.Add(this.lbViews);
			this.Name = "ViewsUserControl";
			this.Size = new System.Drawing.Size(224, 440);
			this.Load += new System.EventHandler(this.ViewsUserControl_Load);
			this.ResumeLayout(false);

		}
		#endregion
	}
}