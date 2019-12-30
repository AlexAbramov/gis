using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinMap.Forms
{
	public partial class SearchForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;
		public SearchForm(App app)
		{
			InitializeComponent();
			this.app = app;
			ucSearch.InitControl(app);
		}

        private void SearchForm_Load(object sender, EventArgs e)
        {
            DockPanelUtils.Localize(this);
        }
	}
}

