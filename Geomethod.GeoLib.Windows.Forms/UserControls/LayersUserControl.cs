using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;
using Geomethod;

namespace Geomethod.GeoLib.Windows.Forms
{

	/// <summary>
	/// Summary description for LayersUserControl.
	/// </summary>
	public partial class LayersUserControl : System.Windows.Forms.UserControl
	{
		IGeoApp app;

		#region Events
		public event EventHandler OnLayerSelected;
		public event EventHandler<LayerEventArgs> OnLayerAdded;
		public event EventHandler<LayerEventArgs> OnLayerRemoved;
		public event EventHandler<LayerEventArgs> OnLayerChanged;
		public event EventHandler<LayerEventArgs> OnLayerUpdated;
		#endregion

		#region Properties
		GLib Lib { get { return app.Lib; } }
		Layers Layers { get { return Lib.Layers; } }
		public Layer AppLayer { get { return app.Layer; } }
		bool IsAllInclusiveLayerSelected
		{ 
			get
			{
				foreach(Layer layer in lbLayers.SelectedItems) if(layer.IsAllInclusive) return true;
				return false;
			}
		}
		public Layer SelectedLayer
		{
			get
			{
				if(lbLayers.SelectedItems.Count!=1) return null;
				return (Layer)lbLayers.SelectedItems[0];
			}
		}
		public IEnumerable<Layer> SelectedLayers
		{
			get
			{
				return (IEnumerable<Layer>)lbLayers.SelectedItems.GetEnumerator();
			}
		}
		bool AutoSave { get { return app.GetControlsAttr(ControlsAttr.AutoSave); } }
		#endregion

		public LayersUserControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public void InitControl(IGeoApp app)
		{
			this.app = app;
		}


		public void UpdateList() { UpdateList(null); }
		public void UpdateList(Layer selLayer)
		{
			lbLayers.BeginUpdate();
			lbLayers.Items.Clear();
			if (app.Lib != null)
			{
				foreach(Layer layer in Layers)
				{
					lbLayers.Items.Add(layer);
					if(selLayer==null && layer.IsAllInclusive) selLayer=layer;
				}
				lbLayers.SelectedItem = selLayer;
			}
			lbLayers.EndUpdate();
		}

		private void lbLayers_SelectedValueChanged(object sender, System.EventArgs e)
		{
			OnLayerSelected(this, null);
			if(app.GetControlsAttr(ControlsAttr.ShowPropertiesOnSelect)) app.ShowProperties(SelectedLayer);
		}

		private void miAdd_Click(object sender, System.EventArgs e)
		{
			AddLayer();
		}

		private void miUpdate_Click(object sender, System.EventArgs e)
		{
			UpdateSelectedLayer();
		}

		private void miRemove_Click(object sender, System.EventArgs e)
		{
			RemoveSelectedLayer();
		}

		private void miRename_Click(object sender, System.EventArgs e)
		{
			RenameSelectedLayer();
		}

		private void cmLayers_Popup(object sender, System.EventArgs e)
		{
			int selCount=lbLayers.SelectedItems.Count;
			if(IsAllInclusiveLayerSelected) selCount=0;
			miUpdate.Enabled = selCount == 1 && AppLayer != null;
			miRemove.Enabled=selCount>0;
			miRename.Enabled=selCount==1;
		}

		public bool AddLayer()
		{
			try
			{
				NameForm form = new NameForm(Layers);
				if (form.ShowDialog(this) == DialogResult.OK)
				{
					Layer layer = new Layer(Layers.Lib, form.InputText);
					layer.Init(AppLayer);
					if (Layers.Add(layer))
					{
						this.UpdateList(layer);
						if (AutoSave)
						{
							using (Context context = Lib.GetContext()) layer.Save(context);
						}
						if (OnLayerAdded!=null) OnLayerAdded(this, new LayerEventArgs(layer));
						return true;
					}
					else
					{
						string s = Locale.Get("_notuniquename") + ": " + layer.Name;
						MessageBox.Show(s);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			return false;
		}

		public bool RemoveSelectedLayer()
		{
			try
			{
				Layer layer=SelectedLayer;
				if (layer != null)
				{
					if (MessageBox.Show(Locale.Get("_removeitems"), Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						if (Layers.Remove(layer))
						{
							this.UpdateList();
							if (AutoSave) using (Context context = Lib.GetContext()) layer.Remove(context);
							if (OnLayerRemoved!=null) OnLayerRemoved(this, new LayerEventArgs(layer));
							return true;
						}
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			return false;
		}

		public bool RenameSelectedLayer()
		{
			try
			{
				Layer layer=SelectedLayer;
				if(layer!=null)
				{
					NameForm form=new NameForm(Layers);
					if (form.ShowDialog(this) == DialogResult.OK)
					{
						if (layer.Name != form.InputText)
						{
							layer.Name = form.InputText;
							if (AutoSave)
							{
								using(Context context=Lib.GetContext()) layer.Save(context);
							}
							this.UpdateList();
							if (OnLayerChanged!=null) OnLayerChanged(this, new LayerEventArgs(layer));
							return true;
						}
					}
				}
			}
			catch(Exception ex)
			{
				Log.Exception(ex);
			}
			return false;
		}

		public bool UpdateSelectedLayer()
		{
			Layer layer=SelectedLayer;
			if (layer != null && AppLayer != null)
			{
				try
				{
					if (MessageBox.Show(Locale.Get("_UpdateSelectedLayer"), Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						layer.Init(AppLayer);
						if (AutoSave) using (Context context = Lib.GetContext()) layer.Save(context);
						if (OnLayerUpdated!=null) OnLayerUpdated(this, new LayerEventArgs(layer));
						return true;
					}
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
				}
			}
			return false;
		}

		public void UpdateAppLayer()
		{
			if (IsAllInclusiveLayerSelected)
			{
				AppLayer.InitFromLib();
			}
			else
			{
				AppLayer.Clear();
				foreach(Layer layer in lbLayers.SelectedItems)
				{
					AppLayer.Merge(layer);
				}
			}
		}

		private void lbLayers_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		}

		private void LayersUserControl_Load(object sender, System.EventArgs e)
		{
		}

		private void lbLayers_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.Delete:
					this.RemoveSelectedLayer();
					break;
				case Keys.Insert:
					this.AddLayer();
					break;
			}
		}


	}

	public class LayerEventArgs : EventArgs
	{
		Layer layer;
		public Layer Layer { get { return layer; } }
		public LayerEventArgs(Layer layer) { this.layer = layer; }
	}
}
