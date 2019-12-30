using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for ViewsUserControl.
	/// </summary>
	public partial class ViewsUserControl : System.Windows.Forms.UserControl
	{
		IGeoApp app = null;

		#region Events
		public event EventHandler OnViewSelected;
		public event EventHandler<ViewEventArgs> OnViewAdded;
		public event EventHandler<ViewEventArgs> OnViewRemoved;
		public event EventHandler<ViewEventArgs> OnViewChanged;
		public event EventHandler<ViewEventArgs> OnViewUpdated;
		#endregion

		#region Properties
		public GLib Lib { get { return app.Lib; } }
		public Views Views { get { return Lib.Views; } }
		bool IsWholeMapViewSelected
		{
			get
			{
				foreach (View view in lbViews.SelectedItems) if (view.IsOverall) return true;
				return false;
			}
		}
		public View SelectedView
		{
			get
			{
				if (lbViews.SelectedItems.Count != 1) return null;
				return (View)lbViews.SelectedItems[0];
			}
		}
		public IEnumerable<View> SelectedViews
		{
			get
			{
				return (IEnumerable<View>)lbViews.SelectedItems.GetEnumerator();
			}
		}
		bool AutoSave { get { return app.GetControlsAttr(ControlsAttr.AutoSave); } }
		#endregion
		
		public void InitControl(IGeoApp app) { this.app = app; }


		public ViewsUserControl()
		{
			InitializeComponent();
		}


		bool DefaultViewSelected
		{ 
			get
			{
				foreach(string s in lbViews.SelectedItems) if(s==Constants.DefaultViewName) return true;
				return false;
			}
		}

		public void UpdateList(){UpdateList(null);}
		public void UpdateList(View selView)
		{
			lbViews.BeginUpdate();
			lbViews.Items.Clear();
			if (app.Lib != null)
			{
				foreach (GeoLib.View view in Views)
				{
					lbViews.Items.Add(view);
				}
				lbViews.SelectedItem=selView;
			}
			lbViews.EndUpdate();
		}

		private void lbViews_SelectedValueChanged(object sender, System.EventArgs e)
		{
            try
            {
                if (OnViewSelected != null) OnViewSelected(this, null);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
		}

		private void miAdd_Click(object sender, System.EventArgs e)
		{
			AddView();
		}

		private void miUpdate_Click(object sender, System.EventArgs e)
		{
			UpdateView();
		}

		private void miRemove_Click(object sender, System.EventArgs e)
		{
			RemoveView();
		}

		private void miRename_Click(object sender, System.EventArgs e)
		{
			RenameView();
		}

		private void cmViews_Popup(object sender, System.EventArgs e)
		{
			int selCount=lbViews.SelectedItems.Count;
			if(DefaultViewSelected) selCount=0;
			miUpdate.Enabled=selCount==1;
			miRemove.Enabled=selCount>0;
			miRename.Enabled=selCount==1;
		}

		void AddView()
		{
			try
			{
				Map map=app.CurrentMap;
				if(map!=null)
				{
					NameForm form=new NameForm(Views);
					if(form.ShowDialog(this)==DialogResult.OK)
					{
						GeoLib.View view=new GeoLib.View(form.InputText,map);
						if(Views.Add(view))
						{
							this.UpdateList(view);
							if(app.GetControlsAttr(ControlsAttr.AutoSave)) using(Context context=Lib.GetContext()) view.Save(context);
							if (OnViewAdded!=null) OnViewAdded(this, new ViewEventArgs(view));
						}
						else
						{
							string s=Locale.Get("_notuniquename")+": "+view.Name;
						  MessageBox.Show(s);
						}
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void RemoveView()
		{
			try
			{
				View selView=SelectedView;
				if (selView != null && !selView.IsOverall)
				{
					if (MessageBoxUtils.AskLocalized("_removeitems"))
					{
						if (Views.Remove(selView))
						{
							this.UpdateList();
							if(app.GetControlsAttr(ControlsAttr.AutoSave)) using(Context context=Lib.GetContext()) selView.Remove(context);
							if (OnViewRemoved!=null) OnViewRemoved(this, new ViewEventArgs(selView));
						}
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void RenameView()
		{
			try
			{
				View selView = SelectedView;
				if (selView != null && !selView.IsOverall)
				{
					NameForm form = new NameForm(Views);
					if (form.ShowDialog(this) == DialogResult.OK)
					{
						selView.Name = form.InputText;
						this.UpdateList();
						if(app.GetControlsAttr(ControlsAttr.AutoSave)) using(Context context=Lib.GetContext()) selView.Save(context);
						if (OnViewChanged!=null) OnViewChanged(this, new ViewEventArgs(selView));
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void UpdateView()
		{
			try
			{
				View selView = SelectedView;
				if (selView != null && !selView.IsOverall)
				{
					Map map = app.CurrentMap;
					if (map != null)
					{
						selView.Init(map);
						if (app.GetControlsAttr(ControlsAttr.AutoSave)) using (Context context = Lib.GetContext()) selView.Save(context);
						if (OnViewUpdated!=null) OnViewUpdated(this, new ViewEventArgs(selView));
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void ViewsUserControl_Load(object sender, System.EventArgs e)
		{
		}

		private void lbViews_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Delete:
					RemoveView();
					break;
				case Keys.Insert:
					AddView();
					break;
			}
		}

		private void lbViews_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}


	}

	public class ViewEventArgs : EventArgs
	{
		View view;
		public View View { get { return view; } }
		public ViewEventArgs(View view) { this.view = view; }
	}
}
