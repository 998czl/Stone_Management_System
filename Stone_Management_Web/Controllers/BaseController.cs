using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCore_Entity.RedisManager;
using WebCore_Model;

namespace Stone_Management_Web.Controllers
{
    public class BaseController : Controller
    {
		public IHttpContextAccessor _accessor;
	
		public RedisSession RedisSession => new RedisSession(_accessor.HttpContext);

		protected Administrator CurrentUser
		{
			get
			{
				RedisSession.SetExpire("ADMINUSER", 60);
				return RedisSession.Get<Administrator>("ADMINUSER");
			}
			set { RedisSession.Add("ADMINUSER", value); }
		}
	}
}