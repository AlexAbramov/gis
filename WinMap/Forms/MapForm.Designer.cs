using System;
using System.Collections.Generic;
using System.Text;

namespace WinMap.Forms
{
	partial class MapForm
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.ucMap = new Geomethod.GeoLib.Windows.Forms.MapUserControl();
            this.SuspendLayout();
            // 
            // ucMap
            // 
            this.ucMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMap.Location = new System.Drawing.Point(0, 0);
            this.ucMap.MinimumSize = new System.Drawing.Size(1, 1);
            this.ucMap.Name = "ucMap";
            this.ucMap.Size = new System.Drawing.Size(392, 366);
            this.ucMap.TabIndex = 0;
            // 
            // MapForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(392, 366);
            this.Controls.Add(this.ucMap);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Name = "MapForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.MapForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapForm_FormClosed);
            this.ResumeLayout(false);

		}
		#endregion
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		internal Geomethod.GeoLib.Windows.Forms.MapUserControl ucMap;
	}
}
