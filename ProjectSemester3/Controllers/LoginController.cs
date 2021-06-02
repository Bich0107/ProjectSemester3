using GoogleAuthenticatorService.Core;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using ProjectSemester.Helpers;
using ProjectSemester3.Helpers;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("account")]   
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
            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
            return View("Login",new AccountObject());
        }
        
        [Route("login")]
        [HttpPost]
        public ActionResult Login(AccountObject accountObject)
        {
            var LoginAccount = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(accountObject.Username));
            
            if(LoginAccount != null)
            {
                HttpContext.Session.SetString("username", LoginAccount.Username);
                HttpContext.Session.SetString("name", LoginAccount.Name);
                HttpContext.Session.SetInt32("position", LoginAccount.PositionId);
                if (CheckLocked(LoginAccount))
                {
                    if (ProcessLogin(LoginAccount, accountObject.Password))
                    {
                        if (LoginAccount.IsAuthentication)
                        {
                            return RedirectToAction("Authentication");
                        }
                        else
                        {
                            if (LoginAccount.PositionId == 1)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home", new { area = "Admin" });
                            }
                        }
                    }
                    else
                    {
                        ViewBag.loginFailed = "Username or password invalid";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.loginFailed = "Your Account has been locked";
                    return View("Login");
                }
               
            }
            else
            {
                ViewBag.loginFailed = "Username or password invalid";
                return View("Login");
            }
                
            
        }
        //Check Process Login
        private bool ProcessLogin(AccountObject account, string password)
        {
            try
            {
                var passValid = BCrypt.Net.BCrypt.Verify(password, account.Password);
                if (passValid)
                {
                        account.WrongPassword = 0;
                        db.SaveChanges();
                        HttpContext.Session.SetInt32("wrongPassword", account.WrongPassword);
                        return true;
                }
                else
                {
                    account.WrongPassword = account.WrongPassword + 1;
                    if(account.WrongPassword >= 3)
                    {
                        account.Locked = true;
                    }
                    HttpContext.Session.SetInt32("wrongPassword", account.WrongPassword);
                    MsgWarning(account);
                    db.SaveChanges();
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        //Check Locked
        private bool CheckLocked(AccountObject account)
        {
            try
            {
                if (account.Locked)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            
        }
        private async void MsgWarning(AccountObject accountObject)
        {
            Debug.WriteLine("Wrong password: " + accountObject.WrongPassword);            
                try
                {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Admin",
                "tinhoang7901@gmail.com");
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("User", accountObject.Email);
                message.To.Add(to);                       

                message.Subject = "This is email Warning";

                BodyBuilder bodyBuilder = new BodyBuilder();
                //bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
                var Msg = db.Settings.Find(1);
                if (accountObject.WrongPassword == 1)
                {
                    bodyBuilder.TextBody = Msg.WarningMsg1;
                }else if(accountObject.WrongPassword == 2)
                {
                    bodyBuilder.TextBody = Msg.WarningMsg2;
                }
                else
                {
                    bodyBuilder.TextBody = Msg.WarningMsg3;
                };
                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("tinhoang7901@gmail.com", "0347557353");
                
                await client.SendAsync(message);
                Debug.WriteLine("Done");
                client.Disconnect(true);
                client.Dispose();
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            
        }

        [Route("authentication")]
        [HttpGet]
        public ActionResult Authentication()

        {
            var username = HttpContext.Session.GetString("username");
            if (HttpContext.Session.GetString("username") != null)
            {

                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                var UserUniqueKey = (Convert.ToString(HttpContext.Session.GetString("username")) + key);

                HttpContext.Session.SetString("UserUniqueKey", UserUniqueKey);

                var setupInfo = tfa.GenerateSetupCode("Banking Online", username, UserUniqueKey, 100, 100);

                //QR code Image
                ViewBag.BarcodeImageUrl = setupInfo.QrCodeSetupImageUrl;
                //Manual Key
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
            var username = (HttpContext.Session.GetString("username"));

            if (username == null)
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
                var account = db.AccountObjects.SingleOrDefault(a => a.Username.Equals(username));
                HttpContext.Session.SetString("id", Convert.ToString(username));
                passcode = "";
                if (account.PositionId == 1)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            

        }
        //Check Process Login Authentication
        private bool ProcessAuthentication(string passcode)
        {
            try
            {
                var token = passcode;
                var username = (HttpContext.Session.GetString("username"));
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

                string UserUniqueKey = (Convert.ToString(username) + key);

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
            HttpContext.Session.Remove("username");
            return RedirectToAction("login");
        }
        // forgot-password
        
        [Route("forgot-password")]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            
            return View("ForgotPassword");
        }
        [Route("forgot-password")]
        [HttpPost]
        public IActionResult ForgotPassword(AccountObject accountObject)
        {
            try
            {
                var account = db.AccountObjects.SingleOrDefault(a => a.IdNum.Equals(accountObject.IdNum) 
                && a.PhoneNumber.Equals(accountObject.PhoneNumber)
                && a.Email.Equals(accountObject.Email));

                if (account != null)
                {
                    Generator generator = new Generator();
                    var accountFrom = db.Helps.Find(1);
                    var from = accountFrom.Email;
                    var password = accountFrom.Password;
                    var subject = accountFrom.Subject;
                    var name = db.Settings.Find(1).Title;

                    var otp = generator.GenerateOtp();
                    var body = "This is new password " + otp +". Now, you can submit this password when you want to login.Please dont't share this password with anyone.";
                    MailHelper mail = new MailHelper();
                    mail.Send(name,from,password,account.Name,account.Email,subject,body);
                    account.Password = BCrypt.Net.BCrypt.HashPassword(otp);
                    account.Locked = false;
                    db.SaveChanges();
                    ViewBag.resetPasswordSuccess = "Reset password success. Now you need to go back to the login page";
                    return View("ForgotPassword");
                }
                else
                {
                    ViewBag.resetPasswordFailed = "Identity Number, phone number or email invalid";
                    return View("ForgotPassword");
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("Forgot password Failed: " + e.Message);
                return View("ForgotPassword");
            }

            
        }
        [Route("sign-up")]
        [HttpGet]
        public IActionResult SignUp()
        {

            return View("SignUp");
        }
        [Route("sign-up")]
        public IActionResult SignUp(AccountObject _accountObj)
        {

            try
            {
                var checkUsername = db.AccountObjects.FirstOrDefault(a => 
                a.Username.Equals(_accountObj.Username)
                || a.PhoneNumber.Equals(_accountObj.PhoneNumber)
                || a.Email.Equals(_accountObj.Email)
                || a.IdNum.Equals(_accountObj.IdNum)
                );
                if(checkUsername == null)
                {
                    if(_accountObj.Id == Guid.Empty)
                    {
                        _accountObj.Id = Guid.NewGuid();
                        _accountObj.CreatedDate = DateTime.Today;
                        _accountObj.Password = BCrypt.Net.BCrypt.HashString(_accountObj.Password);
                        _accountObj.Locked = false;
                        _accountObj.WrongPassword = 0;
                        _accountObj.IsAuthentication = false;
                        _accountObj.Staff = false;
                        _accountObj.PositionId = 1;
                        db.AccountObjects.Add(_accountObj);
                        //gui mail xac nhan dang ki thanh cong
                        db.SaveChanges();                        
                    }
                    return Json(new
                    {
                        success = true,
                        Error = "Signup successfully.Now you need to go back to the login page",
                        result = "Redirect",
                        url = Url.Action("Logout", "Login")
                    });
                }
                else
                {
                    return Json(new { success = false, Error = "Some of your information already exists. Please double-check before you sign up" });
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Signup Failed: " + e.InnerException.Message);
                return Json(new { success = false, Error = "Something went wrong. Please try again" });
            }
        }
    }
}
