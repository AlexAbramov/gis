using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Geomethod.Windows.Forms;

namespace WinMap.Forms
{
	public partial class TypesForm : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		App app;
		public TypesForm(App app)
		{
			InitializeComponent();
			this.app = app;
			ucTypes.InitControl(app);
		}

		private void ucTypes_OnCompositeSelected(object sender, EventArgs e)
		{
			ucTypes.App.ShowProperties(ucTypes.SelectedComposite);
		}

		private void TypesForm_Load(object sender, EventArgs e)
		{
            DockPanelUtils.Localize(this);
        }

	}
}

