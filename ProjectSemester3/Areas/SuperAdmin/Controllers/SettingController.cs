using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        [Route("update")]
        public IActionResult Update()
        {
            var setting = db.Settings.Find(1);

            return View("update", setting);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(Setting _setting)
        {
            var setting = db.Settings.Find(1);

            setting.DefaultCurrencyId = _setting.DefaultCurrencyId;
            setting.Title = _setting.Title;
            setting.DefaultNumOfShowedTransaction = _setting.DefaultNumOfShowedTransaction;

            db.SaveChanges();
            return View("update");
        }
    }
}
