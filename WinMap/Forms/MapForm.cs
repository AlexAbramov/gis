using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib.Windows.Forms;

namespace WinMap.Forms
{

	/// <summary>
	/// Summary description for MapForm.
	/// </summary>
	public partial class MapForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;

		public MapForm()
		{
			InitializeComponent();
		}
	
		public void InitForm(App app)
		{
			this.app=app;
			ucMap.InitControl(app);
		}

        public Map Map { get { return ucMap.Map; } }

		public MapUserControl MapUserControl{get{return ucMap;}}

		private void MapForm_Load(object sender, System.EventArgs e)
		{
			app.MainForm.OnMapFormOpened(this);
		}

		public void UpdateTitle()
		{
			Text=string.Format("1:{0}",Map.Scale);
		}

		private void MapForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			app.MainForm.OnMapFormClosed(this);
		}
	}
}
