using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private DatabaseContext db;

        public LoginController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("login")]
        [Route("")]
        [Route("~/")]
        public IActionResult Login()
        {
            var hhh = 123;
            ViewBag.title = db.Settings.Find(1).Title;
            return View();
        }
    }
}
