namespace WinMap.Forms
{
	partial class BackgroundForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackgroundForm));
            this.ucBg = new Geomethod.GeoLib.Windows.Forms.BgUserControl();
            this.SuspendLayout();
            // 
            // ucBg
            // 
            this.ucBg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBg.Location = new System.Drawing.Point(0, 0);
            this.ucBg.Name = "ucBg";
            this.ucBg.Size = new System.Drawing.Size(284, 264);
            this.ucBg.TabIndex = 1;
            // 
            // BackgroundForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.ucBg);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BackgroundForm";
            this.TabText = "_background";
            this.Text = "_background";
            this.Load += new System.EventHandler(this.BackgroundForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		internal Geomethod.GeoLib.Windows.Forms.BgUserControl ucBg;
	}
}
