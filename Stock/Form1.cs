using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stock.BusinessRule;
using Stock.Download;

namespace Stock
{
	public partial class Form1 : Form
	{
		public IEnumerable<Stock> Stocks { get; set; }

		private int iStock = -1;

		public Stock Stock {
			get {
				return Stocks.ToList()[iStock];
			}
		}
		//private SQLiteConnection sqlite_conn;
		//private SQLiteCommand sqlite_cmd;


		private SqlConnection sqlConn;
		public Form1()
		{
			string strConn = "server=durantw.database.windows.net;database=Stock;User ID=duranhsieh;Password=Aa@123456;Trusted_Connection=False;Encrypt=True;";

			sqlConn = new SqlConnection(strConn);
			// Open
			//sqlConn.Open();
			//if (!TableExists("Stocks", sqlite_conn)){ 
			//// 要下任何命令先取得該連結的執行命令物件
			//sqlite_cmd = sqlite_conn.CreateCommand();

			//// 要下的命令新增一個表
			//sqlite_cmd.CommandText = "CREATE Table Stocks  (id varchar(7) primary key, name nvarchar(10), type varchar(1), createDate datetime, modifyDate datetime);";


			//sqlite_cmd.ExecuteNonQuery();
			//}
			//sqlite_conn.Close();
			InitializeComponent();
		}

		public void InitListView() {
			listView1.Items.Clear();
			foreach (var s in Stocks) {
				//輸入資料
				string type = GetStockType(s.Id); ;
				ListViewItem item = new ListViewItem(s.Id + s.Name + " " + type); //ID   
				if (type == "強勢股") {
					item.BackColor = Color.Pink;
				}else if(type == "強勢股") {
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

		private void GoToWeb()
		{
			webBrowser1.Url = new Uri(string.Format(@"https://so.cnyes.com/JavascriptGraphic/chartstudy.aspx?country=tw&market=tw&code={0}&divwidth=990&divheight=330", Stock.Id.ToString()));
			label1.Text = string.Format("{0} {1} {2}/{3}", Stock.Id, Stock.Name, iStock, Stocks.Count()-1);
			label2.Text = GetStockType();
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
			if (iStock < (Stocks.Count() - 1)) {
				iStock = iStock + 1;
			}
			GoToWeb();
		}

		/// <summary>
		/// 上一個
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			if (iStock > 0)
			{
				iStock = iStock - 1;
			}
			GoToWeb();
		}

		private string GetStockType(string stockId) {
			string type = string.Empty;
			sqlConn.Open();
			var sqlite_cmd = sqlConn.CreateCommand();
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
			sqlConn.Close();
			return type;
		}

		private string GetStockType() {
			return GetStockType(Stock.Id);
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

		private void SaveStock(string type) {
			sqlConn.Open();
			var sqlite_cmd = sqlConn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Stocks WHERE id = '{0}'", Stock.Id);
			if (sqlite_cmd.ExecuteScalar() != null)
			{
				sqlite_cmd.CommandText = string.Format("update Stocks set type = '{2}' , modifyDate = '{1}' where id = '{0}'", Stock.Id, System.DateTime.Now.ToString("yyyy/MM/dd"), type);
				sqlite_cmd.ExecuteNonQuery();
			}
			else
			{
				sqlite_cmd.CommandText = string.Format("insert into Stocks (id, name, type, createDate, stockType) values ('{0}',N'{1}','{2}' ,'{3}','{4}')", Stock.Id, Stock.Name, type, System.DateTime.Now.ToString("yyyy/MM/dd"),  this.Stock.stockType);
				sqlite_cmd.ExecuteNonQuery();
			}
			sqlConn.Close();
		}

		private void button6_Click(object sender, EventArgs e)
		{
			iStock = int.Parse(textBox1.Text);
			GoToWeb();
		}

		private void button7_Click(object sender, EventArgs e)
		{
			sqlConn.Open();
			var sqlite_cmd = sqlConn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Stocks WHERE id = '{0}'", Stock.Id);
			if (sqlite_cmd.ExecuteScalar() != null)
			{
				sqlite_cmd.CommandText = string.Format("delete from Stocks where id = '{0}'", Stock.Id);
				sqlite_cmd.ExecuteNonQuery();
			}
			sqlConn.Close();
		}

		private List<Stock> GetStocks(string type)
		{
			List<Stock> stocks = new List<Stock>();
			sqlConn.Open();
			var sqlite_cmd = sqlConn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Stocks WHERE type = '{0}'", type);
			SqlDataReader reader  = sqlite_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			while (reader.Read())
			{
				Stock stock = new Stock() {
					Id = reader["id"].ToString(),
					Name = reader["name"].ToString(),
					stockType = reader["stockType"].ToString()
				};
				stocks.Add(stock);
			}
			sqlConn.Close();
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

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
		

		private void button12_Click(object sender, EventArgs e)
		{
			IEnumerable<Stock> stocks = new List<Stock>();
			YahooDownload yd = new YahooDownload();
			yd.LowPrice = int.Parse(textBox2.Text);
			yd.HighPrice = int.Parse(textBox3.Text); ;
			yd.LowVol = int.Parse(textBox4.Text); ;
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=up&e=tse&n=100";
			yd.stockType = "1";
			stocks = yd.GetStocks();
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=down&e=tse&n=100";
			yd.stockType = "1";
			stocks = stocks.Concat(yd.GetStocks());
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=up&e=otc&n=100";
			yd.stockType = "2";
			stocks = stocks.Concat(yd.GetStocks());
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=down&e=otc&n=100";
			yd.stockType = "2";
			stocks = stocks.Concat(yd.GetStocks());
			this.Stocks = stocks;
			this.iStock = -1;
			InitListView();
			button1_Click(sender, e);
		}
	}
}
