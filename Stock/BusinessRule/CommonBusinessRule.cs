using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessRule
{
	public class CommonBusinessRule
	{
		private SQLiteConnection sqlite_conn;
		private SQLiteCommand sqlite_cmd;

		public CommonBusinessRule()
		{
			sqlite_conn = new SQLiteConnection("Data source=database.db");
			// Open
			sqlite_conn.Open();
			if (!TableExists("Business", sqlite_conn))
			{
				// 要下任何命令先取得該連結的執行命令物件
				sqlite_cmd = sqlite_conn.CreateCommand();

				// 要下的命令新增一個表
				sqlite_cmd.CommandText = @"CREATE TABLE [Business] (
					  [seq] INTEGER NOT NULL
					, [id] nvarchar(7)  NULL
					, [date] nvarchar(10)  NULL
					, [type] nvarchar(1)  NULL
					, [status] nvarchar(1)  NULL
					, [price] float NULL
					, [vol] int  NULL
					, [fee] int  NULL
					, [tax] int  NULL
					, [income] int  NULL
					, [increase] int  NULL
					, [rate] float NULL
					, [modifyDate] nvarchar(10)  NULL
					, CONSTRAINT [sqlite_autoindex_Business_1] PRIMARY KEY ([seq])
					);";


				sqlite_cmd.ExecuteNonQuery();
			}
			sqlite_conn.Close();
		}

		public static bool TableExists(String tableName, SQLiteConnection connection)
		{
			SQLiteCommand cmd = connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
			cmd.Parameters.Add("@name", DbType.String).Value = tableName;
			return (cmd.ExecuteScalar() != null);
		}
		public IEnumerable<Product> QueryProducts(DateTime startTime, DateTime endTime) {
			List<Product> products = new List<Product>();
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("select a.*,b.name from business a inner join stocks b on a.id = b.id where date between '{0}' and '{1}'", startTime.ToString("yyyy/MM/dd"),endTime.ToString("yyyy/MM/dd"));
			SQLiteDataReader reader = sqlite_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			while (reader.Read())
			{
				Product p = new Product()
				{
					Id = reader["id"].ToString(),
					Name = reader["name"].ToString(),
					Price = float.Parse(reader["price"].ToString()),
					Date = reader["date"].ToString(),
					Type = reader["type"].ToString(),
					Vol = int.Parse(reader["vol"].ToString()),
					Fee = int.Parse(reader["fee"].ToString()),
					Tax = int.Parse(reader["tax"].ToString()),
					Income = int.Parse(reader["income"].ToString()),
					Increase = int.Parse(reader["increase"].ToString()),
					Rate = float.Parse(reader["rate"].ToString()),
					ModifyDate = reader["modifyDate"].ToString()
				};
				products.Add(p);
			}
			sqlite_conn.Close();
			return products;
		}

		public IEnumerable<Stock> GetStocks(string type)
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
					Name = reader["name"].ToString(),
					stockType = reader["stockType"].ToString()
				};
				stocks.Add(stock);
			}
			sqlite_conn.Close();
			return stocks;
		}

		/// <summary>
		/// 已買進股票
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Stock> GetBoughtStocks(string type)
		{
			List<Stock> stocks = new List<Stock>();
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT distinct b.*, a.price as buyPrice " +
				"FROM Business a " +
				"inner join Stocks b " +
				"on a.id = b.id " +
				"WHERE b.type = '{0}' and a.type = 'B' and a.status = '0' ", type);
			SQLiteDataReader reader = sqlite_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			while (reader.Read())
			{
				Stock stock = new Stock()
				{
					Id = reader["id"].ToString(),
					Name = reader["name"].ToString(),
					Price = float.Parse(reader["buyPrice"].ToString()),
					stockType = reader["stockType"].ToString()
				};
				stocks.Add(stock);
			}
			sqlite_conn.Close();
			return stocks;
		}

		public int GetFee(Product stock) {
			double fee = Math.Round(stock.Vol * 1000 * stock.Price * 0.001425);
			if (fee < 20) {
				return 20;
			}
			return Convert.ToInt32(fee);
		}

		public int GetTax(Product stock)
		{
			double tax = Math.Round(stock.Vol * 1000 * stock.Price * 0.003);
			if (tax < 20)
			{
				return 20;
			}
			return Convert.ToInt32(tax);
		}

		/// <summary>
		/// 寫入買進
		/// </summary>
		/// <param name="stock"></param>
		public void Buy(Product stock) {
			stock.Type = "B";
			stock.Status = "0";
			stock.Fee = GetFee(stock);
			stock.Date = System.DateTime.Now.ToString("yyyy/MM/dd");
			stock.Income = Convert.ToInt32(stock.Vol * 1000 * stock.Price) + stock.Fee;
			Save(stock);
		}

		public void Sale(Product stock)
		{
			int buyCount = 0;
			int allBuyIncome = 0;
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("SELECT * FROM Business WHERE id = '{0}' and type = 'B' and status = '0' ", stock.Id);
			SQLiteDataReader reader = sqlite_cmd.ExecuteReader(CommandBehavior.CloseConnection);
			while (reader.Read() && buyCount <= stock.Vol)
			{
				int seq = int.Parse(reader["seq"].ToString());
				allBuyIncome += int.Parse(reader["income"].ToString());
				SQLiteCommand sqlite_cmdSale = sqlite_conn.CreateCommand();
				sqlite_cmdSale.CommandText = string.Format("update Business set status = '1', modifyDate = '{1}' where seq = {0}", seq, System.DateTime.Now.ToString("yyyy/MM/dd"));
				sqlite_cmdSale.ExecuteNonQuery();
				buyCount = buyCount + 1;
			}
			sqlite_conn.Close();

			stock.Type = "S";
			stock.Status = "1";
			stock.Fee = GetFee(stock);
			stock.Tax = GetTax(stock);
			stock.Date = System.DateTime.Now.ToString("yyyy/MM/dd"); ;
			stock.Income = Convert.ToInt32(stock.Vol * 1000 * stock.Price) - stock.Fee - stock.Tax;
			stock.Increase = stock.Income - allBuyIncome;
			stock.Rate = stock.Increase / allBuyIncome;
			Save(stock);
		}

		public void Save(Product stock) {
			sqlite_conn.Open();
			sqlite_cmd = sqlite_conn.CreateCommand();
			sqlite_cmd.CommandText = string.Format("insert into Business (id, date, type, price,vol,fee,tax,status,income,rate,modifyDate) " +
				"values ('{0}','{1}','{2}' ,{3}, {4}, {5}, {6}, '{7}',{8},{9},'{10}')",
				stock.Id, stock.Date, stock.Type, stock.Price, stock.Vol, stock.Fee, stock.Tax, stock.Status, stock.Income, stock.Rate,System.DateTime.Now.ToString("yyyy/MM/dd"));
			sqlite_cmd.ExecuteNonQuery();
			sqlite_conn.Close();
		}
	}
}
