using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Stock.Download
{
	public class YahooDownload : IGetStockData
	{
		public string Url { get; set; }

		public string stockType { get; set; }

		public float? HighPrice { get; set; }
		public float? LowPrice { get; set; }

		public float? HighVol { get; set; }
		public float? LowVol { get; set; }
		public IEnumerable<Stock> GetStocks()
		{
			List<Stock> stocks = new List<Stock>();
			var web = new HtmlWeb
			{
				AutoDetectEncoding = false,
				OverrideEncoding = Encoding.Default,
			};
			HtmlDocument doc = web.Load(Url);
			var tr = doc.DocumentNode.SelectNodes("//tr[@bgcolor=\"#ffffff\"]");
			foreach (var data in tr) {
				string[] values = data.InnerText.Split('\n');
				string[] ids = values[2].Split(' ');
				Stock stock = new Stock()
				{
					Id = ids[0],
					Name = ids[1],
					Price = float.Parse(values[3]),
					High = float.Parse(values[6]),
					Low = float.Parse(values[7]),
					Vol = float.Parse(values[9].Replace(",",string.Empty)),
					stockType = this.stockType
				};
				stocks.Add(stock);
			}
			if (LowPrice != null) {
				stocks = stocks.Where(x=>x.Price >= LowPrice).ToList();
			}
			if (HighPrice != null)
			{
				stocks = stocks.Where(x => x.Price <= HighPrice).ToList();
			}
			if (LowVol != null)
			{
				stocks = stocks.Where(x => x.Vol >= LowVol).ToList();
			}
			if (HighVol != null)
			{
				stocks = stocks.Where(x => x.Vol <= HighVol).ToList();
			}
			return stocks;
		}
	}
}
