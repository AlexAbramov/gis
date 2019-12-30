using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Data;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for GisConnectionForm.
	/// </summary>
	public partial class ConnectionForm : System.Windows.Forms.Form
	{

		GisConnections gisConnections;
		GisConnection gisConnection;
		bool editing=true;
		bool formLoaded=false;

		public GisConnection GisConnection{get{return gisConnection;}}

		public ConnectionForm(GisConnections gisConnections,GisConnection gisConnection)
		{
			InitializeComponent();

			if(gisConnection==null)
			{
				gisConnection=new GisConnection("","","","");
				editing=false;
			}
			this.gisConnections=gisConnections;
			this.gisConnection=gisConnection;
		}


		private void btnOk_Click(object sender, System.EventArgs e)
		{
			gisConnection.Name=tbName.Text.Trim();
			gisConnection.ProviderName=this.cbDataProvider.Text;
			gisConnection.ConnectionString=tbConnStr.Text.Trim();
			gisConnection.Options=tbOptions.Text.Trim();
		}

		private void GisConnectionForm_Load(object sender, System.EventArgs e)
		{
			LocaleUtils.Localize(this);
			tbName.Text=gisConnection.Name;
			tbConnStr.Text=gisConnection.ConnectionString;
			InitDataProviderList();
			cbDataProvider.Text=gisConnection.ProviderName;
			tbOptions.Text=gisConnection.Options;
			formLoaded=true;
			UpdateControls();
			MinimumSize=Size;
			MaximumSize=new Size(GConfig.Instance.geometry.maxFormWidth,Size.Height);
		}

		void InitDataProviderList()
		{
			this.cbDataProvider.Items.Add("");
			foreach(GmProviderFactory pr in GmProviders.Items)
			{
				this.cbDataProvider.Items.Add(pr.Name);
			}
		}

		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
			UpdateControls();
		}

		private void tbConnStr_TextChanged(object sender, System.EventArgs e)
		{
			UpdateControls();		
		}

		void UpdateControls()
		{
            if (formLoaded)
            {
                string name=tbName.Text.Trim();
                bool enabled = name.Length > 0;
                if (enabled)
                {
                    if (!editing || gisConnection.Name != name)
                    {
                        enabled = !gisConnections.HasName(name);
                    }
                }
                btnOk.Enabled = enabled;
            }
		}
	}
}
