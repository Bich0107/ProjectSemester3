using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    public class CurrencyAPI
    {
        public string id { get; set; }
        public string currencyName { get; set; }
        public string currencySymbol { get; set; }
    }

    public class Result
    {
        public List<CurrencyAPI> Results { get; set; }
    }

    [Area("admin")]
    [Route("admin/currency")]
    public class CurrencyController : Controller
    {
        private DatabaseContext db;
        private static string currencyAPIKey = "f4383dc2ba2dcc94a2d6";

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
            ViewBag.tagName = "Currency";
            ViewBag.activeTag = "currency";

            ViewBag.settings = db.Settings.Find(1);
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
            catch (Exception)
            {
                return null;
            }
        }

        [Route("updateAll")]
        public IActionResult UpdateAll()
        {
            try
            {
                // get default currency
                string baseCurrency = db.Currencies.Find(db.Settings.Find(1).DefaultCurrencyId).Name;

                foreach (var currency in db.Currencies.ToList())
                {
                    using (WebClient web = new WebClient())
                    {
                        string url = string.Format("https://free.currconv.com/api/v7/convert?q={0}_{1}&compact=ultra&apiKey={2}", currency.Name.ToUpper(),
                            baseCurrency.ToUpper(), currencyAPIKey);
                        string response = web.DownloadString(url);
                        response = response.Remove(response.Length - 1);
                        string[] result = response.Split(':');

                        currency.ExchangeRate = float.Parse(result[1]);
                    }
                }

                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
            catch (Exception)
            {
                return null;
            }
        }
    }
}
