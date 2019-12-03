using System;
using System.Collections.Generic;
using System.Text;

namespace WebCore_Entity.RedisManager
{
	public class RedisConfiguration
	{
		/// <summary>
		/// Host
		/// </summary>
		public string Host { get; set; }
		/// <summary>
		/// Port
		/// </summary>
		public int Port { get; set; }
		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; }
		/// <summary>
		/// DbIndex
		/// </summary>
		public int DbNumber { get; set; }
	}
}
