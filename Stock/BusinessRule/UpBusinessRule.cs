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
		public void Buy()
		{
			foreach (var s in GetStocks()) {
				Product b = new Product();
				b.Id = s.Id;
				b.Price = s.Price;
				b.Vol = 1;
				b.Date = System.DateTime.Now;
				Buy(b);
			}
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

		public void Sale()
		{
			IFilterRule upRule = new UpSaleFilter();
			foreach (var s in upRule.Filter(GetBoughtStocks())) {
				Product b = new Product();
				b.Id = s.Id;
				b.Price = s.Price;
				b.Vol = 1;
				b.Date = System.DateTime.Now;
				Sale(b);
			}
		}
	}
}
