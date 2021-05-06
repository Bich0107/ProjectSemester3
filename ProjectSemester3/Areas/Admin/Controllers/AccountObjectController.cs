using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/accountObject")]
    public class AccountObjectController : Controller
    {
        private DatabaseContext db;

        public AccountObjectController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var account = new AccountObject
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Today
            };
            return View("add", account);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(AccountObject _accountObj)
        {
            _accountObj.Password = BCrypt.Net.BCrypt.HashString(_accountObj.Password);
            db.AccountObjects.Add(_accountObj);
            db.SaveChanges();

            return View("add");
        }
    }
}
