using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity.RedisManager
{
	public class RedisHelper
	{
		/// <summary>
		/// configuration
		/// </summary>
		private static RedisConfiguration configuration;
		/// <summary>
		/// Configuration
		/// </summary>
		public static RedisConfiguration Configuration
		{
			get
			{
				if (configuration == null)
				{
					if (JsonConfigHelper.ConfigurationCollection.ContainsKey("Redis"))
					{
						var setting = JsonConfigHelper.Get<string>("Redis");
						configuration = new RedisConfiguration()
						{
							Host = setting["Host"],
							Port = int.Parse(setting["Port"]),
							Password = setting["Password"],
							DbNumber = int.Parse(setting["DbNumber"]),
						};
					}
					else if (JsonConfigHelper.ConnectionStrings.ContainsKey("Redis"))
					{
						var connStr = JsonConfigHelper.ConnectionStrings["Redis"];
						var array = connStr.Split(',');
						var array0 = array[0].Split(':');
						var array1 = array[1].Split('=');
						configuration = new RedisConfiguration()
						{
							Host = array0[0],
							Port = int.Parse(array0[1]),
							Password = array1[1],
							DbNumber = 2,
						};
					}
					else
					{
						throw new Exception("Redis未配置");
					}
				}
				return configuration;
			}
		}

		/// <summary>
		/// 创建redis连接
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public static ConnectionMultiplexer CreateInstance(RedisConfiguration configuration = null)
		{
			if (configuration == null)
			{
				configuration = Configuration;
			}
			var str = CreateConfiguration(configuration.Host, configuration.Port, configuration.Password);
			return ConnectionMultiplexer.Connect(str);
		}

		/// <summary>
		/// CreateConfiguration
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string CreateConfiguration(string host = "localhost", int port = 6379, string password = null)
		{
			return string.Format("{0}:{1},password={2}", host, port, password);
		}


		/// <summary>
		/// 获取redis连接对象
		/// </summary>
		public static ConnectionMultiplexer Instance => CreateInstance(Configuration);
		/// <summary>
		/// Db
		/// </summary>
		/// <returns></returns>
		public static IDatabase Database => Instance.GetDatabase(Configuration.DbNumber);
	}
}
