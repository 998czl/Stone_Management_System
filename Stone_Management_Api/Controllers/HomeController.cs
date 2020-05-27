using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stone_Management_Api.Extensions;
using Stone_Management_Api.Models;
using WebCore_Entity;
using WebCore_Entity.Application.Interface;
using WebCore_Model.External;
using WebCore_Utils;
using WebCore_Utils.ApiUtils;

namespace Stone_Management_Api.Controllers
{
    [Route("api/Home")]
	[Produces(HttpContentType.Json)]
	public class HomeController : ApiController
	{
		/// <summary>
		/// 登录
		/// </summary>
		/// <param name="model">登录信息</param>
		/// <returns></returns>
		[ApiAuthorize(VerifyCode = true)]
		[HttpPost("/api/Home/Login")]
		public ReturnResult<Token<LoginInfo>> Login([FromBody] LoginModel model)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var token = _adminservice.Login(model.Username, model.Password);
			return Json(token);
		}
	}
}