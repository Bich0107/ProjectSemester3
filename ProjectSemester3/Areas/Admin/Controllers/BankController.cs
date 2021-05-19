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

        #region Add, Edit, Delete
        [HttpPost]
        [Route("add")]
        public IActionResult Add(Bank bank)
        {
            try
            {
                db.Banks.Add(bank);
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
            return new JsonResult(db.Banks.Find(id));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(Bank _bank)
        {
            try
            {
                var bank = db.Banks.Find(_bank.Id);

                bank.Name = _bank.Name;
                bank.Description = _bank.Description;
                bank.Address = _bank.Address;
                bank.Status = _bank.Status;

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
                db.Banks.Remove(db.Banks.Find(id));
                db.SaveChanges();
                return Ok();
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
            ViewBag.activeTag = "bank";
            ViewBag.tagName = "Bank";
            return View("index");
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.Banks.ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error bank: " + e.Message);
                return null;
            }
        }

        [Route("search/{keyword}")]
        public IActionResult Search(string keyword)
        {
            try
            {
                return new JsonResult(db.Banks.Where(
                    a => a.Name.Contains(keyword) 
                ).ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error account obj: " + e.Message);
                return null;
            }
        }
    }
}
