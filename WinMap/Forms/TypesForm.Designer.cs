namespace WinMap.Forms
{
	partial class TypesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TypesForm));
            this.ucTypes = new Geomethod.GeoLib.Windows.Forms.TypesUserControl();
            this.SuspendLayout();
            // 
            // ucTypes
            // 
            this.ucTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTypes.Location = new System.Drawing.Point(0, 0);
            this.ucTypes.Name = "ucTypes";
            this.ucTypes.Size = new System.Drawing.Size(355, 382);
            this.ucTypes.TabIndex = 1;
            this.ucTypes.OnCompositeSelected += new System.EventHandler(this.ucTypes_OnCompositeSelected);
            // 
            // TypesForm
            // 
            this.ClientSize = new System.Drawing.Size(355, 382);
            this.Controls.Add(this.ucTypes);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TypesForm";
            this.TabText = "_types";
            this.Text = "_types";
            this.Load += new System.EventHandler(this.TypesForm_Load);
            this.ResumeLayout(false);

		}

		#endregion

		internal Geomethod.GeoLib.Windows.Forms.TypesUserControl ucTypes;
	}
}
