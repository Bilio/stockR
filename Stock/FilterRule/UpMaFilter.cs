using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDotNet;

namespace Stock.FilterRule
{
	/// <summary>
	/// 強勢股均線判斷過濾
	/// </summary>
	public class UpMaFilter : IFilterRule
	{
		public IEnumerable<Stock> Filter(IEnumerable<Stock> stocks)
		{
			string today = System.DateTime.Today.ToString("yyyy-MM-dd");
			string yearAgo = System.DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
			string orcode = @"library(quantmod)
			stock <- getSymbols('{0}.TW', auto.assign = FALSE,from='{1}')
			stock <- na.omit(stock)
			ma5 <- runMean(stock[, 4], n = 5)
			ma10 <- runMean(stock[, 4], n = 10)
			ma20 <-runMean(stock[, 4], n = 20)
			ma60 <- runMean(stock[, 4], n = 60)
			c2 <- ifelse(last(ma5) > last(ma10) & last(ma10) > last(ma20) & last(ma20) > last(ma60), 1, 0)
			c2";
			List<Stock> okList = new List<Stock>();
			foreach (var s in stocks)
			{
				try
				{
					REngine engine = REngine.GetInstance();
					engine.Initialize();
					string rcode = string.Format(orcode, s.Id, "2017-01-03", "2018-01-25");
					var a = engine.Evaluate(rcode).AsCharacter().ToArray();
					engine.Evaluate("dev.off");
					if (a[0] == "1")
					{
						okList.Add(s);
					}
				}
				catch (Exception e2)
				{

				}
			}
			return okList;
		}
	}
}
