using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("account")]
    public class HomeController : Controller
    {
        private DatabaseContext db;

        public HomeController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index()
        {
            //var account = db.AccountObjects.SingleOrDefault(x => x.Username.Equals(HttpContext.Session.GetString("username")));
            var account = db.AccountObjects.SingleOrDefault(x => x.Username.Equals("username3"));
            var bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).ToList();
            ViewBag.bankAccounts = bankAccounts;
            return View();
        }
        [Route("detail")]
        public IActionResult Detail()
        {
            var account = db.AccountObjects.SingleOrDefault(x => x.Username.Equals(HttpContext.Session.GetString("username")));
            return View(account);
        }
    }
}
