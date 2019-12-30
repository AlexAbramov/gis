using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for SearchUserControl.
	/// </summary>
	partial class SearchUserControl
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
			this.dgSearch = new System.Windows.Forms.DataGrid();
			this.cmSearch = new System.Windows.Forms.ContextMenu();
			this.miClear = new System.Windows.Forms.MenuItem();
			this.btnSearchOptions = new System.Windows.Forms.Button();
			this.btnSearch = new System.Windows.Forms.Button();
			this.tbSearch = new System.Windows.Forms.TextBox();
			this.cbType = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dgSearch)).BeginInit();
			this.SuspendLayout();
			// 
			// dgSearch
			// 
			this.dgSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgSearch.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dgSearch.CaptionVisible = false;
			this.dgSearch.CausesValidation = false;
			this.dgSearch.ContextMenu = this.cmSearch;
			this.dgSearch.DataMember = "";
			this.dgSearch.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgSearch.Location = new System.Drawing.Point(0, 72);
			this.dgSearch.Name = "dgSearch";
			this.dgSearch.PreferredColumnWidth = 100;
			this.dgSearch.ReadOnly = true;
			this.dgSearch.Size = new System.Drawing.Size(208, 320);
			this.dgSearch.TabIndex = 8;
			this.dgSearch.Click += new System.EventHandler(this.dgSearch_Click);
			this.dgSearch.DoubleClick += new System.EventHandler(this.dgSearch_DoubleClick);
			this.dgSearch.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dgSearch_Navigate);
			// 
			// cmSearch
			// 
			this.cmSearch.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.miClear});
			// 
			// miClear
			// 
			this.miClear.Index = 0;
			this.miClear.Text = "_clear";
			this.miClear.Click += new System.EventHandler(this.miClear_Click);
			// 
			// btnSearchOptions
			// 
			this.btnSearchOptions.Location = new System.Drawing.Point(88, 48);
			this.btnSearchOptions.Name = "btnSearchOptions";
			this.btnSearchOptions.TabIndex = 7;
			this.btnSearchOptions.Text = "Options";
			this.btnSearchOptions.Visible = false;
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(0, 48);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(72, 23);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// tbSearch
			// 
			this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbSearch.Location = new System.Drawing.Point(0, 24);
			this.tbSearch.Name = "tbSearch";
			this.tbSearch.Size = new System.Drawing.Size(208, 20);
			this.tbSearch.TabIndex = 5;
			this.tbSearch.Text = "";
			this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
			// 
			// cbType
			// 
			this.cbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbType.Location = new System.Drawing.Point(0, 0);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(208, 21);
			this.cbType.TabIndex = 9;
			// 
			// SearchUserControl
			// 
			this.Controls.Add(this.cbType);
			this.Controls.Add(this.dgSearch);
			this.Controls.Add(this.btnSearchOptions);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.tbSearch);
			this.Name = "SearchUserControl";
			this.Size = new System.Drawing.Size(208, 392);
			this.Load += new System.EventHandler(this.SearchUserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgSearch)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.DataGrid dgSearch;
		private System.Windows.Forms.Button btnSearchOptions;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.TextBox tbSearch;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.ContextMenu cmSearch;
		private System.Windows.Forms.MenuItem miClear;
	}
}