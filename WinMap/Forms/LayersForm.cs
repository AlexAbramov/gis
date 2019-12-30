using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinMap.Forms
{
	public partial class LayersForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;
		public LayersForm(App app)
		{
			InitializeComponent();
			this.app = app;
			ucLayers.InitControl(app);
		}

        private void LayersForm_Load(object sender, EventArgs e)
        {
            DockPanelUtils.Localize(this);
        }
	}
}

