using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	partial class LayersUserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox lbLayers;
		private System.Windows.Forms.ContextMenu cmLayers;
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
            this.lbLayers = new System.Windows.Forms.ListBox( );
            this.cmLayers = new System.Windows.Forms.ContextMenu( );
            this.miAdd = new System.Windows.Forms.MenuItem( );
            this.miUpdate = new System.Windows.Forms.MenuItem( );
            this.miRename = new System.Windows.Forms.MenuItem( );
            this.miRemove = new System.Windows.Forms.MenuItem( );
            this.SuspendLayout( );
            // 
            // lbLayers
            // 
            this.lbLayers.ContextMenu = this.cmLayers;
            this.lbLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLayers.Location = new System.Drawing.Point( 0, 0 );
            this.lbLayers.Name = "lbLayers";
            this.lbLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbLayers.Size = new System.Drawing.Size( 224, 433 );
            this.lbLayers.TabIndex = 1;
            this.lbLayers.SelectedIndexChanged += new System.EventHandler( this.lbLayers_SelectedIndexChanged );
            this.lbLayers.SelectedValueChanged += new System.EventHandler( this.lbLayers_SelectedValueChanged );
            this.lbLayers.KeyDown += new System.Windows.Forms.KeyEventHandler( this.lbLayers_KeyDown );
            // 
            // cmLayers
            // 
            this.cmLayers.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.miAdd,
            this.miUpdate,
            this.miRename,
            this.miRemove} );
            this.cmLayers.Popup += new System.EventHandler( this.cmLayers_Popup );
            // 
            // miAdd
            // 
            this.miAdd.Index = 0;
            this.miAdd.Text = "_add";
            this.miAdd.Click += new System.EventHandler( this.miAdd_Click );
            // 
            // miUpdate
            // 
            this.miUpdate.Index = 1;
            this.miUpdate.Text = "_update";
            this.miUpdate.Click += new System.EventHandler( this.miUpdate_Click );
            // 
            // miRename
            // 
            this.miRename.Index = 2;
            this.miRename.Text = "_rename";
            this.miRename.Click += new System.EventHandler( this.miRename_Click );
            // 
            // miRemove
            // 
            this.miRemove.Index = 3;
            this.miRemove.Text = "_remove";
            this.miRemove.Click += new System.EventHandler( this.miRemove_Click );
            // 
            // LayersUserControl
            // 
            this.Controls.Add( this.lbLayers );
            this.Name = "LayersUserControl";
            this.Size = new System.Drawing.Size( 224, 440 );
            this.Load += new System.EventHandler( this.LayersUserControl_Load );
            this.ResumeLayout( false );

		}
		#endregion
	}
}