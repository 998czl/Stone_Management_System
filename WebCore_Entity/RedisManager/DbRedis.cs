using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity.RedisManager
{
	public class DbRedis
	{		
		/// <summary>
		/// _CacheDb
		/// </summary>
		private static IDatabase _CacheDb;

		/// <summary>
		/// 缓存数据库
		/// </summary>
		public static IDatabase CacheDb
		{
			get
			{
				if (_CacheDb == null)
				{
					_CacheDb = RedisHelper.Database;
				}
				return _CacheDb;
			}
		}

		
	}
}
