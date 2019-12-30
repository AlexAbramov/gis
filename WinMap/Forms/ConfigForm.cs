using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinMap.Forms
{
	public partial class ConfigForm : Form
	{
		Config config;
		public ConfigForm(Config config)
		{
			InitializeComponent();
			this.config = config;
		}

		private void ConfigForm_Load(object sender, EventArgs e)
		{

		}
	}
}