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
    [Route("admin/currency")]
    public class CurrencyController : Controller
    {
        private DatabaseContext db;

        public CurrencyController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var currency = new Currency();
            return View("add", currency);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Currency currency)
        {
            db.Currencies.Add(currency);
            db.SaveChanges();
            return View("add");
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Add(int id)
        {
            var currency = db.Currencies.Find(id);
            return View("update", currency);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(Currency _currency)
        {
            var currency = db.Currencies.Find(_currency.Id);

            currency.Name = _currency.Name;
            currency.Fullname = _currency.Fullname;
            currency.ExchangeRate = _currency.ExchangeRate;
            currency.Default = _currency.Default;

            db.SaveChanges();

            return View("update");
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            db.Currencies.Remove(db.Currencies.Find(id));
            db.SaveChanges();
            return View("delete");
        }
    }
}
