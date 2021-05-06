using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/accountObject")]
    public class AccountObjectController : Controller
    {
        private DatabaseContext db;

        public AccountObjectController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("add")]
        [Route("")]
        [Route("~/")]
        // for testing only
        public IActionResult Add()
        {
            var accountObject = new AccountObject
            {
                Id = Guid.NewGuid(),
                Name = "James",
                Birthday = new DateTime(2000, 12, 28),
                PhoneNumber = "0123456789",
                Email = "mail1@gmail.com",
                Address = "address 2",
                Job = "job 2",
                Gender = Gender.Male,
                IdNum = "123456789",
                Staff = false,
                PositionId = 1,
                Username = "username2",
                Password = BCrypt.Net.BCrypt.HashString("123"),
                CreatedDate = DateTime.Today,
                Locked = false
            };

            db.AccountObjects.Add(accountObject);
            db.SaveChanges();
            return View();
        }
    }
}
