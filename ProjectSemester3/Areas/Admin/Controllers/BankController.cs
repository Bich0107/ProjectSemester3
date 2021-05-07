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
    [Route("admin/bank")]
    public class BankController : Controller
    {
        private DatabaseContext db;

        public BankController(DatabaseContext _db)
        {
            db = _db;
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var bank = new Bank();
            return View("add", bank);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Bank bank)
        {
            db.Banks.Add(bank);
            db.SaveChanges();
            return View("add");
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult Add(int id)
        {
            var bank = db.Banks.Find(id);
            bank.Id = id;
            return View("update", bank);
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update(Bank _bank)
        {
            var bank = db.Banks.Find(_bank.Id);

            Debug.WriteLine(_bank.Id + "\n" + _bank.Name + "\n" + _bank.Description);


            bank.Name = _bank.Name;
            bank.Description = _bank.Description;
            bank.Address = _bank.Address;
            bank.Status = _bank.Status;

            db.SaveChanges();

            return View("update");
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            db.Banks.Remove(db.Banks.Find(id));
            db.SaveChanges();
            return View("delete");
        }
    }
}
