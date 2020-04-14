using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore_Model.External;
using WebCore_Utils;
using WebCore_Utils.ApiUtils;

namespace Stone_Management_Api.Extensions
{
	public class ApiController: ControllerBase
	{
		/// <summary>
		/// Token
		/// </summary>
		private string _Token;
		/// <summary>
		/// 平台标识
		/// </summary>
		private int _Platform;
		/// <summary>
		/// IP地址
		/// </summary>
		private string _Ip;
		/// <summary>
		/// Mac地址
		/// </summary>
		private string _Mac;
		/// <summary>
		/// 登录信息
		/// </summary>
		private Token<LoginInfo> _LoginInfo;

		/// <summary>
		/// Token
		/// </summary>
		public string Token
		{
			get
			{
				if (_Token == null)
				{
					_Token = Request.Headers["X-Token"].FirstOrDefault();
				}
				return _Token;
			}
		}

		/// <summary>
		/// 平台标识
		/// </summary>
		public int Platform
		{
			get
			{
				if (_Platform == 0)
				{
					_Platform = Request.Headers["X-Platform"].FirstOrDefault().ToInt32(0);
				}
				return _Platform;
			}
		}

		/// <summary>
		/// Ip地址
		/// </summary>
		public string Ip
		{
			get
			{
				if (_Ip == null)
				{
					//_Ip = Request.GetIP();
					_Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
				}
				return _Ip;
			}
		}

		/// <summary>
		/// Mac地址
		/// </summary>
		public string Mac
		{
			get
			{
				if (_Mac == null)
				{
					_Mac = Request.Headers["X-Mac"].FirstOrDefault();
				}
				return _Mac;
			}
		}

		/// <summary>
		/// 登录信息
		/// </summary>
		public Token<LoginInfo> LoginInfo
		{
			get
			{
				if (_LoginInfo == null)
				{
					_LoginInfo = GetLoginInfo();
				}
				return _LoginInfo;
			}
		}

		/// <summary>
		/// 获取登录信息
		/// </summary>
		/// <returns></returns>
		protected Token<LoginInfo> GetLoginInfo()
		{
			return null;
		}

		/// <summary>
		/// Ok
		/// </summary>
		/// <param name="message">消息</param>
		/// <returns></returns>
		protected ReturnResult<string> Ok(string message = null)
		{
			return Content("", HttpContentType.Json, message);
		}

		/// <summary>
		/// Json
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="content">内容</param>
		/// <param name="message">消息</param>
		/// <returns></returns>
		protected ReturnResult<T> Json<T>(T content, string message = null)
		{
			return Content(content, HttpContentType.Json, message);
		}

		/// <summary>
		/// Content
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="content">内容</param>
		/// <param name="contentType">内容类型</param>
		/// <param name="message">消息</param>
		/// <returns></returns>
		protected ReturnResult<T> Content<T>(T content, string contentType, string message = "success")
		{
			if (string.IsNullOrEmpty(message))
			{
				message = "success";
			}
			var code = message == "success" ? ReturnCode.OK : ReturnCode.CustomException;
			return new ReturnResult<T>(Request, code, message, content, contentType);
		}
	}
}
