
using System;
using System.Collections.Generic;
using System.Text;
using WebCore_Model;
using WebCore_Model.External;
using WebCore_Utils.ApiUtils;

namespace WebCore_Entity.Application.Interface
{
	public interface IAdministratorService
	{
		Administrator GetAdmin(string userName, string password);

		bool AddAdmin(Administrator model);

		//api登陆
		Token<LoginInfo> Login(string username, string password);
	}
}
