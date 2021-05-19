using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Print()
        {
            return View();
        }
    }
}
