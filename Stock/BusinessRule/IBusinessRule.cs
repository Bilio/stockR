using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessRule
{
	public interface IBusinessRule
	{
		/// <summary>
		/// 取得符合條件股票
		/// </summary>
		/// <returns></returns>
		IEnumerable<Stock> GetStocks();

		/// <summary>
		/// 已買進股票
		/// </summary>
		/// <returns></returns>
		IEnumerable<Stock> GetBoughtStocks();

		void Buy();

		void Sale();
	}
}
