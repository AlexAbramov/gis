using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Geomethod;
using Geomethod.Windows.Forms;

namespace Geomethod.GeoLib.Windows.Forms
{
	enum SortMode { Name, Brightness}
	public partial class ColorsForm : Form
	{
        IGeoApp app = null;
        GLib lib;
		Colors colors;

        #region Events
        public event EventHandler<NamedColorEventArgs> OnNamedColorAdded;
        public event EventHandler<NamedColorEventArgs> OnNamedColorRemoved;
        public event EventHandler<NamedColorEventArgs> OnNamedColorChanged;
        #endregion

        #region Properties
        public IGeoApp App { get { return app; } }
        public GLib Lib { get { return app.Lib; } }
		bool IsEditMode { get { return listView.SelectedItems.Count == 1; } }
		ListViewItem SelectedItem { get { return listView.SelectedItems.Count ==1 ? listView.SelectedItems[0] : null; } }
		NamedColor SelectedNamedColor { get { ListViewItem lvi=SelectedItem; return lvi!=null ? lvi.Tag as NamedColor : null; } }
        #endregion

        public ColorsForm(IGeoApp app)
		{
			InitializeComponent();
            this.app = app;
			this.lib = app.Lib;
			this.colors = lib.Colors;
        }


		private void ColorsForm_Load(object sender, EventArgs e)
		{
    	    GmApplication.Initialize(this);
			MinimumSize = Size;
			if (colors.Count>0) foreach (NamedColor nc in colors) AddColor(nc);
			SetSorting(SortMode.Name);
			UpdateColor();
			UpdateControls();
		}

		bool CanChange
		{
			get
			{
				if (IsEditMode)
				{
					NamedColor origColor = SelectedNamedColor;
					string name = tbColorName.Text.Trim();
					Color color = ucColor.BackColor;
					bool isOk = name.Length > 0 && color != Color.Empty;
					bool isChanged = origColor.Color != color || origColor.Name != name;
					return isOk && isChanged && IsUniqueColorName(name, origColor);
				}
				return false;
			}
		}

		bool CanAdd
		{
			get 
			{
				string name = tbColorName.Text.Trim();
				Color color = ucColor.BackColor;
				bool isOk = name.Length > 0 && color != Color.Empty;
				return !IsEditMode && isOk && IsUniqueColorName(name, null);
			}
		}

		bool IsUniqueColorName(string colorName, NamedColor ignoredItem)
		{
			if (colors.Count > 0)
			{
				foreach (NamedColor nc in colors)
				{
					if (nc != ignoredItem && string.Compare(nc.Name, colorName, true) == 0) return false;
				}
			}
			return true;
		}

		void UpdateControls()
		{
			btnChange.Enabled = CanChange;
			btnAdd.Enabled = CanAdd;
			btnRemove.Enabled = listView.SelectedItems.Count>0;
			btnUnselect.Enabled = listView.SelectedItems.Count > 0;
		}

		private void btnChange_Click(object sender, EventArgs e)
		{
			if (CanChange)
			{
				NamedColor origColor = SelectedNamedColor;
				string name = tbColorName.Text.Trim();
				Color color = ucColor.BackColor;
				origColor.Name = name;
				origColor.Color = color;
				foreach (ListViewItem lvi in listView.Items)
				{
					if (lvi.Tag == origColor)
					{
						lvi.Text = origColor.Name;
						lvi.SubItems[1].BackColor = origColor.Color;
						break;
					}
				}
				listView.Sort();
				UpdateControls();
                if (app.GetControlsAttr(ControlsAttr.AutoSave)) using (Context context = lib.GetContext()) origColor.Save(context);
                if (OnNamedColorChanged != null) OnNamedColorChanged(this, new NamedColorEventArgs(origColor));
            }
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (CanAdd)
			{
				string name = tbColorName.Text.Trim();
				Color color = ucColor.BackColor;
				NamedColor nc = new NamedColor(lib, name, color);
				colors.Add(nc);
				AddColor(nc);
				listView.Sort();
				UpdateControls();
                if (app.GetControlsAttr(ControlsAttr.AutoSave)) using (Context context = lib.GetContext()) nc.Save(context);
                if (OnNamedColorAdded != null) OnNamedColorAdded(this, new NamedColorEventArgs(nc));
			}
		}

		private void AddColor(NamedColor nc)
		{
			string[] subitems ={nc.Name, ""};
			ListViewItem lvi = new ListViewItem(subitems);
			lvi.UseItemStyleForSubItems = false;
			lvi.Tag = nc;
			lvi.SubItems[1].BackColor=nc.Color;
			listView.Items.Add(lvi);
		}

		private void tbColorName_TextChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem lvi in listView.SelectedItems)
			{
                NamedColor nc = lvi.Tag as NamedColor;
				colors.Remove(nc);
				listView.Items.Remove(lvi);
                if (app.GetControlsAttr(ControlsAttr.AutoSave)) using (Context context = lib.GetContext()) nc.Remove(context);
                if (OnNamedColorRemoved != null) OnNamedColorRemoved(this, new NamedColorEventArgs(nc));
            }
			UpdateControls();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
//			App.Config.SetChanged();
		}

		private void SetSorting(SortMode sortMode)
		{
			cbSort.SelectedIndex = (int)sortMode;
			this.listView.ListViewItemSorter = new ListViewItemComparer(sortMode, colors);
		}

		private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Set the ListViewItemSorter property to a new ListViewItemComparer 
			// object. Setting this property immediately sorts the 
			// ListView using the ListViewItemComparer object.
			switch (e.Column)
			{
				case 0: 
					SetSorting(SortMode.Name); 
					break;
				case 1: 
					SetSorting(SortMode.Brightness); 
					break;
			}
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IsEditMode)
			{
				NamedColor nc = SelectedNamedColor;
				tbColorName.Text = nc.Name;
				ucColor.BackColor = nc.Color;
				UpdateARGB();
			}
			UpdateControls();
		}

		private void UpdateARGB()
		{
			Color c = ucColor.BackColor;
			nudAlpha.Value = c.A;
			nudRed.Value = c.R;
			nudGreen.Value = c.G;
			nudBlue.Value = c.B;
		}


		private void cbSort_SelectedIndexChanged(object sender, EventArgs e)
		{
			SortMode sortMode = (SortMode)cbSort.SelectedIndex;
			SetSorting(sortMode);
		}

		private void btnColor_Click(object sender, EventArgs e)
		{
			if (dlgColor.ShowDialog() == DialogResult.OK)
			{
				ucColor.BackColor = dlgColor.Color;
				UpdateARGB();
				UpdateControls();
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			listView.SelectedItems.Clear();
			UpdateControls();
		}

		private void nudAlpha_ValueChanged(object sender, EventArgs e)
		{
			UpdateColor();
		}

		private void nudRed_ValueChanged(object sender, EventArgs e)
		{
			UpdateColor();
		}

		private void nudGreen_ValueChanged(object sender, EventArgs e)
		{
			UpdateColor();
		}

		private void nudBlue_ValueChanged(object sender, EventArgs e)
		{
			UpdateColor();
		}

		private void UpdateColor()
		{
			Color c = Color.FromArgb((int)nudAlpha.Value, (int)nudRed.Value, (int)nudGreen.Value, (int)nudBlue.Value);
			ucColor.BackColor = c;
			UpdateControls();
		}

	}

	// Implements the manual sorting of items by columns.
	class ListViewItemComparer : System.Collections.IComparer
	{
		SortMode sortMode;
		Colors colors;
		public ListViewItemComparer(SortMode sortMode, Colors colors)
		{
			this.sortMode = sortMode;
			this.colors = colors;
		}
		public int Compare(object x, object y)
		{
			ListViewItem lvi1 = (ListViewItem)x;
			ListViewItem lvi2 = (ListViewItem)y;
			NamedColor nc1 = (NamedColor)lvi1.Tag;
			NamedColor nc2 = (NamedColor)lvi2.Tag;
			switch (sortMode)
			{
/*				case SortMode.Custom:
					int i1 = colors.IndexOf(nc1);
					int i2 = colors.IndexOf(nc2);
					return Math.Sign(i1 - i2);*/
				case SortMode.Brightness:
					return Math.Sign(nc2.Color.GetBrightness()-nc1.Color.GetBrightness());
				case SortMode.Name:
					return String.Compare(nc1.Name, nc2.Name);
			}
			return 0;
		}
	}

    public class NamedColorEventArgs : EventArgs
    {
        NamedColor namedColor;
        public NamedColor NamedColor { get { return namedColor; } }
        public NamedColorEventArgs(NamedColor namedColor) { this.namedColor = namedColor; }
    }

}