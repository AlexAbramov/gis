namespace Geomethod.GeoLib.Windows.Forms.Forms
{
	partial class ColorFromImageForm
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
			this.btnLoadImage = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.lblImagePath = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// btnLoadImage
			// 
			this.btnLoadImage.AutoSize = true;
			this.btnLoadImage.Location = new System.Drawing.Point(12, 12);
			this.btnLoadImage.Name = "btnLoadImage";
			this.btnLoadImage.Size = new System.Drawing.Size(100, 23);
			this.btnLoadImage.TabIndex = 11;
			this.btnLoadImage.Text = "Изображение...";
			this.btnLoadImage.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.pictureBox);
			this.panel1.Location = new System.Drawing.Point(12, 41);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(448, 375);
			this.panel1.TabIndex = 13;
			// 
			// pictureBox
			// 
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(100, 100);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox.TabIndex = 9;
			this.pictureBox.TabStop = false;
			// 
			// lblImagePath
			// 
			this.lblImagePath.AutoSize = true;
			this.lblImagePath.Location = new System.Drawing.Point(118, 17);
			this.lblImagePath.Name = "lblImagePath";
			this.lblImagePath.Size = new System.Drawing.Size(68, 13);
			this.lblImagePath.TabIndex = 17;
			this.lblImagePath.Text = "lblImagePath";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(385, 422);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 26;
			this.btnCancel.Text = "_cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(304, 422);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 25;
			this.btnOk.Text = "_ok";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.Filter = "Графические файлы (*.bmp; *.gif; *.jpg; *.png)|*.bmp; *.gif; *.jpg; *.png; |Все ф" +
					"айлы|*.*";
			// 
			// ColorFromImageForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(472, 457);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnLoadImage);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblImagePath);
			this.Name = "ColorFromImageForm";
			this.Text = "ColorFromImageForm";
			this.Load += new System.EventHandler(this.ColorFromImageForm_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnLoadImage;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Label lblImagePath;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
	}
}