using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore_Entity;
using WebCore_Entity.Application.Interface;
using WebCore_Utils;

namespace Stone_Management_Web.Controllers
{
	public class AccountController : BaseController
	{
		public AccountController(IHttpContextAccessor accessor)
		{
			_accessor = accessor;
		}
		public IActionResult Index()
		{
			ViewBag.CpMoblie = "false";
			ViewBag.LoginImg = Setting.loginImg;
			ViewBag.SysTitleName = ".net core";
			return View();
		}

		[HttpPost]
		public IActionResult Index(string userName, string password, string validateCode, string mobilevalidatecode)
		{
			if (HttpContext.Session.GetString("VALIDATE_CODE") == null || !validateCode.Equals(HttpContext.Session.GetString("VALIDATE_CODE")))
			{
				ViewBag.Content = "验证码输入不正确！";
				ViewBag.CpMoblie = "false";
				ViewBag.LoginImg = Setting.loginImg;
				return View();
			}
			try
			{
				var _adminservice = Container.Instance.Resolve<IAdministratorService>();
				var admin = _adminservice.GetAdmin(userName, password);
				if (admin != null)
				{
					CurrentUser = admin;
					return RedirectToAction("Index", "Home");
				}
			}
			catch (MTSException)
			{
				throw;
			}
			return View();
		}


		public FileContentResult ValidateCode()
		{
			ValidateCodeMaker vc = new ValidateCodeMaker();
			string code = vc.CreateValidateCode(4);
			byte[] imgByte = vc.CreateValidateGraphic(code);
			HttpContext.Session.SetString("VALIDATE_CODE", code);
			return File(imgByte, @"image/jpg");
		}
	}
}