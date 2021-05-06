using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/bankAccount")]
    public class BankAccountController : Controller
    {
        private DatabaseContext db;

        public BankAccountController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("add")]
        [Route("~/")]
        public IActionResult Add()
        {
            var bankAccount = new BankAccount
            {
                CreatedDate = DateTime.Today,
                ExpiredDate = DateTime.Today.AddDays(BankAccountConstances.Duration),
                Locked = false
            };

            ViewBag.accountObjects = db.AccountObjects.ToList();
            ViewBag.currencies = db.Currencies.ToList();
            ViewBag.banks = db.Banks.ToList();

            return View("add", bankAccount);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(BankAccount _bankAccount)
        {
            Debug.WriteLine(_bankAccount.BankId);
            db.BankAccounts.Add(_bankAccount);
            db.SaveChanges();

            ViewBag.accountObjects = db.AccountObjects.ToList();
            ViewBag.currencies = db.Currencies.ToList();
            ViewBag.banks = db.Banks.ToList();

            return View("add");
        }
    }
}
