using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Customer.Controllers
{
    [Area("customer")]
    [Route("customer/transaction")]
    public class TransactionController : Controller
    {
        private DatabaseContext db;

        public TransactionController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var bank = new Transaction
            {
                Time = DateTime.Now,
            };
            return View("add", bank);
        }

        // wait for session/current logging customer
    }
}
