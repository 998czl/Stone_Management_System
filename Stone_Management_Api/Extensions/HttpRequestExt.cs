using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCore_Utils;

namespace Stone_Management_Api.Extensions
{
	/// <summary>
	/// HttpRequestExt
	/// </summary>
	public static class HttpRequestExt
	{
		/// <summary>
		/// GetFullPath
		/// </summary>
		/// <param name="httpRequest"></param>
		/// <returns></returns>
		public static string GetFullPath(this HttpRequest httpRequest)
		{
			return string.Format("{0}://{1}{2}", httpRequest.Scheme, httpRequest.Host.Value, httpRequest.PathBase.Value);
		}

		/// <summary>
		/// 获取请求参数
		/// </summary>
		/// <param name="httpRequest"></param>
		/// <returns></returns>
		public static IDictionary<string, string> GetParameters(this HttpRequest httpRequest)
		{
			var parameters = new Dictionary<string, string>();
			if (httpRequest.Query.Count > 0)
			{
				foreach (var kv in httpRequest.Query)
				{
					parameters.Add(kv.Key, kv.Value);
				}
			}
			else if (httpRequest.HasFormContentType)
			{
				foreach (var kv in httpRequest.Form)
				{
					parameters.Add(kv.Key, kv.Value);
				}
			}
			else if (httpRequest.Body.CanRead)
			{
				httpRequest.EnableBuffering();
				httpRequest.Body.Seek(0, 0);
				var str = "";
				using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
				{
					var task = reader.ReadToEndAsync();
					str = task.Result;
				}
				if (!string.IsNullOrEmpty(str))
				{
					var contentType = httpRequest.ContentType;
					if (!string.IsNullOrEmpty(contentType) && contentType.IndexOf("/json") != -1)
					{
						parameters = JsonHelper.DeserializeObject<Dictionary<string, string>>(str);
					}
					else
					{
						parameters.Add("", str);
					}
				}
			}
			else
			{
			}
			return parameters;
		}

		/// <summary>
		/// 是否移动设备
		/// </summary>
		/// <param name="httpRequest"></param>
		/// <returns></returns>
		public static bool IsMobileDevice(this HttpRequest httpRequest)
		{
			httpRequest.Headers.TryGetValue("User-Agent", out var userAgent);
			if (string.IsNullOrEmpty(userAgent))
			{
				return false;
			}
			return new Regex(@"(iemobile|iphone|ipod|android|nokia|sonyericsson|blackberry|samsung|sec\-|windows ce|motorola|mot\-|up.b|midp\-)", RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(userAgent);
		}
	}
}
