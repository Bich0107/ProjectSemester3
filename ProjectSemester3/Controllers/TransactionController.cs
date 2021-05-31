using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("transfer")]
    public class TransactionController : Controller
    {
        private DatabaseContext db;

        public TransactionController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("index")]
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
            ViewBag.bankAccounts = db.BankAccounts.Where(a => a.UserAccountId == account.Id).ToList();
            return View();
        }
        [Route("index")]
        [HttpPost]
        public IActionResult Index(Transaction transaction)
        {
            try
            {
                var bankCode = Request.Form["bankto"];
                var bank = db.BankAccounts.FirstOrDefault(b => b.BankCode.Equals(bankCode));
                transaction.BankAccountIdTo = bank.Id;
                ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
                ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
                return View("Step2",transaction);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Step1 Failed: " + e.InnerException.Message);
                return null;
            }
        }
        [Route("step2")]
        public IActionResult Step2(Transaction transaction)
        {
            ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
            ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
            return View("Step2",transaction);
        }
        [Route("step2")]
        [HttpPost]
        public IActionResult Step22(Transaction transaction)
        {
           
            var bankAccountTo = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.BankAccountIdTo);
            var bankAccountFrom = db.BankAccounts.FirstOrDefault(b => b.Id == transaction.BankAccountIdFrom);
            var accountTo = db.AccountObjects.FirstOrDefault(a => a.Id == bankAccountTo.UserAccountId);
            var accounForm = db.Helps.Find(1);
            var nameFrom = db.Settings.Find(1).Title;
            var mailFrom = accounForm.Email;
            var password = accounForm.Password;
            var subject = accounForm.Subject;
            var nameTo = accountTo.Name;
            var mailTo = accountTo.Email;
            var otp = new Generator();
            var otpSend = otp.GenerateNumericString(6);
            var mail = new MailHelper();
            if (mail.Send(nameFrom, mailFrom, password, nameTo, mailTo, subject, "Your OTP " + otpSend))
            {
                var bankOtp = new BankOtp();
                bankOtp.Otp = otpSend;
                bankOtp.Date = DateTime.Parse(DateTime.Now.ToString(("dd/MM/yyyy HH:mm:ss")));
                bankOtp.AccountObjectId = bankAccountFrom.UserAccountId;
                bankOtp.Status = false;
                db.BankOtps.Add(bankOtp);
                db.SaveChanges();

            }
            return View("Step3",transaction);
        }
        [Route("step3")]
        public IActionResult Step3(Transaction transaction)
        {
            return View("Step3",transaction);
        }
        [Route("step3")]
        [HttpPost]
        public IActionResult Step33(Transaction transaction)
        {
            
            try
            {
                var otpCode = Request.Form["otp"];
                var timeOtp = DateTime.Parse(DateTime.Now.ToString(("dd/MM/yyyy HH:mm:ss")));
                if (otpCode == "")
                {
                    ViewBag.otpFailed = "OTP Code invalid";
                    return View("Step3");
                }
                else
                {
                    var bankOtp = db.BankOtps.FirstOrDefault(b => b.Otp.Equals(otpCode));
                    var diff = timeOtp.Subtract(bankOtp.Date).TotalSeconds;
                    Debug.WriteLine("TotalSeconds: " + diff);
                    if (bankOtp != null && bankOtp.Status == false && diff <= 30)
                    {
                        if (transaction != null)
                        {
                            var accountFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
                            var accountTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
                            accountFrom.Balance = accountFrom.Balance - transaction.Amount;
                            accountTo.Balance = accountTo.Balance + transaction.Amount;
                            bankOtp.Status = true;

                            transaction.Time = DateTime.Parse(DateTime.Now.ToString(("dd/MM/yyyy HH:mm:ss")));
                            transaction.ResultId = 1;
                            transaction.BalanceFrom = accountFrom.Balance;
                            transaction.BalanceTo = accountTo.Balance;


                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.otpFailed = "OTP Code invalid";
                            return View("Step3");
                        }
                        ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
                        ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
                        return View("Success",transaction);
                    }
                    else
                    {
                        ViewBag.otpFailed = "OTP Code invalid";
                        return View("Step3");
                    }

                }
                
            }
            catch(Exception e){
                Debug.WriteLine("OTP Failed: " + e.InnerException.Message);
                return View("Index", "Home");
            }
        }
        [Route("success")]
        public IActionResult Success(Transaction transaction)
        {
            ViewBag.bankFrom = db.BankAccounts.Find(transaction.BankAccountIdFrom);
            ViewBag.bankTo = db.BankAccounts.Find(transaction.BankAccountIdTo);
            return View("Success",transaction);
        }
    }
}
