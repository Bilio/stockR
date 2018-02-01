using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stock.Download;

namespace Stock
{
	public partial class FormBusiness : Form
	{
		public FormBusiness()
		{
			
			InitializeComponent();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch ((sender as TabControl).SelectedIndex)
			{
				case 0:
					
					break;
				case 1:
					var form = new Form1();
					form.FormBorderStyle = FormBorderStyle.None;
					form.ControlBox = false;
					form.TopLevel = false;
					form.Visible = true;
					tabPage1.Controls.Add(form);
					tabPage1.Show();
					break;
			}
		}
	}
}
