using GoogleAuthenticatorService.Core;
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
    [Route("profile")]
    public class AccountObjectController : Controller
    {
        private DatabaseContext db;
        const string key = "test123!@@)(*";
        public AccountObjectController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
            var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
            ViewBag.name = account.Name;
            //account.Birthday = DateTime.Parse(account.Birthday.ToString("dd/MM/yyyy"));
            return View(account);
        }
        [Route("setting")]
        public IActionResult Setting()
        {
            try
            {
                var username = HttpContext.Session.GetString("username");
                var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
                if(account != null)
                {
                    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                    var UserUniqueKey = (Convert.ToString(HttpContext.Session.GetString("username")) + key);

                    HttpContext.Session.SetString("UserUniqueKey", UserUniqueKey);

                    var setupInfo = tfa.GenerateSetupCode("Banking Online", username, UserUniqueKey, 100, 100);

                    ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;

                    ViewBag.SetupCode = setupInfo.ManualEntryKey;

                    
                    return View("Setting", account);
                }
                else
                {
                    return RedirectToAction("Logout", "Login");
                }
            }catch(Exception e)
            {
                Debug.WriteLine("Confirm Password Failed: " + e.InnerException.Message);
                return BadRequest(e.Message);
            }
        }
        [Route("confirm-password")]
        public IActionResult ConfirmPassword(AccountObject accountObject)
        {
            try
            {
                var account = db.AccountObjects.Find(accountObject.Id);
                if (BCrypt.Net.BCrypt.Verify(accountObject.Password, account.Password))
                {
                    return Json(new { success = true, Error = "Successfully" });
                }
                else
                {
                    return Json(new { success = false, Error = "Wrong password" });
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine("Confirm Password Failed: " + e.InnerException.Message);
                return Json(new { success = false, Error = "Something went wrong. Please try again" });
            }
        }
        [Route("change-password")]
        public IActionResult ChangePassword(AccountObject accountObject)
        {
            try
            {
                var account = db.AccountObjects.Find(accountObject.Id);
                if (BCrypt.Net.BCrypt.Verify(accountObject.Password, account.Password))
                {
                    return Json(new { success = false, Error = "The new password cannot be the same as the old password" });
                }
                else
                {
                    account.Password = BCrypt.Net.BCrypt.HashPassword(accountObject.Password);
                    db.SaveChanges();
                    return Json(new { success = true, 
                        Error = "Change password successfully.Now you need to log out and log back in",
                        result = "Redirect",
                        url = Url.Action("Logout", "Login")
                    });
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Change Password Failed: " + e.InnerException.Message);
                return Json(new { success = false, Error = "Something went wrong. Please try again" });
            }
        }
        [Route("getmodel/{id}")]
        public IActionResult GetModel(Guid id)
        {
            var model = db.AccountObjects.FirstOrDefault(b => b.Id == id);
            return new JsonResult(model);
        }
        [Route("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return new JsonResult(db.AccountObjects.Find(id));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AccountObject _accountObj)
        {
            try
            {
                var accounts = db.AccountObjects.Where(a => a.Id != _accountObj.Id).ToList();
                var msg = "Your data has been saved successfully";
                if (accounts.Count(a => a.PhoneNumber == _accountObj.PhoneNumber) >= 1)
                {
                    msg = "Can't save duplicate phone number";
                    return Json(new { success = false, Error = msg });
                }
                if (accounts.Count(a => a.Email == _accountObj.Email) >= 1)
                {
                    msg = "Can't save duplicate email";
                    return Json(new { success = false, Error = msg });
                }

                var accountObj = db.AccountObjects.Find(_accountObj.Id);
                accountObj.PhoneNumber = _accountObj.PhoneNumber;
                accountObj.Email = _accountObj.Email;
                accountObj.Address = _accountObj.Address;
                accountObj.Job = _accountObj.Job;                
                db.SaveChanges();
                return Json(new { success = true, Error = msg });
            }
            catch (Exception e)
            {
                Debug.WriteLine("account Object Failed: " + e.InnerException.Message);
                return Json(new { success = false, Error = "Something went wrong. Please try again" });
            }
        }
        //Check Process Login Authentication
        [Route("authentication/{passcode}")]
        public IActionResult Authentication(string passcode)
        {
            try
            {
                
                var token = passcode;
                Debug.WriteLine("Token: " + token);
                var username = (HttpContext.Session.GetString("username"));
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                string UserUniqueKey = (Convert.ToString(username) + key);

                bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
                var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(username));
                if (isValid)
                {
                    
                    account.IsAuthentication = !account.IsAuthentication;
                    db.SaveChanges();
                    return Json(new { success = true, 
                        Error = "Your change have been saved", 
                        check = account.IsAuthentication,
                        result = "Redirect",
                        url = Url.Action("Setting", "Profile")});
                }
                else
                {
                    return Json(new { success = false, Error = "The 2FA code wrong", check = account.IsAuthentication });
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("Authenticator Failed: " + e.InnerException.Message);
                return Json(new { success = false, Error = "Something went wrong. Please try again" });
            }
        }
    }
}
