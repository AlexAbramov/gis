namespace WinMap.Forms
{
	partial class ViewsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewsForm));
            this.ucViews = new Geomethod.GeoLib.Windows.Forms.ViewsUserControl();
            this.SuspendLayout();
            // 
            // ucViews
            // 
            this.ucViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucViews.Location = new System.Drawing.Point(0, 0);
            this.ucViews.Name = "ucViews";
            this.ucViews.Size = new System.Drawing.Size(284, 264);
            this.ucViews.TabIndex = 1;
            this.ucViews.OnViewSelected += new System.EventHandler(this.ucViews_OnViewSelected);
            // 
            // ViewsForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.ucViews);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ViewsForm";
            this.TabText = "_views";
            this.Text = "_views";
            this.Load += new System.EventHandler(this.ViewsForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		internal Geomethod.GeoLib.Windows.Forms.ViewsUserControl ucViews;
	}
}
