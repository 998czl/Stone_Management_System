using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity
{
	public static class DbHelper
	{
		/// <summary>
		/// 数据库连接字符串
		/// </summary>
		public static string ConnectionString => JsonConfigHelper.ConnectionStrings["MySQL"];

		/// <summary>
		/// ConnectionConfig
		/// </summary>
		internal static ConnectionConfig ConnectionConfig => new ConnectionConfig()
		{
			ConnectionString = ConnectionString,
			DbType = SqlSugar.DbType.MySql,
			//开启自动释放模式和EF原理一样
			IsAutoCloseConnection = true
		};

		/// <summary>
		/// 缓存的实体
		/// </summary>
		internal static string[] CacheModels
		{
			get
			{
				JsonConfigHelper.ConfigurationCollection.TryGetValue("CacheModel", out var obj);
				if (obj == null)
				{
					return new string[] { };
				}
				return obj.ToString().Split(',');
			}
		}

		/// <summary>
		/// 是否需要缓存的实体
		/// </summary>
		/// <param name="modelName"></param>
		/// <returns></returns>
		internal static bool IsCacheModel(string modelName)
		{
			return Array.IndexOf(CacheModels, modelName) > -1;
		}
	}
}
