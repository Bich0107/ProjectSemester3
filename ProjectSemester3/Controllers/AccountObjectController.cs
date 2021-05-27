using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{
    [Route("profile")]
    public class AccountObjectController : Controller
    {
        private DatabaseContext db;

        public AccountObjectController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("setting")]
        [Route("")]
        public IActionResult Setting()
        {
            return View("Setting");
        }
        [Route("getmodel/{id}")]
        [HttpGet]
        public IActionResult GetModel(string id)
        {
            var guidId = Guid.Parse(id);
            var model = db.AccountObjects.SingleOrDefault(b => b.Id == guidId);
            return new JsonResult(model);
        }
    }
}
