using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore_Utils;

namespace WebCore_Entity.RedisManager
{
	public class RedisHashCache
	{
		private IDatabase redis => DbRedis.CacheDb;

		/// <summary>
		/// 向缓存hash表中添加一个对象
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <param name="field">缓存的列名</param>
		/// <param name="value">缓存的对象</param>
		public void Add<T>(string key, string field, T value) where T : class
		{
			IDatabase db = redis;
			db.HashSet(key, field, JsonHelper.SerializeObject(value));
		}

		/// <summary>
		/// 获取一个<see cref="bool"/>值，该值表示拥有指定键值和列名的缓存是否存在
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <param name="field">缓存的列名</param>
		/// <returns>如果缓存存在，则返回true，否则返回false</returns>
		public bool Exists(string key, string field)
		{
			IDatabase db = redis;
			return db.HashExists(key, field);
		}

		/// <summary>
		/// 获取一个<see cref="bool"/>值，该值表示拥有指定键值的缓存是否存在
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <returns>如果缓存存在，则返回true，否则返回false</returns>
		public bool Exists(string key)
		{
			IDatabase db = redis;
			return db.KeyExists(key);
		}

		/// <summary>
		/// 从缓存hash表中读取对象
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <param name="field">缓存的列名</param>
		/// <returns>缓存的对象</returns>
		public T Get<T>(string key, string field) where T : class
		{
			IDatabase db = redis;
			return JsonHelper.DeserializeObject<T>(db.HashGet(key, field));
		}

		/// <summary>
		/// 从缓存中读取hash表
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <returns>缓存hash表中所有数据的集合</returns>
		public List<T> GetAll<T>(string key) where T : class
		{
			IDatabase db = redis;
			var values = db.HashValues(key);

			List<T> list = new List<T>();
			foreach (var item in values)
			{
				list.Add(JsonHelper.DeserializeObject<T>(item));
			}
			return list;
		}

		/// <summary>
		/// 向缓存hash表中更新一个对象
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <param name="field">缓存的列名</param>
		/// <param name="value">缓存的对象</param>
		public void Put<T>(string key, string field, T value) where T : class
		{
			Add<T>(key, field, value);
		}

		/// <summary>
		/// 从缓存hash表中移除对象
		/// </summary>
		/// <param name="key">缓存的键值</param>
		/// <param name="field">缓存的列名</param>
		public void Remove(string key, string field)
		{
			IDatabase db = redis;
			db.HashDelete(key, field);
		}

		/// <summary>
		/// 从缓存中移除对象
		/// </summary>
		/// <param name="key">缓存的键值</param>
		public void RemoveAll(string key)
		{
			IDatabase db = redis;
			db.KeyDelete(key);
		}
	}
}
