﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stone_Management_Api.Extensions
{
	public class CustomException:Exception
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public CustomException() : this(string.Empty, null)
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message"></param>
		public CustomException(string message) : this(message, null)
		{
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CustomException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
