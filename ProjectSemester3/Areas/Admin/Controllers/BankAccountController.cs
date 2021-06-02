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

        #region Add, edit, delete
        [HttpPost]
        [Route("add")]
        public IActionResult Add(BankAccount _bankAccount)
        {
            try
            {
                _bankAccount.CreatedDate = DateTime.Today;
                
                db.BankAccounts.Add(_bankAccount);
                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            try
            {
                return new JsonResult(db.BankAccounts.Find(id));
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(BankAccount _bankAccount)
        {
            try
            {
                var bankAccount = db.BankAccounts.Find(_bankAccount.Id);

                bankAccount.UserAccountId = _bankAccount.UserAccountId;
                bankAccount.Balance = _bankAccount.Balance;
                bankAccount.ExpiredDate = _bankAccount.ExpiredDate;
                bankAccount.BankId = _bankAccount.BankId;
                bankAccount.Locked = _bankAccount.Locked;
                bankAccount.CurrencyId = _bankAccount.CurrencyId;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                db.BankAccounts.Remove(db.BankAccounts.Find(id));
                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
            ViewBag.tagName = "Bank account";

            ViewBag.activeTag = "bankAccount";

            ViewBag.accountObjects = db.AccountObjects.Where(a => !a.Staff).ToList();
            ViewBag.currencies = db.Currencies.ToList();
            ViewBag.banks = db.Banks.ToList();

            return View();
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.BankAccounts.Where(b => !b.UserAccount.Staff).ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error bank account: " + e.Message);
                return null;
            }
        }

        [Route("search/{keyword}")]
        public IActionResult Search(string keyword)
        {
            try
            {
                return new JsonResult(db.BankAccounts.Where(
                    a => (a.UserAccount.Name.Contains(keyword) ||
                      a.Bank.Name.Contains(keyword) ||
                      a.Currency.Name.Contains(keyword) ||
                      a.Currency.Fullname.Contains(keyword)) && !a.UserAccount.Staff
                ).ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error bank account: " + e.Message);
                return null;
            }
        }

        [Route("resetOTP")]
        public IActionResult ResetOTP()
        {
            foreach (var bankAccount in db.BankAccounts.ToList())
            {
                bankAccount.BankCode = null;
            }

            return Ok();
        }
    }
}
