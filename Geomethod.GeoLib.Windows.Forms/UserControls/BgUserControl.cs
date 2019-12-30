using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;

namespace Geomethod.GeoLib.Windows.Forms
{
	/// <summary>
	/// Summary description for BgUserControl.
	/// </summary>
	public partial class BgUserControl : System.Windows.Forms.UserControl
	{
        IGeoApp app=null;
        bool updating=false;

		#region Events
		public event EventHandler OnBgImageSelected;
		public event EventHandler<BgImageEventArgs> OnBgImageAdded;
		public event EventHandler<BgImageEventArgs> OnBgImageRemoved;
//		public event EventHandler<BgImageEventArgs> OnBgImageChanged;
		public event EventHandler<BgImageEventArgs> OnBgImageChecked;
//		public event EventHandler<BgImageEventArgs> OnBgImageUpdated;
		#endregion

		#region Properties
		public IGeoApp App { get { return app; } }
		public GLib Lib { get { return app.Lib; } }
		public BgImages BgImages { get { return Lib.BgImages; } }
		public BgImage SelectedBgImage { get { TreeNode tn = tvItems.SelectedNode; return tn != null ? tn.Tag as BgImage : null; } }
		#endregion

		public BgUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}

		public void InitControl(IGeoApp app)
		{
			this.app = app;
		}

		void BeginUpdate()
		{
			tvItems.BeginUpdate();
			updating=true;
		}

		void EndUpdate()
		{
			tvItems.EndUpdate();
			updating=false;
		}

		public void UpdateList()
		{
			BeginUpdate();
			tvItems.Nodes.Clear();
			if (app.Lib != null)
			{
				foreach (BgImage bgImage in BgImages)
				{
					AddNode(bgImage);
				}
			}
			EndUpdate();
		}

		private void AddNode(BgImage bgImage)
		{
			TreeNode tn = tvItems.Nodes.Add(bgImage.Name);
			tn.Tag = bgImage;
		}


		private void BgUserControl_Load(object sender, System.EventArgs e)
		{
			if (!base.DesignMode)
			{
				this.dlgOpenFile.Filter = FileFilter.ImagesFilter;
			}
		}

		private void tvItems_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (!updating)
			{
				if(app.GetControlsAttr(ControlsAttr.ShowPropertiesOnSelect)) app.ShowProperties(SelectedBgImage);
				if (OnBgImageSelected!=null) OnBgImageSelected(this, null);
			}
		}

		private void miAdd_Click(object sender, System.EventArgs e)
		{
			AddBgImage();
		}

		private void miRemove_Click(object sender, System.EventArgs e)
		{
			RemoveBgImage();
		}

		private void RemoveBgImage()
		{
			try
			{
				TreeNode tn=tvItems.SelectedNode;
				if (tn!=null)
				{
					BgImage bgImage = tn.Tag as BgImage;
					if (MessageBoxUtils.AskLocalized("_removeitems"))
					{
						tvItems.Nodes.Remove(tn);
						BgImages.Remove(bgImage);
						if (app.GetControlsAttr(ControlsAttr.AutoSave)) using (Context context = Lib.GetContext()) bgImage.Remove(context);
						if (OnBgImageRemoved!=null) OnBgImageRemoved(this, new BgImageEventArgs(bgImage));
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		void AddBgImage()
		{
			try
			{
				GLib lib =Lib;
				Map map=app.CurrentMap;
				if (lib != null && map != null)
				{
					if (this.dlgOpenFile.ShowDialog() == DialogResult.OK)
					{
						BgImage bgImage = new BgImage(map);
						bgImage.FilePath = dlgOpenFile.FileName;
						BgImages.Add(bgImage);
						UpdateList();
						if (app.GetControlsAttr(ControlsAttr.AutoSave)) using (Context context = lib.GetContext()) bgImage.Save(context);
						if (OnBgImageAdded!=null) OnBgImageAdded(this, new BgImageEventArgs(bgImage));
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
		}

		private void tvItems_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Right)
			{
				TreeNode node=tvItems.GetNodeAt(e.X,e.Y);
				if(node!=null) tvItems.SelectedNode=node;
			}				
		}

		private void tvItems_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			try
			{
				if (!updating)
				{
					BgImage bgImage = e.Node.Tag as BgImage;
					bgImage.isChecked = e.Node.Checked;
					if (OnBgImageChecked!=null) OnBgImageChecked(this, new BgImageEventArgs(bgImage));
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
		}
	}

	public class BgImageEventArgs : EventArgs
	{
		BgImage bgImage;
		public BgImage BgImage { get { return bgImage; } }
		public BgImageEventArgs(BgImage bgImage) { this.bgImage = bgImage; }
	}
}
