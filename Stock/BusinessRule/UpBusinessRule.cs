using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stock.FilterRule;

namespace Stock.BusinessRule
{
	public class UpBusinessRule : CommonBusinessRule, IBusinessRule
	{
		public IEnumerable<Product> Buy()
		{
			List<Product> buys = new List<Product>();
			foreach (var s in GetStocks()) {
				Product b = new Product();
				b.Id = s.Id;
				b.Name = s.Name;
				b.Price = s.Price;
				b.Vol = 1;
				b.Date = System.DateTime.Now.ToString("yyyy/MM/dd"); ;
				Buy(b);
				buys.Add(b);
			}
			return buys;
		}

		public IEnumerable<Product> Buy(DateTime today)
		{
			List<Product> buys = new List<Product>();
			foreach (var s in GetStocks(today))
			{
				Product b = new Product();
				b.Id = s.Id;
				b.Name = s.Name;
				b.Price = s.Price;
				b.Vol = 1;
				b.Date = System.DateTime.Now.ToString("yyyy/MM/dd"); ;
				Buy(b);
				buys.Add(b);
			}
			return buys;
		}



		public IEnumerable<Stock> GetBoughtStocks()
		{
			var ups2 = GetBoughtStocks("2");
			var ups3 = GetBoughtStocks("3");
			return ups3.Concat(ups2);
		}

		public IEnumerable<Stock> GetStocks()
		{
			IFilterRule upRule = new UpMaPriceBuyFilter();
			var ups = upRule.Filter(GetStocks("3"));
			var ups2 = upRule.Filter(GetStocks("2"));
			var upAll = ups.Concat(ups2);
			return upAll;
		}

		public IEnumerable<Stock> GetStocks(DateTime today)
		{
			UpMaPriceBuyFilter upRule = new UpMaPriceBuyFilter();
			var ups = upRule.Filter(GetStocks("3"), today);
			var ups2 = upRule.Filter(GetStocks("2"), today);
			var upAll = ups.Concat(ups2);
			return upAll;
		}

		public IEnumerable<Product> Sale()
		{
			List<Product> sales = new List<Product>();
			IFilterRule upRule = new UpSaleFilter();
			foreach (var s in upRule.Filter(GetBoughtStocks())) {
				Product b = new Product();
				b.Id = s.Id;
				b.Name = s.Name;
				b.Price = s.Price;
				b.Vol = int.Parse(s.Vol.ToString());
				b.Date = System.DateTime.Now.ToString("yyyy/MM/dd"); 
				Sale(b);
				sales.Add(b);
			}
			return sales;
		}

		public IEnumerable<Product> Sale(DateTime today)
		{
			List<Product> sales = new List<Product>();
			UpSaleFilter upRule = new UpSaleFilter();
			foreach (var s in upRule.Filter(GetBoughtStocks(), today))
			{
				Product b = new Product();
				b.Id = s.Id;
				b.Name = s.Name;
				b.Price = s.Price;
				b.Vol = int.Parse(s.Vol.ToString());
				b.Date = System.DateTime.Now.ToString("yyyy/MM/dd");
				Sale(b);
				sales.Add(b);
			}
			return sales;
		}

	}
}
