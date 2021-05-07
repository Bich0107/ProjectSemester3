using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/help")]
    public class HelpController : Controller
    {
        private DatabaseContext db;

        public HelpController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("Update")]
        public IActionResult Update()
        {
            var help = db.Helps.Find(1);

            return View("update", help);
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(Help _help)
        {
            var help = db.Helps.Find(1);

            help.ContactNumber1 = _help.ContactNumber1;
            help.ContactNumber2 = _help.ContactNumber2;
            help.Address = _help.Address;
            help.Email = _help.Email;

            db.SaveChanges();

            return View("update");
        }
    }
}
