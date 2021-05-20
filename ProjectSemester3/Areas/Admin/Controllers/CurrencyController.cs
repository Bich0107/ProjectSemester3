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

        #region Add, edit, delete
        [HttpPost]
        [Route("add")]
        public IActionResult Add(Currency currency)
        {
            try
            {
                db.Currencies.Add(currency);
                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            return new JsonResult(db.Currencies.Find(id));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Update(Currency _currency)
        {
            try
            {
                var currency = db.Currencies.Find(_currency.Id);

                currency.Name = _currency.Name;
                currency.Fullname = _currency.Fullname;
                currency.ExchangeRate = _currency.ExchangeRate;
                currency.Default = _currency.Default;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                db.Currencies.Remove(db.Currencies.Find(id));
                db.SaveChanges();
                return Ok();
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
            ViewBag.tagName = "Currency";
            ViewBag.activeTag = "currency";
            return View("index");
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.Currencies.ToList());
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [Route("search/{keyword}")]
        public IActionResult Search(string keyword)
        {
            try
            {
                return new JsonResult(db.Currencies.Where(
                    a => a.Name.Contains(keyword) ||
                        a.Fullname.Contains(keyword)
                ).ToList());
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
