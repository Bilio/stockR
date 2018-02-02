using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stock.BusinessRule;

namespace Stock
{
	public partial class FormMoney : Form
	{
		public FormMoney()
		{
			InitializeComponent();
		}

		public IEnumerable<Product> Products { get; set; }

		private void InitProductsTable(DateTime start, DateTime end) {
			panel1.Controls.Clear();
			CommonBusinessRule cb = new CommonBusinessRule();
			this.Products = cb.QueryProducts(start, end);
			
			TableLayoutPanel table = new TableLayoutPanel();
			int ic = 0;
			string[] titles = new string[] { "股票代碼","股票名稱","交易日期","交易方式","成交價","數量","手續費","交易稅","收付","損益","報酬率" };
			foreach (var t in titles) {
				Label title = new Label();
				title.Text = t;
				table.Controls.Add(title, ic, 0);
				ic = ic + 1;
			}
			table.Size = new Size(1140, 20);
			panel1.Controls.Add(table);
			table = new TableLayoutPanel();
			int ir = 0;
			foreach (var p in this.Products) {
				Label title = new Label();
				title.Text = p.Id;
				table.Controls.Add(title, 0, ir);

				title = new Label();
				title.Text = p.Name;
				table.Controls.Add(title, 1, ir);

				title = new Label();
				title.Text = p.Date;
				table.Controls.Add(title, 2, ir);

				title = new Label();
				title.Text = p.Type == "B"?"買進":"賣出";
				table.Controls.Add(title, 3, ir);

				title = new Label();
				title.Text = p.Price.ToString();
				table.Controls.Add(title, 4, ir);

				title = new Label();
				title.Text = p.Vol.ToString();
				table.Controls.Add(title, 5, ir);

				title = new Label();
				title.Text = p.Fee.ToString();
				table.Controls.Add(title, 6, ir);

				title = new Label();
				title.Text = p.Tax.ToString();
				table.Controls.Add(title, 7, ir);

				title = new Label();
				title.Text = p.Income.ToString();
				table.Controls.Add(title, 8, ir);

				title = new Label();
				title.Text = p.Increase.ToString();
				table.Controls.Add(title, 9, ir);

				title = new Label();
				title.Text = p.Rate.ToString();
				table.Controls.Add(title, 10, ir);
				ir = ir + 1;
			}
			//table.Location = new Point(14, 43);
			table.Top = 20;
			table.Size = new Size(1140, 300);
			table.AutoScroll = true;
			panel1.Controls.Add(table);
			Chart();
			//this.Controls.Add(table);
		}

		/// <summary>
		/// 今日
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			var today = System.DateTime.Now;
			InitProductsTable(today, today);
		}

		/// <summary>
		/// 本月
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			var today = System.DateTime.Now;
			var firstMondy = new DateTime(today.Year, today.Month, 1);
			InitProductsTable(firstMondy, today);
		}

		/// <summary>
		/// 日期查詢
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			InitProductsTable(dateTimePicker1.Value, dateTimePicker2.Value);
		}

		private void Chart() {
			chart1.Series[0].Points.Clear();
			chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			int sum = 0;
			int c = 0;
			var dayP = this.Products.GroupBy(x => x.Date);
			double[] data1 = new double[dayP.Count()];
			foreach (var p in dayP) {
				sum += p.Sum(x=>x.Income);
				chart1.Series[0].Points.AddXY(p.Key, sum);
				c++;
			}
		}
	}
}
