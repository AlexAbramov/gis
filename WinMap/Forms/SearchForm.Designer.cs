namespace WinMap.Forms
{
	partial class SearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.ucSearch = new Geomethod.GeoLib.Windows.Forms.SearchUserControl();
            this.SuspendLayout();
            // 
            // ucSearch
            // 
            this.ucSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearch.Location = new System.Drawing.Point(0, 0);
            this.ucSearch.Name = "ucSearch";
            this.ucSearch.Size = new System.Drawing.Size(284, 264);
            this.ucSearch.TabIndex = 0;
            // 
            // SearchForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.ucSearch);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchForm";
            this.TabText = "_search";
            this.Text = "_search";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		internal Geomethod.GeoLib.Windows.Forms.SearchUserControl ucSearch;

	}
}
