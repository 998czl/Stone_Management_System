using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity
{
	public class Setting
	{
		/// <summary>
		/// 数据库连接字符串
		/// </summary>
		public static string loginImg => JsonConfigHelper.Settngs["loginImg"];
		
	}
}
