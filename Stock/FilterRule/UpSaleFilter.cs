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
		string today = System.DateTime.Today.ToString("yyyyMM01");
		string monthAgo = System.DateTime.Today.AddMonths(-1).ToString("yyyyMM01");
		public IEnumerable<Stock> Filter(IEnumerable<Stock> stocks)
		{
			string orcode = @"library(tidyverse)
			library(httr)
			library(jsonlite)
			library(quantmod)
			dataLastMonth <- fromJSON('http://www.twse.com.tw/exchange/exchangeReport/STOCK_DAY?response=json&date={1}&stockNo={0}')
			stockLastMonth < -dataLastMonth$data
			dataThisMonth < -fromJSON('http://www.twse.com.tw/exchange/exchangeReport/STOCK_DAY?response=json&date={2}&stockNo={0}')
			stockThisMonth < -dataThisMonth$data
			stock < -rbind(stockLastMonth, stockThisMonth)
			ma5 <- runMean(as.numeric(stock[, 7]), n = 5)
			ma10 <- runMean(as.numeric(stock[, 7]), n = 10)
			ma20 <-runMean(as.numeric(stock[, 7]), n = 20)
			ma60 <- runMean(as.numeric(stock[, 7]), n = 60)
			price <- last(as.numeric(stock[,7]))
			c2 <- ifelse(last(ma20) > price, 1, 0)
			c2";
			List<Stock> okList = new List<Stock>();
			float stopUp = 0.15f;
			foreach (var s in stocks)
			{
				try
				{
					REngine engine = REngine.GetInstance();
					engine.Initialize();
					string rcode = string.Format(orcode, s.Id, today, monthAgo);
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
