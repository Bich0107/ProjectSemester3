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
            try
            {
                ViewBag.Title = db.Settings.Find(1).Title;
                ViewBag.mail = db.Helps.Find(1).Email;
                ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
                ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
                var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
                ViewBag.name = account.Name;
                var bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).ToList();
                dynamic model = new ExpandoObject();
                model.bankAccounts = bankAccounts;
                return View(model);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("reports/{id}/{value}")]
        public IActionResult Report(Guid id, string value)
        {
            try
            {
                var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id).ToList();
                if (value.Equals("ten-day"))
                {
                    model.Where(t => t.Time >= t.Time.AddDays(-10)).ToList();
                }
                else if (value.Equals("one-month"))
                {
                    model.Where(t => t.Time >= t.Time.AddDays(-30)).ToList();
                }
                else if (value.Equals("three-months"))
                {
                    model.Where(t => t.Time >= t.Time.AddDays(-90)).ToList();
                }
                else
                {
                    model = null;
                }
                Debug.WriteLine("transaction id: " + id + value);
                var models = new List<Transaction>();
                if (model != null)
                {
                    foreach (var m in model)
                    {
                        if (m.BankAccountIdFrom == id)
                        {
                            m.Amount = -m.Amount;
                        }
                        models.Add(m);
                    }
                }
                var bankAccount = db.BankAccounts.Find(id);
                ViewBag.bankCode = bankAccount.BankCode;
                ViewBag.name = bankAccount.UserAccount.Name;
                ViewBag.balance = bankAccount.Balance;
                ViewBag.currency = bankAccount.Currency.Name;


                ViewBag.models = models;
                return View("Report");
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("report2s/{id}/{from}/{to}/{min}/{max}")]
        public IActionResult Report2(Guid id, DateTime from, DateTime to, decimal min, decimal max)
        {
            try
            {
                var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id).ToList();
                if (max == 0)
                {
                    model = model.Where(t =>
                        t.Time >= DateTime.Parse(from.ToString("yyyy/MM/dd"))
                        && t.Time <= DateTime.Parse(to.ToString("yyyy/MM/dd"))
                        && t.Amount >= min
                    ).ToList();
                }
                else
                {
                    model = model.Where(t =>
                        t.Time >= from
                        && t.Time <= to
                        && t.Amount >= min
                        && t.Amount <= max
                    ).ToList();
                }
                var models = new List<Transaction>();
                if (model != null)
                {
                    foreach (var m in model)
                    {
                        if (m.BankAccountIdFrom == id)
                        {
                            m.Amount = -m.Amount;
                        }
                        models.Add(m);
                    }
                }
                var bankAccount = db.BankAccounts.Find(id);
                ViewBag.bankCode = bankAccount.BankCode;
                ViewBag.name = bankAccount.UserAccount.Name;
                ViewBag.balance = bankAccount.Balance;
                ViewBag.currency = bankAccount.Currency.Name;


                ViewBag.models = models;
                return View("Report2");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("getmodel/{id}")]
        [HttpGet]
        public IActionResult GetModel(string id)
        {
            try
            {
                var guidId = Guid.Parse(id);
                var model = db.BankAccounts.SingleOrDefault(b => b.Id == guidId);
                return new JsonResult(model);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("getbankmodel/{code}")]
        [HttpGet]
        public IActionResult GetBankModel(string code)
        {
            try
            {
                var model = db.BankAccounts.FirstOrDefault(b => b.BankCode == code);
                return new JsonResult(model);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("quick-search/{id}/{value}")]
        [HttpGet]
        public IActionResult QuickSearch(string value, Guid id)
        {

            try
            {
                var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id).ToList();
                if (value.Equals("ten-day"))
                {
                    model.Where(t => t.Time >= t.Time.AddDays(-10)).ToList();
                }
                else if (value.Equals("one-month"))
                {
                    model.Where(t => t.Time >= t.Time.AddDays(-30)).ToList();
                }
                else if (value.Equals("three-months"))
                {
                    model.Where(t => t.Time >= t.Time.AddDays(-90)).ToList();
                }
                else
                {
                    model = null;
                }
                Debug.WriteLine("transaction id: " + id + value);
                var models = new List<Transaction>();
                if (model != null)
                {
                    foreach (var m in model)
                    {
                        if (m.BankAccountIdFrom == id)
                        {
                            m.Amount = -m.Amount;
                        }
                        models.Add(m);
                    }
                }
                model = models;
                return new JsonResult(model);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
        [Route("search/{id}/{from}/{to}/{min}/{max}")]
        [HttpGet]
        public IActionResult Search(Guid id, DateTime from, DateTime to, decimal min, decimal max)
        {

            try
            {
                Debug.WriteLine(from + "==" + to + "==" + min + "==" + max);
                var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id).ToList();
                if (max == 0)
                {
                    model = model.Where(t =>
                        t.Time >= DateTime.Parse(from.ToString("yyyy/MM/dd"))
                        && t.Time <= DateTime.Parse(to.ToString("yyyy/MM/dd"))
                        && t.Amount >= min
                    ).ToList();
                }
                else
                {
                    model = model.Where(t =>
                        t.Time >= from
                        && t.Time <= to
                        && t.Amount >= min
                        && t.Amount <= max
                    ).ToList();
                }
                var models = new List<Transaction>();
                if (model != null)
                {
                    foreach (var m in model)
                    {
                        if (m.BankAccountIdFrom == id)
                        {
                            m.Amount = -m.Amount;
                        }
                        models.Add(m);
                    }
                }
                model = models;
                return new JsonResult(model);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

    }
}
