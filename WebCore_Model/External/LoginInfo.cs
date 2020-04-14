using System;
using System.Collections.Generic;
using System.Text;
using WebCore_Utils.ApiUtils;

namespace WebCore_Model.External
{
	public class LoginInfo: TokenData
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="category">用户类别</param>
		public LoginInfo(LoginCategory category)
		{
			Category = category;
		}

		/// <summary>
		/// 昵称
		/// </summary>
		public string Nickname { get; set; }
		/// <summary>
		/// 姓名
		/// </summary>
		public string FullName { get; set; }
		/// <summary>
		/// 手机号码
		/// </summary>
		public string Mobile { get; set; }
		/// <summary>
		/// 角色
		/// </summary>
		public ICollection<int> RoleIds { get; set; }
		/// <summary>
		/// 用户类别
		/// </summary>
		public LoginCategory Category { get; set; }
		/// <summary>
		/// 头像
		/// </summary>
		public string Avatar { get; set; }
	}
}
