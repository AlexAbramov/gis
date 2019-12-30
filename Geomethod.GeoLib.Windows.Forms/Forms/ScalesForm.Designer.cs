using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for ScalesForm.
	/// </summary>
	public partial class ScalesForm
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
			this.textBox = new System.Windows.Forms.TextBox();
			this.listBox = new System.Windows.Forms.ListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.errorProvider = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Location = new System.Drawing.Point(16, 16);
			this.textBox.MaxLength = 6;
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(136, 20);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "";
			this.textBox.Validating += new System.ComponentModel.CancelEventHandler(this.textBox_Validating);
			// 
			// listBox
			// 
			this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox.Location = new System.Drawing.Point(16, 56);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(136, 160);
			this.listBox.TabIndex = 1;
			this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
			// 
			// addButton
			// 
			this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addButton.Enabled = false;
			this.addButton.Location = new System.Drawing.Point(160, 56);
			this.addButton.Name = "addButton";
			this.addButton.TabIndex = 2;
			this.addButton.Text = "_add";
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Enabled = false;
			this.btnRemove.Location = new System.Drawing.Point(160, 88);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.TabIndex = 3;
			this.btnRemove.Text = "_remove";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(16, 228);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 12;
			this.btnOk.Text = "_ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(120, 228);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 13;
			this.cancelButton.Text = "_cancel";
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			this.errorProvider.DataMember = "";
			// 
			// ScalesForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(242, 266);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.textBox);
			this.Name = "ScalesForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "_scales";
			this.Load += new System.EventHandler(this.ScalesForm_Load);
			this.ResumeLayout(false);

		}
		#endregion
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.ErrorProvider errorProvider;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
	}
}