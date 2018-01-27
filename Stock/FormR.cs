using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RDotNet;
using RDotNet.Devices;
using RDotNet.Internals;
using Stock.FilterRule;

namespace Stock
{
	public partial class FormR : Form
	{
		
		public IEnumerable<Stock> Stocks { get; set; }

		private int iStock = -1;

		public Stock Stock
		{
			get
			{
				return Stocks.ToList()[iStock];
			}
		}

		private SQLiteConnection sqlite_conn;
		private SQLiteCommand sqlite_cmd;
		public FormR()
		{
			sqlite_conn = new SQLiteConnection("Data source=database.db");
			// Open
			sqlite_conn.Open();
			if (!TableExists("Stocks", sqlite_conn))
			{
				// 要下任何命令先取得該連結的執行命令物件
				sqlite_cmd = sqlite_conn.CreateCommand();

				// 要下的命令新增一個表
				sqlite_cmd.CommandText = "CREATE Table Stocks  (id varchar(7) primary key, name nvarchar(10), type varchar(1), createDate datetime, modifyDate datetime);";


				sqlite_cmd.ExecuteNonQuery();
			}
			sqlite_conn.Close();
			InitializeComponent();
		}

		public static bool TableExists(String tableName, SQLiteConnection connection)
		{
			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
			cmd.Parameters.Add("@name", DbType.String).Value = tableName;
			return (cmd.ExecuteScalar() != null);
		}

		/// <summary>
		/// 下一個
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{

			iStock = iStock + 1;
			GoToWeb();
		}

		/// <summary>
		/// 上一個
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{

			iStock = iStock - 1;
			GoToWeb();
		}

		private void GoTo() {
			try
			{
				string today = System.DateTime.Today.ToString("yyyy-MM-dd");
				string yearAgo = System.DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
				string rnd = System.Guid.NewGuid().ToString().Replace("-", "");
				string filename = "i" + rnd + "__Rimage";
				string filenamePNG = filename+".png";
				string rcode = string.Format(@"library(quantmod)
			stock <- getSymbols('{0}.TW', auto.assign = FALSE,from='{1}')
			ind <- apply(stock, 1, function(x) all(is.na(x)))
			stock <- stock[!ind,]
			chartSeries(stock['{1}::{2}'],name='{3}')",
			Stock.Id, "2017-01-03", "2018-01-25", filename);
				REngine engine = REngine.GetInstance();
				engine.Initialize();

				//engine.Evaluate(string.Format("png(file='{0}',bg ='transparent',width={1},height={2})", filename, this.pictureBox1.Width, this.pictureBox1.Height));

				engine.Evaluate(rcode);
				//engine.Evaluate("dev.off");

				//string path = System.IO.Path.GetFullPath(filenamePNG);
				//Bitmap image = new Bitmap(path);
				//pictureBox1.Image = image;
				label1.Text = string.Format("{0} {1} {2}/{3}", Stock.Id, Stock.Name, iStock, Stocks.Count());
				label2.Text = GetStockType();
			}
			catch (Exception e) {

			}
		}

		private void GoToWeb() {
			webBrowser1.Url = new Uri(string.Format(@"https://so.cnyes.com/JavascriptGraphic/chartstudy.aspx?country=tw&market=tw&code={0}&divwidth=990&divheight=330", Stock.Id.ToString()));
			label1.Text = string.Format("{0} {1} {2}/{3}", Stock.Id, Stock.Name, iStock, Stocks.Count());
			label2.Text = GetStockType();
		}

		private string GetStockType()
		{
			string type = string.Empty;
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT type FROM Stocks WHERE id = '{0}'", Stock.Id);
			var obj = sqlite_cmd.ExecuteScalar();
			if (obj != null)
			{
				switch (obj.ToString())
				{
					case "2":
						type = "盤整向上";
						break;
					case "3":
						type = "強勢股";
						break;
					case "4":
						type = "弱勢股";
						break;
				}
			}
			sqlite_conn.Close();
			return type;
		}

		/// <summary>
		/// 強勢股
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button3_Click(object sender, EventArgs e)
		{
			SaveStock("3");
		}

		private void button4_Click(object sender, EventArgs e)
		{
			SaveStock("4");
		}

		private void button5_Click(object sender, EventArgs e)
		{
			SaveStock("2");
		}

		private void SaveStock(string type)
		{
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Stocks WHERE id = '{0}'", Stock.Id);
			if (sqlite_cmd.ExecuteScalar() != null)
			{
				sqlite_cmd.CommandText = string.Format("update Stocks set type = '{2}' , modifyDate = {1} where id = '{0}'", Stock.Id, System.DateTime.Now.ToString("yyyy/MM/dd"), type);
				sqlite_cmd.ExecuteNonQuery();
			}
			else
			{
				sqlite_cmd.CommandText = string.Format("insert into Stocks (id, name, type, createDate) values ('{0}','{1}','{3}' ,{2})", Stock.Id, Stock.Name, System.DateTime.Now.ToString("yyyy/MM/dd"), type);
				sqlite_cmd.ExecuteNonQuery();
			}
			sqlite_conn.Close();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			iStock = int.Parse(textBox1.Text);
			iStock = iStock - 1;
			GoToWeb();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Stocks WHERE id = '{0}'", Stock.Id);
			if (sqlite_cmd.ExecuteScalar() != null)
			{
				sqlite_cmd.CommandText = string.Format("delete from Stocks where id = '{0}'", Stock.Id);
				sqlite_cmd.ExecuteNonQuery();
			}
			sqlite_conn.Close();
		}

		private List<Stock> GetStocks(string type)
		{
			List<Stock> stocks = new List<Stock>();
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Stocks WHERE type = '{0}'", type);
			SQLiteDataReader reader = sqlite_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			while (reader.Read())
			{
				Stock stock = new Stock()
				{
					Id = reader["id"].ToString(),
					Name = reader["name"].ToString()
				};
				stocks.Add(stock);
			}
			sqlite_conn.Close();
			return stocks;
		}

		private void button8_Click(object sender, EventArgs e)
		{
			iStock = -1;
			this.Stocks = GetStocks("3");
			InitListView();
			this.button1_Click(sender, e);
		}

		private void button9_Click(object sender, EventArgs e)
		{
			iStock = -1;
			this.Stocks = GetStocks("4");
			InitListView();
			this.button1_Click(sender, e);
		}

		private void button10_Click(object sender, EventArgs e)
		{
			iStock = -1;
			this.Stocks = GetStocks("2");
			InitListView();
			this.button1_Click(sender, e);
		}

		/// <summary>
		/// 線形過濾按鈕
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button11_Click(object sender, EventArgs e)
		{
			IFilterRule rule = new UpMaFilter();
			this.Stocks = rule.Filter(this.Stocks);
			this.Stocks = new UpPriceBuyFilter().Filter(this.Stocks);
			InitListView();
		}
		

		public void InitListView()
		{
			listView1.Items.Clear();
			foreach (var s in Stocks)
			{
				//輸入資料
				string type = GetStockType(s.Id); ;
				ListViewItem item = new ListViewItem(s.Id + s.Name + " " + type); //ID   
				if (type == "強勢股")
				{
					item.BackColor = Color.Pink;
				}
				else if (type == "強勢股")
				{
					item.BackColor = Color.LightGreen;
				}
				else if (type == "盤整向上")
				{
					item.BackColor = Color.Yellow;
				}
				listView1.Items.Add(item);

			}
			//自動調整欄位大小
			//listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private string GetStockType(string stockId)
		{
			string type = string.Empty;
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT type FROM Stocks WHERE id = '{0}'", stockId);
			var obj = sqlite_cmd.ExecuteScalar();
			if (obj != null)
			{
				switch (obj.ToString())
				{
					case "2":
						type = "盤整向上";
						break;
					case "3":
						type = "強勢股";
						break;
					case "4":
						type = "弱勢股";
						break;
				}
			}
			sqlite_conn.Close();
			return type;
		}
	}
}
