using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RDotNet;

namespace Stock.FilterRule
{
	/// <summary>
	/// 均線+價錢一次搞定
	/// </summary>
	public class UpMaPriceBuyFilter : IFilterRule
	{
		string today = System.DateTime.Today.ToString("yyyyMM01");
		string monthAgo = System.DateTime.Today.AddMonths(-1).ToString("yyyyMM01");
		public IEnumerable<Stock> Filter(IEnumerable<Stock> stocks)
		{
			string orcode = @"library(tidyverse)
			library(httr)
			library(jsonlite)
			library(quantmod)
			dataLastMonth <- fromJSON('http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date={1}&stockNo={0}')
			stockLastMonth <-dataLastMonth$data
			dataThisMonth <- fromJSON('http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date={2}&stockNo={0}')
			stockThisMonth <- dataThisMonth$data
			stock <- rbind(stockLastMonth, stockThisMonth)
			ma5 <- runMean(as.numeric(stock[, 7]), n = 5)
			ma10 <- runMean(as.numeric(stock[, 7]), n = 10)
			ma20 <-runMean(as.numeric(stock[, 7]), n = 20)
			price <- last(as.numeric(stock[,7]))
			c1 <- ifelse(last(ma5) > last(ma10) & last(ma10) > last(ma20), 1, 0)
			c2 <- ifelse(c1 == 1 & last(ma5) > price & last(ma20) < price, 1, 0)
			c2";
			List<Stock> okList = new List<Stock>();

			
			foreach (var s in stocks)
			{
				try
				{
					REngine engine = REngine.GetInstance();
					engine.Initialize();
					string rcode = string.Format(orcode, s.Id, monthAgo, today );
					//engine.Evaluate("library(tidyverse)");
					//engine.Evaluate("library(httr)");
					//engine.Evaluate("library(jsonlite)");
					//engine.Evaluate("library(quantmod)");
					//engine.Evaluate(string.Format("dataLastMonth <- fromJSON('http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date={1}&stockNo={0}')",s.Id,monthAgo));
					//engine.Evaluate("stockLastMonth < -dataLastMonth$data");
					//engine.Evaluate(string.Format("dataLastMonth <- fromJSON('http://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date={1}&stockNo={0}')", s.Id, today));
					//engine.Evaluate("stockThisMonth < -dataThisMonth$data");
					//engine.Evaluate("stock < -rbind(stockLastMonth, stockThisMonth)");
					//engine.Evaluate("ma5 <- runMean(as.numeric(stock[, 7]), n = 5)");
					//engine.Evaluate("ma10 <- runMean(as.numeric(stock[, 7]), n = 10)");
					//engine.Evaluate("ma20 <-runMean(as.numeric(stock[, 7]), n = 20)");
					//engine.Evaluate("ma60 <- runMean(as.numeric(stock[, 7]), n = 60)");
					//engine.Evaluate("price <- last(as.numeric(stock[,7]))");
					//engine.Evaluate("c1 <- ifelse(last(ma5) > last(ma10) & last(ma10) > last(ma20) & last(ma20) > last(ma60), 1, 0)");
					//engine.Evaluate("c2 <- ifelse(c1 == 1 & last(ma5) > price & last(ma20) < price, 1, 0)");
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
				Thread.Sleep(2 * 60 * 1000);
			}

			return okList;
		}
	}
}
