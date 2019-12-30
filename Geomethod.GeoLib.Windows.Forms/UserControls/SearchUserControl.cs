using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for SearchUserControl.
	/// </summary>
	public partial class SearchUserControl : System.Windows.Forms.UserControl
	{
		IGeoApp app=null;

		public void InitControl(IGeoApp app) { this.app = app; }

		DataTable dtSearch=new DataTable();

		#region Properties
		GLib Lib { get { return app.Lib; } }
		public Layer AppLayer { get { return app.Layer; } }
		bool AutoSave { get { return app.GetControlsAttr(ControlsAttr.AutoSave); } }
		public GType SelectedType { get { return cbType.SelectedItem as GType; } }
		#endregion
		
		public SearchUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public void SetSearchCondition(GType type, string searchStr)
		{
			this.cbType.SelectedItem=type;
			if(searchStr==null) searchStr="";
			this.tbSearch.Text=searchStr;
		}

		void Search()
		{
			string text=tbSearch.Text;
			try
			{
				using (WaitCursor wr = new WaitCursor(app, Locale.Get("_searching...")))
				{
					dgSearch.BeginInit();
					dgSearch.DataSource=null;
					dtSearch.Clear();
                    GType type=SelectedType;
                    int typeId = type!=null? type.Id:0;
                    if(app.Lib.HasDb)
					  SearchUtils.SqlSearch(app.Lib,text,typeId,dtSearch);
                    else 
					  SearchUtils.Search(app.Lib,text,typeId,dtSearch);
					app.Status=string.Format("{0} records found",dtSearch.Rows.Count);
					dgSearch.DataSource=dtSearch;
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			finally
			{
				dgSearch.EndInit();
			}
		}

		void Clear()
		{
			dtSearch.Rows.Clear();
			GLib lib = null;
			if(lib==null) return;
			if(lib.Selection.Count>0)
			{
                Rect bounds = lib.Selection.Bounds;
				lib.Selection.Clear();
                app.CheckRepaint(bounds);
			}
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			Search();		
		}

		private void tbSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Enter) Search();		
		}

		private void dgSearch_DoubleClick(object sender, System.EventArgs e)
		{
			ShowObject();		
		}

		public void ShowObject()
		{
			try
			{
				GObject obj=null;
				GLib lib = Lib;
				using(WaitCursor wr=new WaitCursor(app,"Loading object..."))
				{
					DataGridCell dc=dgSearch.CurrentCell;
					BindingManagerBase bm=dgSearch.BindingContext[dtSearch];
					if(bm.Count==0 || bm.Current.GetType() != typeof(DataRowView)) return;
					DataRowView drv = (DataRowView) bm.Current;
					int objectId=(int)drv[ObjectField.Id.ToString()];
					int rangeId=(int)drv[ObjectField.RangeId.ToString()];
					obj=lib.GetObject(objectId,rangeId);
					if(obj==null)
					{
						obj=lib.LoadObject(objectId);
					}
					if(obj!=null)
					{
						lib.Selection.Set(obj);
						MapUserControl mapCtl=app.CurrentMapControl;
                        if (mapCtl != null)
						{
                            mapCtl.Map.EnsureVisible(lib.Selection.Bounds);
                            mapCtl.Repaint();
						}
					}
				}
				app.ShowProperties(obj);
				if(obj==null)	MessageBox.Show(Locale.Get("_objectsnotfound"));
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void cmSearch_Popup(object sender, System.EventArgs e)
		{
			miClear.Enabled=dtSearch.Rows.Count>0;
		}

		private void SearchUserControl_Load(object sender, System.EventArgs e)
		{
		}

		public void UpdateTypes()
		{
			cbType.Items.Clear();
			GLib lib = Lib;
			if(lib==null)
			{
				Clear();
				return;
			}
			cbType.BeginUpdate();
			foreach(GType type in lib.AllTypes)
			{
				cbType.Items.Add(type.Name);
			}
			cbType.EndUpdate();
		}

		private void dgSearch_Click(object sender, System.EventArgs e)
		{
			ShowObject();		
		}

		private void miClear_Click(object sender, System.EventArgs e)
		{		
			Clear();
		}

		private void dgSearch_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{

		}
	}
}
