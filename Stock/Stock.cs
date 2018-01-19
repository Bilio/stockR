using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock
{
	public class Stock
	{
		/// <summary>
		/// 股票代碼
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 股票名稱
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 收盤價
		/// </summary>
		public float Price { get; set; }

		/// <summary>
		/// 最高
		/// </summary>
		public float High { get; set; }

		/// <summary>
		/// 最低
		/// </summary>
		public float Low { get; set; }

		/// <summary>
		/// 量
		/// </summary>
		public float Vol { get; set; }
	}
}
