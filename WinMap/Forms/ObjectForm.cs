using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using GeoLib;
using CommonLib;

namespace WinMap
{
	/// <summary>
	/// Summary description for ObjectForm.
	/// </summary>
	public class ObjectForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox tbStyle;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox captionTextBox;
		private System.Windows.Forms.Label captionLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		App app;
		GObject gobj;
		private System.Windows.Forms.TextBox typeTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button typeButton;

		public ObjectForm(App app,GObject gobj)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.app=app;
			this.gobj=gobj;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ObjectForm));
			this.btnOk = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.tbStyle = new System.Windows.Forms.TextBox();
			this.tbName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.captionTextBox = new System.Windows.Forms.TextBox();
			this.captionLabel = new System.Windows.Forms.Label();
			this.typeTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.typeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(136, 186);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 22;
			this.btnOk.Text = "_ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(240, 186);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 23;
			this.cancelButton.Text = "_cancel";
			// 
			// tbStyle
			// 
			this.tbStyle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbStyle.Location = new System.Drawing.Point(136, 96);
			this.tbStyle.Name = "tbStyle";
			this.tbStyle.Size = new System.Drawing.Size(352, 20);
			this.tbStyle.TabIndex = 16;
			this.tbStyle.Text = "";
			// 
			// tbName
			// 
			this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbName.Location = new System.Drawing.Point(136, 56);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(352, 20);
			this.tbName.TabIndex = 15;
			this.tbName.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 56);
			this.label2.Name = "label2";
			this.label2.TabIndex = 13;
			this.label2.Text = "_name:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 96);
			this.label1.Name = "label1";
			this.label1.TabIndex = 12;
			this.label1.Text = "_style:";
			// 
			// captionTextBox
			// 
			this.captionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.captionTextBox.Location = new System.Drawing.Point(136, 136);
			this.captionTextBox.Name = "captionTextBox";
			this.captionTextBox.Size = new System.Drawing.Size(352, 20);
			this.captionTextBox.TabIndex = 25;
			this.captionTextBox.Text = "";
			// 
			// captionLabel
			// 
			this.captionLabel.Location = new System.Drawing.Point(16, 136);
			this.captionLabel.Name = "captionLabel";
			this.captionLabel.TabIndex = 24;
			this.captionLabel.Text = "_caption:";
			// 
			// typeTextBox
			// 
			this.typeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.typeTextBox.Location = new System.Drawing.Point(136, 16);
			this.typeTextBox.Name = "typeTextBox";
			this.typeTextBox.ReadOnly = true;
			this.typeTextBox.Size = new System.Drawing.Size(312, 20);
			this.typeTextBox.TabIndex = 27;
			this.typeTextBox.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 16);
			this.label3.Name = "label3";
			this.label3.TabIndex = 26;
			this.label3.Text = "_type:";
			// 
			// typeButton
			// 
			this.typeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.typeButton.Location = new System.Drawing.Point(456, 16);
			this.typeButton.Name = "typeButton";
			this.typeButton.Size = new System.Drawing.Size(32, 23);
			this.typeButton.TabIndex = 28;
			this.typeButton.Text = "...";
			this.typeButton.Click += new System.EventHandler(this.typeButton_Click);
			// 
			// ObjectForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(498, 228);
			this.Controls.Add(this.typeButton);
			this.Controls.Add(this.typeTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.captionTextBox);
			this.Controls.Add(this.captionLabel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.tbStyle);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(1000, 262);
			this.MinimumSize = new System.Drawing.Size(506, 262);
			this.Name = "ObjectForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "_gobj";
			this.Load += new System.EventHandler(this.ObjectForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ObjectForm_Load(object sender, System.EventArgs e)
		{
			WinLib.Utils.Localize(this);
			tbName.MaxLength=MaxLength.Name;
			tbStyle.MaxLength=MaxLength.Style;
			if(gobj==null) return;
			typeTextBox.Text=gobj.Type.Name;
			tbName.Text=gobj.Name;
			tbStyle.Text=gobj.StyleStr;
			captionTextBox.Text=gobj.Caption;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if(gobj==null) return;
			if(!IsChanged()) return;
			try
			{
				gobj.Name=tbName.Text;
				gobj.StyleStr=tbStyle.Text;
				gobj.Caption=captionTextBox.Text;
				if(app.Lib.HasDb)	using(Context context=app.Lib.GetContext()) gobj.Save(context);
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}		
		}

		bool IsChanged()
		{
			if(tbName.Text!=gobj.Name) return true;
			if(captionTextBox.Text!=gobj.Caption) return true;
			if(tbStyle.Text!=gobj.StyleStr) return true;
			return false;
		}

		private void typeButton_Click(object sender, System.EventArgs e)
		{
			TypeForm typeForm=new TypeForm(app,gobj.Type);
			typeForm.ShowDialog(this);
		}
	}
}
