using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinMap.Forms
{
	public partial class PropertiesForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;
		public PropertiesForm(App app)
		{
			InitializeComponent();
			this.app = app;
			ucProperties.InitControl(app);
		}

		private void PropertiesForm_Load(object sender, EventArgs e)
		{
            DockPanelUtils.Localize(this);
        }
	}
}

