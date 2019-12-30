namespace WinMap.Forms
{
	partial class LayersForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayersForm));
            this.ucLayers = new Geomethod.GeoLib.Windows.Forms.LayersUserControl();
            this.SuspendLayout();
            // 
            // ucLayers
            // 
            this.ucLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucLayers.Location = new System.Drawing.Point(0, 0);
            this.ucLayers.Name = "ucLayers";
            this.ucLayers.Size = new System.Drawing.Size(284, 264);
            this.ucLayers.TabIndex = 1;
            // 
            // LayersForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.ucLayers);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LayersForm";
            this.TabText = "_layers";
            this.Text = "_layers";
            this.Load += new System.EventHandler(this.LayersForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		internal Geomethod.GeoLib.Windows.Forms.LayersUserControl ucLayers;
	}
}
