namespace WinMap.Forms
{
	partial class PropertiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesForm));
            this.ucProperties = new Geomethod.GeoLib.Windows.Forms.PropertiesUserControl();
            this.SuspendLayout();
            // 
            // ucProperties
            // 
            this.ucProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucProperties.Location = new System.Drawing.Point(0, 0);
            this.ucProperties.Name = "ucProperties";
            this.ucProperties.SelectedObject = null;
            this.ucProperties.Size = new System.Drawing.Size(393, 436);
            this.ucProperties.TabIndex = 1;
            // 
            // PropertiesForm
            // 
            this.ClientSize = new System.Drawing.Size(393, 436);
            this.Controls.Add(this.ucProperties);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PropertiesForm";
            this.TabText = "_properties";
            this.Load += new System.EventHandler(this.PropertiesForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		internal Geomethod.GeoLib.Windows.Forms.PropertiesUserControl ucProperties;

	}
}
