using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SmallPortal.Models;

namespace SmallPortal.Controllers
{
    public class ServiceController : Controller
    {
       
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(ILogger<ServiceController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Bookkeeping()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Branding()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult BusinessPlans()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Digital()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Incorporation()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Payroll()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Taxes()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult WebDevelopment()
        {
            return View();
        }

          [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}