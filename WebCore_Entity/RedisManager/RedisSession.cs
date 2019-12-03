using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity.RedisManager
{
	public class RedisSession
	{
		private HttpContext context;
		private int timeout;
		private bool isReadOnly;
		private RedisCache redis;

		//SessionId标识符
		private static readonly string SESSION_NAME = "REDIS_SESSION_ID";

		public RedisSession(HttpContext context)
			: this(context, true, 20)
		{
		}

		public RedisSession(HttpContext context, bool isReadOnly, int timeout)
		{
			this.context = context;
			this.isReadOnly = isReadOnly;
			this.timeout = timeout;
			this.redis = new RedisCache();
		}

		/// <summary>
		/// 获取或设置一个值，该值指定Cookie是否可通过客户端脚本访问。
		/// </summary>
		public bool IsReadOnly
		{
			get { return isReadOnly; }
			set { isReadOnly = value; }
		}

		/// <summary>
		/// 获取会话的唯一标识符。
		/// </summary>
		public string SessionID
		{
			get { return GetSessionID(); }
		}

		/// <summary>
		/// 获取SessionID
		/// </summary>
		/// <param name="key">SessionId标识符</param>
		/// <returns>HttpCookie值</returns>
		private string GetSessionID()
		{
			var getCookie = "";
			context.Request.Cookies.TryGetValue(SESSION_NAME, out getCookie);
			if (string.IsNullOrEmpty(getCookie))
			{
				string newSessionID = Guid.NewGuid().ToString("N").ToUpper();
				context.Response.Cookies.Append(SESSION_NAME, newSessionID, new CookieOptions()
				{
					HttpOnly = isReadOnly,
					Expires = DateTime.Now.AddMinutes(timeout)
				});
				return newSessionID;
			}
			else
			{
				return getCookie;
			}
		}

		/// <summary>
		/// 获取一个带有前缀的键名
		/// </summary>
		/// <param name="name">会话状态的名称。</param>
		/// <returns>返回一个新的的键名</returns>
		private string GetPrefixKey(string name)
		{
			return string.Format("SESSION:{0}:{1}", name, SessionID);
		}

		/// <summary>
		/// 设置会话状态集合的过期时间
		/// </summary>
		/// <param name="timeout">会话状态过期时间（以分钟为单位）。</param>
		public void SetExpire(string name, int timeout)
		{
			var getCookie = "";
			context.Request.Cookies.TryGetValue(SESSION_NAME, out getCookie);
			if (getCookie != "")
			{
				context.Response.Cookies.Append(SESSION_NAME, getCookie, new CookieOptions()
				{
					HttpOnly = isReadOnly,
					Expires = DateTime.Now.AddMinutes(timeout)
				});
				string key = GetPrefixKey(name);
				redis.SetExpire(key, timeout * 60);
			}
		}

		/// <summary>
		/// 判断会话中是否存在指定键
		/// </summary>
		/// <param name="name">会话状态的键。</param>
		/// <returns>返回一个布尔值，成功为true,否则为false</returns>
		public bool Exists(string name)
		{
			string key = GetPrefixKey(name);
			return redis.Exists(key);
		}

		/// <summary>
		/// 从会话状态集合中获取一个新项
		/// </summary>
		/// <param name="name">会话状态的键。</param>
		/// <returns>返回一个会话状态的项</returns>
		public T Get<T>(string name) where T : class
		{
			string key = GetPrefixKey(name);
			return redis.Get<T>(key);
		}

		/// <summary>
		///  向会话状态集合添加一个新项。
		/// </summary>
		/// <param name="name">会话状态的键。</param>
		/// <param name="value">会话状态的项。</param>
		public void Add<T>(string name, T value) where T : class
		{
			string key = GetPrefixKey(name);
			redis.Add<T>(key, value, timeout * 60);
		}

		/// <summary>
		///  向会话状态集合更新一个新项。
		/// </summary>
		/// <param name="name">会话状态的键。</param>
		/// <param name="value">会话状态的项。</param>
		/// <param name="time">会话过期时间。</param>
		public void Set<T>(string name, T value, int time) where T : class
		{
			string key = GetPrefixKey(name);
			redis.Put<T>(key, value, time * 60);
		}

		/// <summary>
		///  删除会话状态集合中的项。
		/// </summary>
		/// <param name="name">会话状态的键。</param>
		public void Remove(string name)
		{
			string key = GetPrefixKey(name);
			redis.Remove(key);
		}

	}
}
