using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCore_Model
{
	public class Administrator
	{
		/// <summary>
		/// 用户编号
		/// </summary>
		[Key]
		public int UserId { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 手机号码
		/// </summary>

		public string Mobile { get; set; }

		/// <summary>
		/// 别名
		/// </summary>
		public string RealName { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		public int UserState { get; set; }

		//角色Id
		public int RoleId { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		public string UserPassword { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 显示手机号码
		/// </summary>
		public bool ShowMobile { get; set; }
	}
}
