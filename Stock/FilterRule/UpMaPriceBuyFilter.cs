using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace Stock.FilterRule
{
	/// <summary>
	/// 均線+價錢一次搞定
	/// </summary>
	public class UpMaPriceBuyFilter : IFilterRule
	{
		string today = System.DateTime.Today.ToString("yyyy-MM-dd");
		string yearAgo = System.DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
		public IEnumerable<Stock> Filter(IEnumerable<Stock> stocks)
		{
			string today = System.DateTime.Today.ToString("yyyy-MM-dd");
			string yearAgo = System.DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
			string orcode = @"library(quantmod)
			stock <- getSymbols('{0}.{2}', auto.assign = FALSE,from='{1}')
			ind <- apply(stock, 1, function(x) all(is.na(x)))
			stock <- stock[!ind,]
			ma5 <- runMean(stock[, 4], n = 5)
			ma10 <- runMean(stock[, 4], n = 10)
			ma20 <-runMean(stock[, 4], n = 20)
			ma60 <- runMean(stock[, 4], n = 60)
			price <- last(stock[,4])
			c1 <- ifelse(last(ma5) > last(ma10) & last(ma10) > last(ma20) & last(ma20) > last(ma60), 1, 0)
			c2 <- ifelse(c1 == 1 & last(ma5) > price & last(ma20) < price, 1, 0)";
			List<Stock> okList = new List<Stock>();

			
			foreach (var s in stocks)
			{
				try
				{
					string type = s.stockType == "1" ? "TW" : "TWO";
					REngine engine = REngine.GetInstance();
					engine.Initialize();
					string rcode = string.Format(orcode, s.Id, yearAgo, type);
					var a = engine.Evaluate(rcode).AsCharacter().ToArray();
					
					
					if (a[0] == "1")
					{
						var price = engine.Evaluate("price").AsCharacter().ToArray();
						s.Price = float.Parse(price[0].ToString());
						okList.Add(s);
					}
					engine.Evaluate("dev.off");

				}
				catch (Exception e2)
				{
					
				}
			}

			return okList;
		}
	}
}
