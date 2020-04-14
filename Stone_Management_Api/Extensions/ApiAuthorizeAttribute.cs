using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore_Entity;
using WebCore_Entity.RedisManager;
using WebCore_Model.External;
using WebCore_Utils;
using WebCore_Utils.ApiUtils;

namespace Stone_Management_Api.Extensions
{
	public class ApiAuthorizeAttribute:ActionFilterAttribute
	{
		/// <summary>
		/// 访问频率
		/// </summary>
		public double Frequency = 10D;
		/// <summary>
		/// 是否验证图片验证码
		/// </summary>
		public bool VerifyCode { get; set; }
		/// <summary>
		/// 是否验证文件上传，1：选填，2：必填
		/// </summary>
		public int VerifyFileUpload { get; set; }
		/// <summary>
		/// 是否验证Excel导出
		/// </summary>
		public bool VerifyExcelExport { get; set; }
		/// <summary>
		/// 是否验证Token
		/// </summary>
		public bool VerifyToken { get; set; }
		/// <summary>
		/// 是否验证权限
		/// </summary>
		public bool VerifyRight { get; set; }

		/// <summary>
		/// OnActionExecutionAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="next"></param>
		/// <returns></returns>
		public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{			
			var temp = context.HttpContext.Request.Headers["X-Timestamp"].FirstOrDefault();
			if (string.IsNullOrEmpty(temp))
			{
				throw new CustomException("无效的请求头");
			}
			var timestamp = temp.ToDouble();

			//判断时间戳是否有效
			var timestampDiff = DateTimeHelper.TimestampOfMilliseconds - timestamp;
			if (timestampDiff < 0D || timestampDiff > 30000D)
			{
				//throw new InvalidOperationException("请求超时");
			}

			//请求地址
			var path = context.HttpContext.Request.Path.Value;
			path = path.Substring(path.IndexOf("/api/"));

			//限制请求频率
			if (Frequency > 0D)
			{
				var redis = RedisHelper.Database15;
				var key = string.Format("Request-{0}-{1}", context.HttpContext.Connection.RemoteIpAddress, path);
				if (redis.KeyExists(key))
				{
					throw new CustomException("操作太过频繁，请稍后重试");
				}
				var milliseconds = (1D / Frequency) * 1000D;
				var expiry = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(milliseconds));
				redis.Set(key, DateTime.UtcNow.ToString(), expiry);
			}

			//验证图片验证码
			if (VerifyCode)
			{
				//context.HttpContext.Request.VerifyImageCode();
			}

			//验证文件上传
			if (VerifyFileUpload > 0)
			{

			}

			//验证Excel导出
			if (VerifyExcelExport)
			{
				var fileName = context.HttpContext.Request.Headers["X-ExcelName"].FirstOrDefault();
				var headers = context.HttpContext.Request.Headers["X-ExcelHeaders"].FirstOrDefault();
				if (string.IsNullOrEmpty(fileName))
				{
					throw new CustomException("文件名不可为空");
				}
				if (string.IsNullOrEmpty(headers))
				{
					throw new CustomException("列头信息不可为空");
				}
				var _headers = headers.Split(',');
				if (_headers.Length == 0)
				{
					throw new CustomException("列头信息格式不正确");
				}
				var dic = new Dictionary<string, string>();
				foreach (var _header in _headers)
				{
					var kv = _header.Split(':');
					if (kv.Length != 2)
					{
						throw new CustomException("列头信息格式不正确：" + _header);
					}
					var key = kv[0];
					if (string.IsNullOrEmpty(key))
					{
						throw new CustomException("列头信息键不能为空：" + key);
					}
					var value = kv[1];
					if (dic.ContainsKey(key))
					{
						throw new CustomException("列头信息存在重复键：" + key);
					}
					dic.Add(key, value);
				}
			}

			if (VerifyToken)
			{
				var token = context.HttpContext.Request.Headers["X-Token"].FirstOrDefault();
				if (string.IsNullOrEmpty(token))
				{
					throw new CustomException("login_timeout");
				}
				var loginInfo = VerifyLoginInfo(token);
				if (loginInfo == null)
				{
					throw new CustomException("login_timeout");
				}
				if (VerifyRight)
				{
					//if (loginInfo.Data.Category != LoginCategory.User || !VerifyRequestRight(loginInfo.Id, path.Substring(1)))
					//{
					//	throw new CustomException("没有访问权限");
					//}
					if (loginInfo.Data.Username != "administrator")
					{
						throw new CustomException("没有访问权限");
					}
				}
			}
			try
			{
				var _path = context.HttpContext.Request.Path;
				var _method = context.HttpContext.Request.Method;
				var _data = context.HttpContext.Request.GetParameters().ToQueryString();
				var _mac = context.HttpContext.Request.Headers["X-Mac"].FirstOrDefault();
				var _ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
				LogHelper.InfoFormat("接收请求=>{0} {1} {2} {3} {4}", _ip, _mac, _method, _path, _data);
			}
			catch (Exception ex)
			{
				LogHelper.Error("接收请求报错，", ex);
			}
			return base.OnActionExecutionAsync(context, next);
		}

		/// <summary>
		/// 验证登录信息
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		protected Token<LoginInfo> VerifyLoginInfo(string token)
		{
			return TokenHelper.Verify<LoginInfo>(token, "User");
		}	
	}
}
