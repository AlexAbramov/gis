using System;
using System.Collections.Generic;
using System.Text;

namespace WinMap.Forms
{
	partial class SplashForm
	{
		private System.Windows.Forms.Label lblProgName;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SplashForm));
			this.lblProgName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblProgName
			// 
			this.lblProgName.BackColor = System.Drawing.Color.Transparent;
			this.lblProgName.CausesValidation = false;
			this.lblProgName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblProgName.ForeColor = System.Drawing.Color.White;
			this.lblProgName.Location = new System.Drawing.Point(8, 176);
			this.lblProgName.Name = "lblProgName";
			this.lblProgName.Size = new System.Drawing.Size(296, 24);
			this.lblProgName.TabIndex = 1;
			this.lblProgName.Text = "WinMap";
			this.lblProgName.Visible = false;
			// 
			// SplashForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(312, 206);
			this.ControlBox = false;
			this.Controls.Add(this.lblProgName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SplashForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SplashForm";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.SplashForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.SplashForm_Paint);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
