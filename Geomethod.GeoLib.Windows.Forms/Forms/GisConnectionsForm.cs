using System;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;


namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for GisConnectionsForm.
	/// </summary>
	public partial class GisConnectionsForm : System.Windows.Forms.Form
	{
		GisConnections gisConnections;
		GisConnection selGisConnection;
		bool connectionsUpdated=false;
		public bool ConnectionsUpdated { get { return connectionsUpdated; } }       
        Dictionary<DataSet.GisConnectionsRow, GisConnection> rowBinding = new Dictionary<DataSet.GisConnectionsRow, GisConnection>();

        DataSet.GisConnectionsRow SelectedRow
        {
            get
            {
                DataRowView drv=bindingSource.Current as DataRowView;                
                return drv!=null ? drv.Row as DataSet.GisConnectionsRow : null;
            }
        }

        public GisConnection SelectedConnection
        {
            get
            {
                return GetGisConnection(SelectedRow);
            }
            set
            {
                int index=0;
                foreach(GisConnection conn in gisConnections)
                {
                    if(conn==value)
                    { 
                        bindingSource.Position = index;
                        break; 
                    }
                    index++;
                }
            }
        }

        private GisConnection GetGisConnection(DataSet.GisConnectionsRow row)
        {
            if (row != null && rowBinding.ContainsKey(row))
            {
                return rowBinding[row];
            }
            return null;
        }

	
		public GisConnectionsForm(GisConnections gisConnections, GisConnection selGisConnection)
		{
			InitializeComponent();
			this.gisConnections=gisConnections;
			this.selGisConnection=selGisConnection;
		}


		private void GisConnectionsForm_Load(object sender, System.EventArgs e)
		{
			LocaleUtils.Localize(this);
            LoadData();
			MinimumSize=Size;
			UpdateControls();
		}

        private void LoadData()
        {
            foreach (GisConnection gisConnection in gisConnections)
            {
                AddRow(gisConnection);
            }
            if (selGisConnection != null) SelectedConnection = selGisConnection;
        }

        DataSet.GisConnectionsRow AddRow(GisConnection gisConnection)
        {
            DataSet.GisConnectionsRow row = dataSet.GisConnections.NewGisConnectionsRow();
            FillRow(row, gisConnection);
            dataSet.GisConnections.Rows.Add(row);
            rowBinding.Add(row, gisConnection);
            return row;
        }

        private void FillRow(DataSet.GisConnectionsRow row, GisConnection gisConnection)
        {
            row.ConnectionString = gisConnection.ConnectionString;
            row.Name = gisConnection.Name;
            row.ProviderName = gisConnection.ProviderName;
            row.Options = gisConnection.Options;
        }

		private void addButton_Click(object sender, System.EventArgs e)
		{
			Add();
		}

		void Add()
		{
			ConnectionForm form=new ConnectionForm(gisConnections,null);
			if(form.ShowDialog(this)==DialogResult.OK)
			{
				GisConnection conn=form.GisConnection;
                gisConnections.Add(conn);
                AddRow(conn);
                SelectedConnection = conn;
                connectionsUpdated = true;
			}
            gridView.Focus();
        }

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			Edit();
		}

		void Edit()
		{
            DataSet.GisConnectionsRow row = SelectedRow;
			GisConnection conn=GetGisConnection(row);

            if (row != null && conn!=null)
            {
                ConnectionForm gisConnectionForm = new ConnectionForm(gisConnections, conn);
                if (gisConnectionForm.ShowDialog(this) == DialogResult.OK)
                {
                    FillRow(row, conn);
                    connectionsUpdated = true;
                }
            }
            gridView.Focus();
		}

		private void btnOpen_Click(object sender, System.EventArgs e)
		{
			Open();
		}

		void Open()
		{
            if (SelectedConnection != null)
            {
                base.DialogResult = DialogResult.OK;
                Close();
            }
        }

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            UpdateControls();		
		}

		void UpdateControls()
		{
			bool selected=SelectedConnection!=null;
			btnRemove.Enabled=selected;
			miRemove.Enabled=selected;
			btnEdit.Enabled=selected;
			miEdit.Enabled=selected;
			btnOpen.Enabled=selected;
			miOpen.Enabled=selected;		
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			Remove();
		}

		void Remove()
		{
            DataSet.GisConnectionsRow row = SelectedRow;
			GisConnection conn=GetGisConnection(row);

            if (row != null && conn!=null)
            {
                bindingSource.RemoveCurrent();
                rowBinding.Remove(row);
                gisConnections.Remove(conn);
                connectionsUpdated = true;
                UpdateControls();
            }
            gridView.Focus();
		}

		private void listView_DoubleClick(object sender, System.EventArgs e)
		{
			Open();		
		}

		private void cmConnections_Popup(object sender, System.EventArgs e)
		{
//			bool en=SelectedItem!=null;
//			miEdit.Enabled=en;
		}

		private void miEdit_Click(object sender, System.EventArgs e)
		{
			Edit();
		}

		private void GisConnectionsForm_Closed(object sender, System.EventArgs e)
		{
		}

		private void miAdd_Click(object sender, System.EventArgs e)
		{
			Add();
		}

		private void miRemove_Click(object sender, System.EventArgs e)
		{
			Remove();
		}

		private void miOpen_Click(object sender, System.EventArgs e)
		{
			Open();
		}

        private void bindingSource_CurrentChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            Open();
        }

	}
}
