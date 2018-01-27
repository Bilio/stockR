using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stock.Download;

namespace Stock
{
	static class Program
	{
		/// <summary>
		/// 應用程式的主要進入點。
		/// 收集資料:
		///		蒐集百大漲跌排行中30~100，成交量1000以上資料
		///	分析資料:
		///		將資料分類成強勢股、弱勢股、平盤向上三種
		///	篩選強勢股:
		///		股價 大於 ma20
		///		ma5 > ma10 > ma20 > ma 60
		///		股價 小於 ma5
		/// </summary>
		[STAThread]
		static void Main()
		{
			IEnumerable<Stock> stocks = new List<Stock>();
			YahooDownload yd = new YahooDownload();
			yd.LowPrice = 30;
			yd.HighPrice = 100;
			yd.LowVol = 5000;
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=up&e=tse&n=100";
			stocks = yd.GetStocks();
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=down&e=tse&n=100";
			stocks = stocks.Concat(yd.GetStocks());
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=up&e=otc&n=100";
			stocks = stocks.Concat(yd.GetStocks());
			yd.Url = @"https://tw.stock.yahoo.com/d/i/rank.php?t=down&e=otc&n=100";
			stocks = stocks.Concat(yd.GetStocks());
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			var form = new FormR();
			form.Stocks = stocks;
			form.InitListView();
			Application.Run(form);
			//var form = new Form1();
			//form.Stocks = stocks;
			//form.InitListView();
			//Application.Run(form);
		}
	}
}
