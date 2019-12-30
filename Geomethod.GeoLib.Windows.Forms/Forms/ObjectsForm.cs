using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using GeoLib;

namespace WinMap
{
	/// <summary>
	/// Summary description for ObjectsForm.
	/// </summary>
	public class ObjectsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.ColumnHeader nameColumnHeader;
		private System.Windows.Forms.ColumnHeader typeColumnHeader;
		private System.Windows.Forms.Button btnOk;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		App app;
		private System.Windows.Forms.Button cancelButton;
		ArrayList objects;

		public ObjectsForm(App app,ArrayList objects)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.app=app;
			this.objects=objects;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ObjectsForm));
			this.listView = new System.Windows.Forms.ListView();
			this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.typeColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.btnOk = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.AllowColumnReorder = true;
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																							 this.nameColumnHeader,
																																							 this.typeColumnHeader});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.LabelWrap = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(408, 272);
			this.listView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listView.TabIndex = 4;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.DoubleClick += new System.EventHandler(this.listView_DoubleClick);
			// 
			// nameColumnHeader
			// 
			this.nameColumnHeader.Text = "_name";
			this.nameColumnHeader.Width = 200;
			// 
			// typeColumnHeader
			// 
			this.typeColumnHeader.Text = "_type";
			this.typeColumnHeader.Width = 200;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(112, 288);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "_ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.CausesValidation = false;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(216, 288);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 24;
			this.cancelButton.Text = "_cancel";
			// 
			// ObjectsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 322);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.listView);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(416, 356);
			this.Name = "ObjectsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "_gobjects";
			this.Load += new System.EventHandler(this.ObjectsForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			OnOk();		
		}

		private void ObjectsForm_Load(object sender, System.EventArgs e)
		{
			WinLib.Utils.Localize(this);
			foreach(GObject gobj in objects)
			{
//				string connStr=ht[name] as string;
				string[] subitems={gobj.Name,gobj.Type.Name};
				ListViewItem item=new ListViewItem(subitems);
				item.Tag=gobj;
				listView.Items.Add(item);
			}
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
		}

		void OnOk()
		{
			ListViewItem item=SelectedItem;
			if(item==null) return;
			GObject gobj=item.Tag as GObject;
			if(gobj==null) return;
			ObjectForm objectForm=new ObjectForm(app,gobj);
			objectForm.ShowDialog(this);
		}

		ListViewItem SelectedItem
		{
			get
			{
				if(listView.SelectedItems.Count==0) return null;
				return listView.SelectedItems[0];
			}
		}
	}
}
