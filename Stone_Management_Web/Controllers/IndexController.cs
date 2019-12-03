using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stone_Management_Web.Controllers
{
    public class IndexController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}