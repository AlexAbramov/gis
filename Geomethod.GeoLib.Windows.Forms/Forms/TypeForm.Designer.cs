using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for TypeForm.
	/// </summary>
	public partial class TypeForm
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
			this.btnOk = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.errorProvider = new System.Windows.Forms.ErrorProvider();
			this.lvTypes = new System.Windows.Forms.ListView();
			this.chName = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(128, 232);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 10;
			this.btnOk.Text = "_ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(232, 232);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "_cancel";
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			this.errorProvider.DataMember = "";
			// 
			// lvTypes
			// 
			this.lvTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lvTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																							this.chName});
			this.lvTypes.FullRowSelect = true;
			this.lvTypes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvTypes.HideSelection = false;
			this.lvTypes.LabelWrap = false;
			this.lvTypes.Location = new System.Drawing.Point(8, 8);
			this.lvTypes.MultiSelect = false;
			this.lvTypes.Name = "lvTypes";
			this.lvTypes.Size = new System.Drawing.Size(424, 208);
			this.lvTypes.TabIndex = 12;
			this.lvTypes.View = System.Windows.Forms.View.Details;
			this.lvTypes.Resize += new System.EventHandler(this.lvTypes_Resize);
			this.lvTypes.DoubleClick += new System.EventHandler(this.lvTypes_DoubleClick);
			this.lvTypes.SelectedIndexChanged += new System.EventHandler(this.lvTypes_SelectedIndexChanged);
			// 
			// chName
			// 
			this.chName.Text = "";
			// 
			// TypeForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 268);
			this.Controls.Add(this.lvTypes);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.cancelButton);
			this.Name = "TypeForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "_gtype";
			this.Load += new System.EventHandler(this.TypeForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.ListView lvTypes;
		private System.Windows.Forms.ColumnHeader chName;
	}
}