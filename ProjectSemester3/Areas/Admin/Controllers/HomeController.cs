using GoogleAuthenticatorService.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        //*************Key*********
        const string key = "test123!@@)(*";

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

            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
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

        [HttpPost]
        [Route("changePassword")]
        public IActionResult EditPassword(AccountObject _accountObj)
        {
            try
            {
                var username = HttpContext.Session.GetString("username");
                username = username == null ? "superAdmin" : username;

                var accountObj = db.AccountObjects.Where(a => a.Username.Equals(username)).SingleOrDefault();

                accountObj.Password = BCrypt.Net.BCrypt.HashString(_accountObj.Password);
                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //*************Create account, QR code and Key**********
        [HttpGet]
        [Route("authenticate")]
        public IActionResult Authentication()
        {
            var username = HttpContext.Session.GetString("username");

            // for testing only
            username = string.IsNullOrEmpty(username) ? "superAdmin" : username;

            if (username != null)
            {
                //using GoogleAuthenticatorService.Core; 
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                var UserUniqueKey = (Convert.ToString(username) + key);

                HttpContext.Session.SetString("UserUniqueKey", UserUniqueKey);

                var setupInfo = tfa.GenerateSetupCode("Banking Online", username, UserUniqueKey, 100, 100);

                //QR code Image
                var barcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                //Manual Key
                var setupCode = setupInfo.ManualEntryKey;

                return new JsonResult(new { barcodeImageUrl, setupCode });
            }
            else
            {
                return Ok(false);
            }
        }

        //**************check authenticate code**************
        [Route("confirmAuthenticate/{passcode}")]
        public IActionResult ProcessAuthentication(string passcode)
        {
            try
            {
                var token = passcode;
                var username = (HttpContext.Session.GetString("username"));

                // for testing only
                username = string.IsNullOrEmpty(username) ? "superAdmin" : username;

                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                string UserUniqueKey = (Convert.ToString(username) + key);

                bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
                return Ok(isValid);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
