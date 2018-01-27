using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.FilterRule
{
	/// <summary>
	/// 過濾條件
	/// </summary>
	public interface IFilterRule
	{
		IEnumerable<Stock> Filter(IEnumerable<Stock> stocks);
	}
}
