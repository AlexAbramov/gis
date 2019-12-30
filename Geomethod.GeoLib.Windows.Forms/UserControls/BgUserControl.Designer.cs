using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;

namespace Geomethod.GeoLib.Windows.Forms
{
	partial class BgUserControl
	{
		private System.Windows.Forms.TreeView tvItems;
		private System.Windows.Forms.ContextMenu cmBackground;
		private System.Windows.Forms.MenuItem miAdd;
		private System.Windows.Forms.MenuItem miRemove;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
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
			this.tvItems = new System.Windows.Forms.TreeView();
			this.cmBackground = new System.Windows.Forms.ContextMenu();
			this.miAdd = new System.Windows.Forms.MenuItem();
			this.miRemove = new System.Windows.Forms.MenuItem();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// tvItems
			// 
			this.tvItems.CheckBoxes = true;
			this.tvItems.ContextMenu = this.cmBackground;
			this.tvItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvItems.FullRowSelect = true;
			this.tvItems.HideSelection = false;
			this.tvItems.ImageIndex = -1;
			this.tvItems.Location = new System.Drawing.Point(0, 0);
			this.tvItems.Name = "tvItems";
			this.tvItems.SelectedImageIndex = -1;
			this.tvItems.ShowLines = false;
			this.tvItems.ShowPlusMinus = false;
			this.tvItems.ShowRootLines = false;
			this.tvItems.Size = new System.Drawing.Size(304, 360);
			this.tvItems.TabIndex = 3;
			this.tvItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvItems_MouseDown);
			this.tvItems.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvItems_AfterCheck);
			this.tvItems.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvItems_AfterSelect);
			// 
			// cmBackground
			// 
			this.cmBackground.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																								 this.miAdd,
																																								 this.miRemove});
			// 
			// miAdd
			// 
			this.miAdd.Index = 0;
			this.miAdd.Text = "_add";
			this.miAdd.Click += new System.EventHandler(this.miAdd_Click);
			// 
			// miRemove
			// 
			this.miRemove.Index = 1;
			this.miRemove.Text = "_remove";
			this.miRemove.Click += new System.EventHandler(this.miRemove_Click);
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.ShowReadOnly = true;
			// 
			// BgUserControl
			// 
			this.Controls.Add(this.tvItems);
			this.Name = "BgUserControl";
			this.Size = new System.Drawing.Size(304, 360);
			this.Load += new System.EventHandler(this.BgUserControl_Load);
			this.ResumeLayout(false);

		}
		#endregion
	}
}