using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("bank-account")]
    public class BankAccountController : Controller
    {
        private DatabaseContext db;

        public BankAccountController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
            var bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).ToList();
            dynamic model = new ExpandoObject();
            model.bankAccounts = bankAccounts;
            return View(model);
        }
        [Route("getmodel/{id}")]
        [HttpGet]
        public IActionResult GetModel(string id)
        {
            var guidId = Guid.Parse(id);
            var model = db.BankAccounts.SingleOrDefault(b => b.Id == guidId);
            return new JsonResult(model);
        }
        [Route("getbankmodel/{code}")]
        [HttpGet]
        public IActionResult GetBankModel(string code)
        {            
            var model = db.BankAccounts.FirstOrDefault(b => b.BankCode == code);
            return new JsonResult(model);
        }
        [Route("quick-search/{id}/{value}")]
        [HttpGet]
        public IActionResult QuickSearch(string value, Guid id)
        {
            
            var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id).ToList();
            if (value.Equals("ten-day"))
            {
                
            }else if (value.Equals("one-month"))
            {

            }
            else
            {

            }
            Debug.WriteLine("transaction id: " + id + value);
            return new JsonResult(model);
        }

    }
}
