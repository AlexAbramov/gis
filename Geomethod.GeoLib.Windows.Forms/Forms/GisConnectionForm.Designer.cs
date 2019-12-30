using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for GisConnectionForm.
	/// </summary>
	partial class ConnectionForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbConnStr = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblConnString = new System.Windows.Forms.Label();
            this.lblOptions = new System.Windows.Forms.Label();
            this.tbOptions = new System.Windows.Forms.TextBox();
            this.lblDataProvider = new System.Windows.Forms.Label();
            this.cbDataProvider = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(126, 124);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "_ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(207, 124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "_cancel";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.Location = new System.Drawing.Point(128, 12);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(297, 20);
            this.tbName.TabIndex = 5;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbConnStr
            // 
            this.tbConnStr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConnStr.Location = new System.Drawing.Point(126, 65);
            this.tbConnStr.Name = "tbConnStr";
            this.tbConnStr.Size = new System.Drawing.Size(299, 20);
            this.tbConnStr.TabIndex = 6;
            this.tbConnStr.TextChanged += new System.EventHandler(this.tbConnStr_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(42, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "_name:";
            // 
            // lblConnString
            // 
            this.lblConnString.AutoSize = true;
            this.lblConnString.Location = new System.Drawing.Point(12, 68);
            this.lblConnString.Name = "lblConnString";
            this.lblConnString.Size = new System.Drawing.Size(51, 13);
            this.lblConnString.TabIndex = 0;
            this.lblConnString.Text = "_connstr:";
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.Location = new System.Drawing.Point(12, 94);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(50, 13);
            this.lblOptions.TabIndex = 11;
            this.lblOptions.Text = "_options:";
            // 
            // tbOptions
            // 
            this.tbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOptions.Location = new System.Drawing.Point(126, 91);
            this.tbOptions.Name = "tbOptions";
            this.tbOptions.Size = new System.Drawing.Size(299, 20);
            this.tbOptions.TabIndex = 12;
            // 
            // lblDataProvider
            // 
            this.lblDataProvider.AutoSize = true;
            this.lblDataProvider.Location = new System.Drawing.Point(12, 41);
            this.lblDataProvider.Name = "lblDataProvider";
            this.lblDataProvider.Size = new System.Drawing.Size(28, 13);
            this.lblDataProvider.TabIndex = 13;
            this.lblDataProvider.Text = "_db:";
            // 
            // cbDataProvider
            // 
            this.cbDataProvider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDataProvider.ItemHeight = 13;
            this.cbDataProvider.Location = new System.Drawing.Point(128, 38);
            this.cbDataProvider.Name = "cbDataProvider";
            this.cbDataProvider.Size = new System.Drawing.Size(154, 21);
            this.cbDataProvider.TabIndex = 14;
            // 
            // ConnectionForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(437, 159);
            this.Controls.Add(this.cbDataProvider);
            this.Controls.Add(this.lblDataProvider);
            this.Controls.Add(this.tbOptions);
            this.Controls.Add(this.tbConnStr);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblOptions);
            this.Controls.Add(this.lblConnString);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "ConnectionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_conn";
            this.Load += new System.EventHandler(this.GisConnectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.TextBox tbConnStr;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblConnString;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblOptions;
		private System.Windows.Forms.Label lblDataProvider;
		private System.Windows.Forms.ComboBox cbDataProvider;
		private System.Windows.Forms.TextBox tbOptions;
	}
}