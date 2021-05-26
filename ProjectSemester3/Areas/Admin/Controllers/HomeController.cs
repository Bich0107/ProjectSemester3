using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        private DatabaseContext db;

        public HomeController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("username");

            // for testing only
            username = string.IsNullOrEmpty(username) ? "superAdmin" : username;

            ViewBag.username = username;

            ViewBag.settings = db.Settings.Find(1);

            ViewBag.tagName = "Profile";
            ViewBag.activeTag = "profile";
            ViewBag.activeParentTag = "home";
            return View("index");
        }

        [Route("account")]
        public IActionResult Account()
        {
            var username = HttpContext.Session.GetString("username");

            // for testing only
            username = string.IsNullOrEmpty(username) ? "superAdmin" : username;

            ViewBag.username = username;

            ViewBag.settings = db.Settings.Find(1);

            ViewBag.tagName = "Account";
            ViewBag.activeTag = "account";
            ViewBag.activeParentTag = "home";
            return View("account");
        }

        [HttpPost]
        [Route("editProfile")]
        public IActionResult Edit(AccountObject _accountObj)
        {
            try
            {
                var username = HttpContext.Session.GetString("username");
                username = username == null ? "superAdmin" : username;

                var accountObj = db.AccountObjects.Where(a => a.Username.Equals(username)).SingleOrDefault();

                accountObj.Name = _accountObj.Name;
                accountObj.Birthday = _accountObj.Birthday;
                accountObj.PhoneNumber = _accountObj.PhoneNumber;
                accountObj.Email = _accountObj.Email;
                accountObj.Address = _accountObj.Address;
                accountObj.Gender = _accountObj.Gender;
                accountObj.IdNum = _accountObj.IdNum;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("editInfo")]
        public IActionResult EditInfo(AccountObject _accountObj)
        {
            try
            {
                var username = HttpContext.Session.GetString("username");
                username = username == null ? "superAdmin" : username;

                var accountObj = db.AccountObjects.Where(a => a.Username.Equals(username)).SingleOrDefault();

                accountObj.Username = _accountObj.Username;
                accountObj.IsAuthentication = _accountObj.IsAuthentication;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
