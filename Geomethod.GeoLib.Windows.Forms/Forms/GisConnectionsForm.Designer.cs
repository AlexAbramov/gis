using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for GisConnectionsForm.
	/// </summary>
	public partial class GisConnectionsForm
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
            this.components = new System.ComponentModel.Container();
            this.btnEdit = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.cmConnnections = new System.Windows.Forms.ContextMenu();
            this.miOpen = new System.Windows.Forms.MenuItem();
            this.miEdit = new System.Windows.Forms.MenuItem();
            this.miAdd = new System.Windows.Forms.MenuItem();
            this.miRemove = new System.Windows.Forms.MenuItem();
            this.gridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.providerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectionStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.optionsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet = new Geomethod.GeoLib.Windows.Forms.DataSet();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(552, 41);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "_edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(552, 70);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(100, 23);
            this.addButton.TabIndex = 5;
            this.addButton.Text = "_add";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(552, 99);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 23);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = "_remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(552, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "_open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(552, 281);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "_close";
            // 
            // cmConnnections
            // 
            this.cmConnnections.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miOpen,
            this.miEdit,
            this.miAdd,
            this.miRemove});
            // 
            // miOpen
            // 
            this.miOpen.Index = 0;
            this.miOpen.Text = "_open";
            this.miOpen.Click += new System.EventHandler(this.miOpen_Click);
            // 
            // miEdit
            // 
            this.miEdit.Index = 1;
            this.miEdit.Text = "_edit";
            this.miEdit.Click += new System.EventHandler(this.miEdit_Click);
            // 
            // miAdd
            // 
            this.miAdd.Index = 2;
            this.miAdd.Text = "_add";
            this.miAdd.Click += new System.EventHandler(this.miAdd_Click);
            // 
            // miRemove
            // 
            this.miRemove.Index = 3;
            this.miRemove.Text = "_remove";
            this.miRemove.Click += new System.EventHandler(this.miRemove_Click);
            // 
            // gridView
            // 
            this.gridView.AllowUserToAddRows = false;
            this.gridView.AllowUserToDeleteRows = false;
            this.gridView.AllowUserToResizeRows = false;
            this.gridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridView.AutoGenerateColumns = false;
            this.gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.providerNameDataGridViewTextBoxColumn,
            this.connectionStringDataGridViewTextBoxColumn,
            this.optionsDataGridViewTextBoxColumn});
            this.gridView.DataSource = this.bindingSource;
            this.gridView.Location = new System.Drawing.Point(12, 12);
            this.gridView.MultiSelect = false;
            this.gridView.Name = "gridView";
            this.gridView.ReadOnly = true;
            this.gridView.RowHeadersVisible = false;
            this.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView.Size = new System.Drawing.Size(534, 292);
            this.gridView.TabIndex = 7;
            this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "_Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // providerNameDataGridViewTextBoxColumn
            // 
            this.providerNameDataGridViewTextBoxColumn.DataPropertyName = "ProviderName";
            this.providerNameDataGridViewTextBoxColumn.HeaderText = "_db";
            this.providerNameDataGridViewTextBoxColumn.Name = "providerNameDataGridViewTextBoxColumn";
            this.providerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // connectionStringDataGridViewTextBoxColumn
            // 
            this.connectionStringDataGridViewTextBoxColumn.DataPropertyName = "ConnectionString";
            this.connectionStringDataGridViewTextBoxColumn.FillWeight = 200F;
            this.connectionStringDataGridViewTextBoxColumn.HeaderText = "_ConnStr";
            this.connectionStringDataGridViewTextBoxColumn.Name = "connectionStringDataGridViewTextBoxColumn";
            this.connectionStringDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // optionsDataGridViewTextBoxColumn
            // 
            this.optionsDataGridViewTextBoxColumn.DataPropertyName = "Options";
            this.optionsDataGridViewTextBoxColumn.HeaderText = "_Options";
            this.optionsDataGridViewTextBoxColumn.Name = "optionsDataGridViewTextBoxColumn";
            this.optionsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bindingSource
            // 
            this.bindingSource.DataMember = "GisConnections";
            this.bindingSource.DataSource = this.dataSet;
            this.bindingSource.CurrentChanged += new System.EventHandler(this.bindingSource_CurrentChanged);
            // 
            // dataSet
            // 
            this.dataSet.DataSetName = "DataSet";
            this.dataSet.Locale = new System.Globalization.CultureInfo("");
            this.dataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GisConnectionsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(664, 316);
            this.Controls.Add(this.gridView);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnOk);
            this.Name = "GisConnectionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "_connections";
            this.Load += new System.EventHandler(this.GisConnectionsForm_Load);
            this.Closed += new System.EventHandler(this.GisConnectionsForm_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ContextMenu cmConnnections;
		private System.Windows.Forms.MenuItem miOpen;
		private System.Windows.Forms.MenuItem miEdit;
		private System.Windows.Forms.MenuItem miAdd;
        private System.Windows.Forms.MenuItem miRemove;
        private DataGridView gridView;
        private IContainer components;
        private BindingSource bindingSource;
        private DataSet dataSet;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn providerNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn connectionStringDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn optionsDataGridViewTextBoxColumn;
	}
}