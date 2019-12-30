using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	partial class TypesUserControl
	{
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
			this.tvTypes = new System.Windows.Forms.TreeView();
			this.cmTypes = new System.Windows.Forms.ContextMenu();
			this.miAddObject = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.miSearch = new System.Windows.Forms.MenuItem();
			this.miAddType = new System.Windows.Forms.MenuItem();
			this.miRemoveType = new System.Windows.Forms.MenuItem();
			this.miMove = new System.Windows.Forms.MenuItem();
			this.miMoveUp = new System.Windows.Forms.MenuItem();
			this.miMoveDown = new System.Windows.Forms.MenuItem();
			this.miSep = new System.Windows.Forms.MenuItem();
			this.miMask = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.miStat = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// tvTypes
			// 
			this.tvTypes.CheckBoxes = true;
			this.tvTypes.ContextMenu = this.cmTypes;
			this.tvTypes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvTypes.HideSelection = false;
			this.tvTypes.Location = new System.Drawing.Point(0, 0);
			this.tvTypes.Name = "tvTypes";
			this.tvTypes.Size = new System.Drawing.Size(216, 392);
			this.tvTypes.TabIndex = 2;
			this.tvTypes.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTypes_AfterCheck);
			this.tvTypes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvTypes_MouseUp);
			this.tvTypes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTypes_AfterSelect);
			this.tvTypes.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tvTypes_MouseMove);
			this.tvTypes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvTypes_MouseDown);
			this.tvTypes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvTypes_KeyDown);
			// 
			// cmTypes
			// 
			this.cmTypes.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miAddObject,
            this.menuItem1,
            this.miSearch,
            this.miAddType,
            this.miRemoveType,
            this.miMove,
            this.miSep,
            this.miMask,
            this.menuItem2,
            this.miStat});
			this.cmTypes.Popup += new System.EventHandler(this.cmTypes_Popup);
			// 
			// miAddObject
			// 
			this.miAddObject.Index = 0;
			this.miAddObject.Text = "_addobject";
			this.miAddObject.Click += new System.EventHandler(this.miAddObject_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.Text = "-";
			// 
			// miSearch
			// 
			this.miSearch.Index = 2;
			this.miSearch.Text = "_search";
			this.miSearch.Click += new System.EventHandler(this.miSearch_Click);
			// 
			// miAddType
			// 
			this.miAddType.Index = 3;
			this.miAddType.Text = "_addtype";
			this.miAddType.Click += new System.EventHandler(this.miAddType_Click);
			// 
			// miRemoveType
			// 
			this.miRemoveType.Index = 4;
			this.miRemoveType.Text = "_removetype";
			this.miRemoveType.Click += new System.EventHandler(this.miRemoveType_Click);
			// 
			// miMove
			// 
			this.miMove.Index = 5;
			this.miMove.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miMoveUp,
            this.miMoveDown});
			this.miMove.Text = "_move";
			// 
			// miMoveUp
			// 
			this.miMoveUp.Index = 0;
			this.miMoveUp.Text = "_up";
			this.miMoveUp.Click += new System.EventHandler(this.miMoveUp_Click);
			// 
			// miMoveDown
			// 
			this.miMoveDown.Index = 1;
			this.miMoveDown.Text = "_down";
			this.miMoveDown.Click += new System.EventHandler(this.miMoveDown_Click);
			// 
			// miSep
			// 
			this.miSep.Index = 6;
			this.miSep.Text = "-";
			// 
			// miMask
			// 
			this.miMask.Checked = true;
			this.miMask.Index = 7;
			this.miMask.Text = "_mask";
			this.miMask.Click += new System.EventHandler(this.miMask_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 8;
			this.menuItem2.Text = "-";
			// 
			// miStat
			// 
			this.miStat.Index = 9;
			this.miStat.Text = "_stat";
			this.miStat.Click += new System.EventHandler(this.miStat_Click);
			// 
			// TypesUserControl
			// 
			this.Controls.Add(this.tvTypes);
			this.Name = "TypesUserControl";
			this.Size = new System.Drawing.Size(216, 392);
			this.Load += new System.EventHandler(this.TypesUserControl_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TreeView tvTypes;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem miMask;
		private System.Windows.Forms.MenuItem miSearch;
		private System.Windows.Forms.MenuItem miAddObject;
		private System.Windows.Forms.MenuItem miSep;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem miAddType;
		private System.Windows.Forms.MenuItem miRemoveType;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem miStat;
		private System.Windows.Forms.MenuItem miMove;
		private System.Windows.Forms.MenuItem miMoveUp;
		private System.Windows.Forms.MenuItem miMoveDown;
		public ContextMenu cmTypes;

	}
}