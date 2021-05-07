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
        public IActionResult Add()
        {
            var bankAccount = new Models.BankAccount
            {
                CreatedDate = DateTime.Today,
                ExpiredDate = DateTime.Today.Add(BankAccountConstances.Duration),
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

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Add(Guid id)
        {
            var bankAccount = db.BankAccounts.Find(id);
            bankAccount.Id = id;

            ViewBag.accountObjects = db.AccountObjects.ToList();
            ViewBag.currencies = db.Currencies.ToList();
            ViewBag.banks = db.Banks.ToList();

            return View("update", bankAccount);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(BankAccount _bankAccount)
        {
            var bankAccount = db.BankAccounts.Find(_bankAccount.Id);

            bankAccount.UserAccountId = _bankAccount.UserAccountId;
            bankAccount.Balance = _bankAccount.Balance;
            bankAccount.BankCode = _bankAccount.BankCode;
            bankAccount.ExpiredDate = _bankAccount.ExpiredDate;
            bankAccount.BankId = _bankAccount.BankId;
            bankAccount.Locked = _bankAccount.Locked;
            bankAccount.CurrencyId = _bankAccount.CurrencyId;

            ViewBag.accountObjects = db.AccountObjects.ToList();
            ViewBag.currencies = db.Currencies.ToList();
            ViewBag.banks = db.Banks.ToList();

            db.SaveChanges();

            return View("update");
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            db.BankAccounts.Remove(db.BankAccounts.Find(id));
            db.SaveChanges();
            return View("delete");
        }
    }
}
