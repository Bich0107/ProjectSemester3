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
    [Route("superAdmin/staff")]
    public class StaffController : Controller
    {
        private DatabaseContext db;

        public StaffController(DatabaseContext _db)
        {
            db = _db;
        }

        #region Add, edit and delete
        [HttpPost]
        [Route("add")]
        public IActionResult Add(AccountObject _accountObj)
        {
            try
            {
                _accountObj.CreatedDate = DateTime.Today;
                _accountObj.Password = BCrypt.Net.BCrypt.HashString(_accountObj.Password);
                _accountObj.WrongPassword = 0;
                _accountObj.PositionId = 2;
                _accountObj.Staff = true;

                db.AccountObjects.Add(_accountObj);
                db.SaveChanges();

                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return new JsonResult(db.AccountObjects.Find(id));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(AccountObject _accountObj)
        {
            try
            {
                var accountObj = db.AccountObjects.Find(_accountObj.Id);

                accountObj.Name = _accountObj.Name;
                accountObj.Birthday = _accountObj.Birthday;
                accountObj.PhoneNumber = _accountObj.PhoneNumber;
                accountObj.Email = _accountObj.Email;
                accountObj.Address = _accountObj.Address;
                accountObj.Job = _accountObj.Job;
                accountObj.Gender = _accountObj.Gender;
                accountObj.IdNum = _accountObj.IdNum;
                accountObj.Locked = _accountObj.Locked;
                accountObj.IsAuthentication = _accountObj.IsAuthentication;

                db.SaveChanges();
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                db.AccountObjects.Remove(db.AccountObjects.Find(id));
                db.SaveChanges();
                return Ok(true);
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
            ViewBag.Title = db.Settings.Find(1).Title;
            ViewBag.mail = db.Helps.Find(1).Email;
            ViewBag.phone1 = db.Helps.Find(1).ContactNumber1;
            ViewBag.phone2 = db.Helps.Find(1).ContactNumber2;
            ViewBag.currencies = db.Currencies.ToList();

            ViewBag.tagName = "Staff";
            ViewBag.activeTag = "staff";
            ViewBag.activeParentTag = "sa";
            return View();
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                // return staffs who are not sa
                return new JsonResult(db.AccountObjects.Where(a => a.Staff && a.PositionId != (int) Roles.Superadmin).ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error account obj: " + e.Message);
                return null;
            }
        }

        // filter with a keyword on multiple columns
        [Route("search/{keyword}")]
        public IActionResult Search(string keyword)
        {
            try
            {
                return new JsonResult(db.AccountObjects.Where(
                    a => (a.Name.Contains(keyword) ||
                        a.Address.Contains(keyword) ||
                        a.PhoneNumber.Contains(keyword) ||
                        a.Email.Contains(keyword) ||
                        a.Username.Contains(keyword) ||
                        a.IdNum.Contains(keyword)) && a.Staff && a.PositionId != (int)Roles.Superadmin
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
