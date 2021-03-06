﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stock.BusinessRule;
using Stock.Temp;

namespace Stock
{
	public partial class FormBig : Form
	{
		public FormBig()
		{
			InitializeComponent();
			webBrowser1.Url = new Uri("http://www.luckstar.com.tw/Taiex/FTaiex.aspx");
		//	webBrowser1.Url = new Uri(string.Format(@"https://histock.tw/stock/tcharti.aspx?no=0000&m=all"));
		}

		private void button1_Click(object sender, EventArgs e)
		{
			label1.Text = "交易中";
			UpBusinessRule rule = new UpBusinessRule();
			
			var Stocks = rule.Buy();
			foreach (var s in Stocks)
			{
				//輸入資料
				ListViewItem item = new ListViewItem(s.Id + s.Name); //ID   
				listView1.Items.Add(item);
			}
			label1.Text = "交易完成";
		}

		private void button2_Click(object sender, EventArgs e)
		{
			label1.Text = "交易中";
			UpBusinessRule rule = new UpBusinessRule();
			var Stocks = rule.Sale();
			foreach (var s in Stocks)
			{
				//輸入資料
				ListViewItem item = new ListViewItem(s.Id + s.Name); //ID   
				listView1.Items.Add(item);
			}
			label1.Text = "交易完成";
		}

		private void button3_Click(object sender, EventArgs e)
		{
			label1.Text = "交易中";
			UpBusinessRule rule = new UpBusinessRule();
			DateTime start = new DateTime(2018, 2, 22);
			DateTime end = System.DateTime.Now;
			while (start < end)
			{
				rule.Buy(start);
				rule.Sale(start);
				start = start.AddDays(1);
			}
			label1.Text = "交易完成";

		}

		private void button4_Click(object sender, EventArgs e)
		{
			AppParse ap = new AppParse();
			ap.ParseB(textBox1.Text);
		}
	}
}
