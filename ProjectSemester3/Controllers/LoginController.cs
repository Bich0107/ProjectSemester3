using GoogleAuthenticatorService.Core;
using Microsoft.AspNetCore.Http;
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
        const string key = "test123!@@)(*";
        public LoginController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("login")]
        [Route("")]
        [Route("~/")]
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.title = db.Settings.Find(1).Title;
            return View("Login",new AccountObject());
        }
        
        [Route("login")]
        [HttpPost]
        public ActionResult Login(AccountObject accountObject)
        {
                if (ProcessLogin(accountObject.Username, accountObject.Password) == null)
                {
                ViewBag.loginFailed = "Username or Password Invalid";
                return View("Login");
            }
                else
                {
                var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(accountObject.Username));
                HttpContext.Session.SetString("tempid", accountObject.Username);
                if (account.IsAuthentication)
                {
                    return RedirectToAction("Authentication");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }                
                }
                
            
        }
        //Check Process Login
        private AccountObject ProcessLogin(string username, string password)
        {
            try
            {
                var account = db.AccountObjects.SingleOrDefault(a => a.Username.Equals(username));
                if (account != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                    {
                        return account;
                    }

                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        [Route("authentication")]
        [HttpGet]
        public ActionResult Authentication()

        {
            var tempid = HttpContext.Session.GetString("tempid");
            if (HttpContext.Session.GetString("tempid") != null)
            {

                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                var UserUniqueKey = (Convert.ToString(HttpContext.Session.GetString("tempid")) + key);

                HttpContext.Session.SetString("UserUniqueKey", UserUniqueKey);

                var setupInfo = tfa.GenerateSetupCode("Banking Online", tempid, UserUniqueKey, 100, 100);

                ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;

                ViewBag.SetupCode = setupInfo.ManualEntryKey;

                return View("Authentication");

            }
            else
            {
                return View("Login");
            }
        }

        [Route("authentication")]
        [HttpPost]
        public ActionResult Authentication(string passcode)
        {
            var tempid = (HttpContext.Session.GetString("tempid"));

            if (tempid == null)
            {

                return RedirectToAction("Login");

            }

            if (!ProcessAuthentication(passcode))
            {
                ViewBag.AuthenticationFailed = "Invalid authentication code";
                return View("Authentication");
            }
            else
            {
                HttpContext.Session.SetString("id", Convert.ToString(tempid));
                passcode = "";
                return RedirectToAction("Index", "Home");
            }
            

        }
        //Check Process Login Authentication
        private bool ProcessAuthentication(string passcode)
        {
            try
            {
                var token = passcode;
                var tempid = (HttpContext.Session.GetString("tempid"));
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                string UserUniqueKey = (Convert.ToString(tempid) + key);

                bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
                return isValid;
            }
            catch
            {
                return false;
            }
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("tempid");
            return RedirectToAction("login");
        }
    }
}
