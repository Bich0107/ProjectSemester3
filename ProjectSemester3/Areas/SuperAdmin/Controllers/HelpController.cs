using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("superAdmin")]
    [Route("superAdmin/help")]
    public class HelpController : Controller
    {
        private DatabaseContext db;

        public HelpController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Update(Help _help)
        {
            try
            {
                var help = db.Helps.Find(1);

                help.ContactNumber1 = _help.ContactNumber1;
                help.ContactNumber2 = _help.ContactNumber2;
                help.Address = _help.Address;
                help.Email = _help.Email;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.tagName = "Help";
            ViewBag.activeTag = "help";
            ViewBag.activeParentTag = "sa";
            return View("index"); 
        }

        // when there are no keyword, search is as same as loadData
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.Helps.Find(1));
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("changeEmailPassword")]
        public IActionResult ChangeEmailPassword(Help _help)
        {
            try
            {
                var help = db.Helps.Find(1);

                help.Password = _help.Password;

                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
