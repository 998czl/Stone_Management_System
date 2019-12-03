using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity
{
	public class SqlSugarHelper
	{
		/// <summary>
		/// CreateDb
		/// </summary>
		/// <returns></returns>
		internal static SqlSugarClient CreateDb()
		{
			var client = new SqlSugarClient(DbHelper.ConnectionConfig);
			client.Ado.IsEnableLogEvent = true;
			return client;
		}
	}
}
