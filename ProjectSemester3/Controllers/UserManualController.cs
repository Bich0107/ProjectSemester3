using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("user-manual")]
    public class UserManualController : Controller
    {
        private DatabaseContext db;
        public UserManualController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.faqs = db.Faqs.ToList();
            return View();
        }
    }
}
