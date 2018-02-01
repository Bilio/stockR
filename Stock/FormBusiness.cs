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
			OpenBig();
		}

		private void OpenT()
		{
			var formBig = new FormT();
			formBig.TopLevel = false;
			formBig.Visible = true;

			formBig.FormBorderStyle = FormBorderStyle.None;
			//formBig.ControlBox = false;

			formBig.MaximizeBox = true;
			formBig.MinimizeBox = true;
			formBig.Padding = new Padding(0, 0, 0, 0);
			formBig.Location = new Point(0, 0);
			
			formBig.Top = 0;
			formBig.Left = 0;
			tabPage1.Controls.Add(formBig);
			tabPage1.Show();
		}

		private void OpenBig() {
			var formBig = new FormBig();
			formBig.FormBorderStyle = FormBorderStyle.None;
			//formBig.ControlBox = false;
			formBig.TopLevel = false;
			formBig.Visible = true;
			formBig.Top = 0;
			formBig.Left = 0;
			tabPage1.Controls.Add(formBig);
			tabPage1.Show();
		}

		private void OpenStock() {
			var form = new Form1();
			form.FormBorderStyle = FormBorderStyle.None;
			form.ControlBox = false;
			form.TopLevel = false;
			form.Visible = true;
			tabPage2.Controls.Add(form);
			tabPage2.Show();
		}

		private void OpenMoney() {
			var form = new FormMoney();
			form.FormBorderStyle = FormBorderStyle.None;
			form.ControlBox = false;
			form.TopLevel = false;
			form.Visible = true;
			tabPage3.Controls.Add(form);
			tabPage3.Show();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch ((sender as TabControl).SelectedIndex)
			{
				case 0:
					OpenBig();
					break;
				case 1:
					OpenStock();
					break;
				case 2:
					OpenMoney();
					break;
				default:
					OpenBig();
					break;
			}
		}
	}
}
