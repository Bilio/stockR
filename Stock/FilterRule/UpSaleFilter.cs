using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace Stock.FilterRule
{
	public class UpSaleFilter : IFilterRule
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
			c2 <- ifelse(last(ma20) > price, 1, 0)
			c2";
			List<Stock> okList = new List<Stock>();
			float stopUp = 0.15f;
			foreach (var s in stocks)
			{
				try
				{
					string type = s.stockType == "1" ? "TW" : "TWO";
					REngine engine = REngine.GetInstance();
					engine.Initialize();
					string rcode = string.Format(orcode, s.Id, yearAgo, type);
					var a = engine.Evaluate(rcode).AsCharacter().ToArray();
					var price = engine.Evaluate("price").AsCharacter().ToArray();
					float nowPrice = float.Parse(price[0].ToString()); ;
					if (a[0] == "1")
					{
						s.Price = nowPrice;
						okList.Add(s);
					}
					else if(nowPrice >= (s.Price * (1+ stopUp))){
						s.Price = nowPrice;
						okList.Add(s);
					}
					engine.Evaluate("dev.off");
				}
				catch (Exception e2)
				{
					System.Console.WriteLine("lost sale:"+s.Id);
				}
			}
			return okList;
		}
	}
}
