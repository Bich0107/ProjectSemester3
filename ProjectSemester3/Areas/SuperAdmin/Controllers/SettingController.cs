using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.SuperAdmin.Controllers
{
    [Area("superAdmin")]
    [Route("superAdmin/setting")]
    public class SettingController : Controller
    {
        private DatabaseContext db;

        public SettingController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Update(Setting _setting)
        {
            try
            {
                var setting = db.Settings.Find(1);

                setting.DefaultCurrencyId = _setting.DefaultCurrencyId;
                setting.Title = _setting.Title;
                setting.DefaultNumOfShowedTransaction = _setting.DefaultNumOfShowedTransaction;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.Settings.Find(1));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
            ViewBag.currencies = db.Currencies.ToList();

            ViewBag.tagName = "Setting";
            ViewBag.activeTag = "setting";
            ViewBag.activeParentTag = "sa";
            return View();
        }
    }
}
