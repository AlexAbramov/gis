using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using GeoLib;

namespace WinMap
{
	/// <summary>
	/// Summary description for LibForm.
	/// </summary>
	public class LibForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.TextBox tbStyle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbSmin;
		private System.Windows.Forms.TextBox tbSmax;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button cancelButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox leftTextBox;
		private System.Windows.Forms.TextBox topTextBox;
		private System.Windows.Forms.TextBox rightTextBox;
		private System.Windows.Forms.TextBox bottomTextBox;
		private System.Windows.Forms.ErrorProvider errorProvider;
		App app;
		private System.Windows.Forms.Label lblName;
		GLib lib;

		public LibForm(App app,GLib lib)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.app=app;
			this.lib=lib;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LibForm));
			this.label1 = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbName = new System.Windows.Forms.TextBox();
			this.tbStyle = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbSmin = new System.Windows.Forms.TextBox();
			this.tbSmax = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.leftTextBox = new System.Windows.Forms.TextBox();
			this.topTextBox = new System.Windows.Forms.TextBox();
			this.rightTextBox = new System.Windows.Forms.TextBox();
			this.bottomTextBox = new System.Windows.Forms.TextBox();
			this.errorProvider = new System.Windows.Forms.ErrorProvider();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 56);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "_style:";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(24, 24);
			this.lblName.Name = "lblName";
			this.lblName.TabIndex = 1;
			this.lblName.Text = "_name:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 88);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Tag = "";
			this.label3.Text = "_scalemin:";
			// 
			// tbName
			// 
			this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbName.Location = new System.Drawing.Point(144, 24);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(280, 20);
			this.tbName.TabIndex = 3;
			this.tbName.Text = "";
			// 
			// tbStyle
			// 
			this.tbStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbStyle.Location = new System.Drawing.Point(144, 56);
			this.tbStyle.Name = "tbStyle";
			this.tbStyle.Size = new System.Drawing.Size(280, 20);
			this.tbStyle.TabIndex = 4;
			this.tbStyle.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 120);
			this.label4.Name = "label4";
			this.label4.TabIndex = 5;
			this.label4.Text = "_scalemax:";
			// 
			// tbSmin
			// 
			this.tbSmin.Location = new System.Drawing.Point(144, 88);
			this.tbSmin.MaxLength = 9;
			this.tbSmin.Name = "tbSmin";
			this.tbSmin.TabIndex = 6;
			this.tbSmin.Text = "";
			this.tbSmin.Validating += new System.ComponentModel.CancelEventHandler(this.tbSmin_Validating);
			// 
			// tbSmax
			// 
			this.tbSmax.Location = new System.Drawing.Point(144, 120);
			this.tbSmax.MaxLength = 9;
			this.tbSmax.Name = "tbSmax";
			this.tbSmax.TabIndex = 7;
			this.tbSmax.Text = "";
			this.tbSmax.Validating += new System.ComponentModel.CancelEventHandler(this.tbSmax_Validating);
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(112, 296);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 10;
			this.btnOk.Text = "_ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(216, 296);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "_cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(24, 152);
			this.label5.Name = "label5";
			this.label5.TabIndex = 12;
			this.label5.Text = "_left:";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 184);
			this.label6.Name = "label6";
			this.label6.TabIndex = 13;
			this.label6.Text = "_top:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 216);
			this.label7.Name = "label7";
			this.label7.TabIndex = 14;
			this.label7.Text = "_right:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(24, 248);
			this.label8.Name = "label8";
			this.label8.TabIndex = 15;
			this.label8.Text = "_bottom:";
			// 
			// leftTextBox
			// 
			this.leftTextBox.Location = new System.Drawing.Point(144, 152);
			this.leftTextBox.MaxLength = 9;
			this.leftTextBox.Name = "leftTextBox";
			this.leftTextBox.TabIndex = 16;
			this.leftTextBox.Text = "";
			this.leftTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.leftTextBox_Validating);
			// 
			// topTextBox
			// 
			this.topTextBox.Location = new System.Drawing.Point(144, 184);
			this.topTextBox.MaxLength = 9;
			this.topTextBox.Name = "topTextBox";
			this.topTextBox.TabIndex = 17;
			this.topTextBox.Text = "";
			this.topTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.topTextBox_Validating);
			// 
			// rightTextBox
			// 
			this.rightTextBox.Location = new System.Drawing.Point(144, 216);
			this.rightTextBox.MaxLength = 9;
			this.rightTextBox.Name = "rightTextBox";
			this.rightTextBox.TabIndex = 18;
			this.rightTextBox.Text = "";
			this.rightTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.rightTextBox_Validating);
			// 
			// bottomTextBox
			// 
			this.bottomTextBox.Location = new System.Drawing.Point(144, 248);
			this.bottomTextBox.MaxLength = 9;
			this.bottomTextBox.Name = "bottomTextBox";
			this.bottomTextBox.TabIndex = 19;
			this.bottomTextBox.Text = "";
			this.bottomTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.bottomTextBox_Validating);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// LibForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 338);
			this.Controls.Add(this.bottomTextBox);
			this.Controls.Add(this.rightTextBox);
			this.Controls.Add(this.topTextBox);
			this.Controls.Add(this.leftTextBox);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.tbSmax);
			this.Controls.Add(this.tbSmin);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbStyle);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1000, 372);
			this.MinimumSize = new System.Drawing.Size(448, 372);
			this.Name = "LibForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "_glib";
			this.Load += new System.EventHandler(this.LibForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		void CheckLocale()
		{
		}

		private void LibForm_Load(object sender, System.EventArgs e)
		{
			WinLib.Utils.Localize(this);
			tbName.MaxLength=MaxLength.Name;
			tbStyle.MaxLength=MaxLength.Style;
			if(lib==null) return;
			tbName.Text=lib.Name;
			tbStyle.Text=lib.StyleStr;
			tbSmin.Text=lib.SMin.ToString();
			tbSmax.Text=lib.SMax.ToString();
			Rect bounds=lib.Bounds;
			leftTextBox.Text=bounds.left.ToString();
			topTextBox.Text=bounds.top.ToString();
			rightTextBox.Text=bounds.right.ToString();
			bottomTextBox.Text=bounds.bottom.ToString();
		}

		void CheckScale(TextBox tb,System.ComponentModel.CancelEventArgs e)
		{
			string msg="";
			try
			{
				int scale=int.Parse(tb.Text);
				if(scale<0) msg="Scale shouldn't be negative.";
			}
			catch
			{
				msg="Wrong integer format.";
			}
			if(msg.Length>0) e.Cancel=true;
			errorProvider.SetError(tb,msg);
		}

		void CheckCoord(TextBox tb,System.ComponentModel.CancelEventArgs e)
		{
			string msg="";
			try
			{
				int coord=int.Parse(tb.Text);
			}
			catch
			{
				msg="Wrong integer format.";
			}
			if(msg.Length>0) e.Cancel=true;
			errorProvider.SetError(tb,msg);
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
		
		}

		private void tbSmin_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckScale(tbSmin,e);
		}

		private void tbSmax_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckScale(tbSmax,e);
		}

		private void leftTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckCoord(leftTextBox,e);
		}

		private void topTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckCoord(topTextBox,e);
		}

		private void rightTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckCoord(rightTextBox,e);
		}

		private void bottomTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			CheckCoord(bottomTextBox,e);		
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if(!IsChanged()) return;
			try
			{
				Rect bounds;
				bounds.left=int.Parse(leftTextBox.Text);
				bounds.top=int.Parse(topTextBox.Text);
				bounds.right=int.Parse(rightTextBox.Text);
				bounds.bottom=int.Parse(bottomTextBox.Text);
				int smin=int.Parse(tbSmin.Text);
				int smax=int.Parse(tbSmax.Text);
				lib.StyleStr=tbStyle.Text;
				lib.SetBounds(bounds);
				lib.SMin=smin;
				lib.SMax=smax;
				lib.Name=tbName.Text;
				if(lib.HasDb) lib.Save(BatchLevel.Current);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		bool IsChanged()
		{
			if(leftTextBox.Text!=lib.Bounds.left.ToString()) return true;
			if(topTextBox.Text!=lib.Bounds.top.ToString()) return true;
			if(rightTextBox.Text!=lib.Bounds.right.ToString()) return true;
			if(bottomTextBox.Text!=lib.Bounds.bottom.ToString()) return true;
			if(tbSmin.Text!=lib.SMin.ToString()) return true;
			if(tbSmax.Text!=lib.SMax.ToString()) return true;
			if(tbStyle.Text!=lib.StyleStr) return true;
			if(tbName.Text!=lib.Name) return true;
			return false;
		}
	}
}
