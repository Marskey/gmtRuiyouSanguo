using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gmt
{
	/// <summary>
	/// 渠道商
	/// </summary>
	public class Channel
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="id">编号</param>
		/// <param name="name">名称</param>
		/// <param name="prefix">前缀</param>
		/// <param name="numberCount">数字个数</param>
		/// <param name="charCount">数字字母个数</param>
		/// <param name="giftChar">礼包字符</param>
		public Channel(int id, string name, string prefix, byte numberCount, byte charCount, char giftChar)
		{
			this.Id = id;
			this.Name = name;
			this.Prefix = prefix;
			this.NumberCount = numberCount;
			this.CharCount = charCount;
			this.GiftChar = giftChar;
		}

		/// <summary>
		/// 编号
		/// </summary>
		public int Id { private set; get; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { private set; get; }

		/// <summary>
		/// 前缀
		/// </summary>
		public string Prefix { private set; get; }

		/// <summary>
		/// 数字个数
		/// </summary>
		public byte NumberCount { private set; get; }

		/// <summary>
		/// 数字字母个数
		/// </summary>
		public byte CharCount { private set; get; }

		/// <summary>
		/// 礼包字符
		/// </summary>
		public char GiftChar { private set; get; }
	}
}