using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Geomethod;
using Geomethod.GeoLib;
using Geomethod.Windows.Forms;

namespace WinMap.Forms
{
	public partial class ViewsForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;
		public ViewsForm(App app)
		{
			InitializeComponent();
			this.app = app;
			ucViews.InitControl(app);
		}

		private void ViewsForm_Load(object sender, EventArgs e)
		{
            DockPanelUtils.Localize(this);
		}

        private void ucViews_OnViewSelected(object sender, EventArgs e)
        {
            Geomethod.GeoLib.View selView = ucViews.SelectedView;
            if (selView != null)
            {
                MapForm mapForm = app.MainForm.MapForm;
                if (mapForm != null)
                {
                    mapForm.Map.SetView(selView);
                    mapForm.MapUserControl.Repaint();
                    app.MainForm.UpdateScaleCombo();
                }
            }
        }

	}
}

