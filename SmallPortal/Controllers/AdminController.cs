using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace SmallPortal.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles="Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}