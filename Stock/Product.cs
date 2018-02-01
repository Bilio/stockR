using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock
{
	/// <summary>
	/// 買進賣出
	/// </summary>
	public class Product
	{
		public int Seq { get; set; }

		public DateTime Date { get; set; }

		/// <summary>
		/// 股票代碼
		/// </summary>
		public string Id { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// B:買進
		/// S:賣出
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// 0:未結案
		/// 1:結案
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// 成交價
		/// </summary>
		public float Price { get; set; }
		
		/// <summary>
		/// 股數
		/// </summary>
		public int Vol { get; set; }

		/// <summary>
		/// 手續費
		/// </summary>
		public int Fee { get; set; }

		/// <summary>
		/// 交易稅
		/// </summary>
		public int Tax { get; set; }

		/// <summary>
		/// 竟收復
		/// </summary>
		public int Income { get; set; }

		/// <summary>
		/// 損益
		/// </summary>
		public int Increase { get; set; }

		/// <summary>
		/// 報酬率
		/// </summary>
		public float Rate { get; set; }
	}
}
