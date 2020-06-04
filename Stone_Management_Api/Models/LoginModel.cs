﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stone_Management_Api.Models
{
	public class LoginModel
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[Required, MaxLength(20), MinLength(6)]
		public string Username { get; set; }
		/// <summary>
		/// 密码
		/// </summary>
		[Required, MaxLength(20), MinLength(6)]
		public string Password { get; set; }
	}
}
