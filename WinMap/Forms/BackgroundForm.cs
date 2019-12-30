using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinMap.Forms
{
	public partial class BackgroundForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;
		public BackgroundForm(App app)
		{
			InitializeComponent();
			this.app = app;
			ucBg.InitControl(app);
		}

        private void BackgroundForm_Load(object sender, EventArgs e)
        {
            DockPanelUtils.Localize(this);
        }
	}
}

