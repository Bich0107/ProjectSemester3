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
                transaction.Time = DateTime.Now;
                return View("Step2",transaction);
            }
            catch(Exception e)
            {
                Debug.WriteLine("Step1 Failed: " + e.InnerException.Message);
                return View();
            }
        }
        [Route("step2")]
        public IActionResult Step2(Transaction transaction)
        {            
            return View("Step2",transaction);
        }
        [Route("step2")]
        [HttpPost]
        public IActionResult Step22(Transaction transaction)
        {

            return View("Step3");
        }
        [Route("step3")]
        public IActionResult Step3()
        {

            return View("Step3");
        }
        [Route("step3")]
        [HttpPost]
        public IActionResult Step33()
        {
            return View("Success");
        }
        [Route("success")]
        public IActionResult Success()
        {

            return View("Success");
        }
    }
}
