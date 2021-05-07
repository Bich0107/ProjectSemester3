using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/check")]
    public class CheckController : Controller
    {
        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }
    }
}
