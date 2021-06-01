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
    [Route("home")]
    public class HomeController : Controller
    {

        private DatabaseContext db;
        AccountObject account;
        List<BankAccount> bankAccounts;

        public HomeController(DatabaseContext _db)
        {
            db = _db;            
        }
        
        [Route("index")]
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                account = db.AccountObjects.SingleOrDefault(x => x.Username.Equals(HttpContext.Session.GetString("username")));
                bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).OrderByDescending(b => b.CreatedDate).ToList();
                ViewBag.bankAccounts = bankAccounts;
                ViewBag.banks = db.Banks.ToList();
                ViewBag.currencies = db.Currencies.ToList();
                return View();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("Login", "Login");
            }
        }
        [Route("index")]
        [HttpPost]
        public IActionResult Index(BankAccount bankAccount)
        {            
            try
            {
                account = db.AccountObjects.SingleOrDefault(x => x.Username.Equals(HttpContext.Session.GetString("username")));
                bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).OrderByDescending(b => b.CreatedDate).ToList();
                ViewBag.bankAccounts = bankAccounts;
                ViewBag.banks = db.Banks.ToList();
                ViewBag.currencies = db.Currencies.ToList();
                if(bankAccount.Id == Guid.Empty)
                {
                    bankAccount.Id = Guid.NewGuid();
                    var _account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
                    bankAccount.UserAccountId = new Guid(_account.Id.ToString());
                    var now = DateTime.Now;
                    bankAccount.CreatedDate = now;
                    bankAccount.ExpiredDate = now.AddYears(5);
                    bankAccount.Balance = 0;
                    bankAccount.Locked = false;
                    db.BankAccounts.Add(bankAccount);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Debug.WriteLine("add failed: " + e.Message);
                Debug.WriteLine("add failed inner: " + e.InnerException.Message);
                ViewBag.addFailed = "Duplicate name cannot be saved";
                return View("Index");
            }
        }
        /*[HttpGet]
        [Route("detail/{id}")]
        public IActionResult Detail(string id)
        {
            try
            {
                var bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).ToList();
                var guidId = Guid.Parse(id);
                var bankAccount = bankAccounts.Find(b => b.Id == guidId);
                return View(account);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("Login", "Login");
            }
        }*/
        [Route("account-detail/{id}")]
        public IActionResult AccountDetail(string id)
        {
            try
            {
                account = db.AccountObjects.SingleOrDefault(x => x.Username.Equals(HttpContext.Session.GetString("username")));
                bankAccounts = db.BankAccounts.Where(b => b.UserAccountId == account.Id).ToList();
                var guidId = Guid.Parse(id);
                var bankAccount = bankAccounts.Find(b => b.Id == guidId);
                dynamic model = new ExpandoObject();
                model.bankAccount = bankAccount;
                model.bankAccounts = bankAccounts;
                return View(model);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return RedirectToAction("Login", "Login");
            }

        }

        [Route("statement/{id}")]
        [HttpGet]
        public IActionResult Statement(Guid id)
        {

            try
            {
                var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id && t.Time >= t.Time.AddDays(-10)).ToList();
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
                Debug.WriteLine("failed:::::::::::: "  + e.InnerException.Message);
                return BadRequest(e.Message);
            }
        }


        [Route("reports/{id}")]
        public IActionResult Report(Guid id)
        {
            try
            {
                var model = db.Transactions.Where(t => t.BankAccountIdFrom == id || t.BankAccountIdTo == id && t.Time >= t.Time.AddDays(-10)).ToList();
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
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
