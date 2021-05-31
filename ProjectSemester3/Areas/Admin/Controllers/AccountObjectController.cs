using System;
using System.Linq;
using ProjectSemester3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    public class MinAccount
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Area("admin")]
    [Route("admin/accountObject")]
    public class AccountObjectController : Controller
    {
        private DatabaseContext db;

        public AccountObjectController(DatabaseContext _db)
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
                _accountObj.PositionId = 1;
                _accountObj.Staff = false;

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
            ViewBag.tagName = "Account Object";
            ViewBag.activeTag = "accountObj";
            return View("index");
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.AccountObjects.Where(a => !a.Staff).ToList());
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
                        a.IdNum.Contains(keyword)) && !a.Staff
                ).ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error account obj: " + e.Message);
                return null;
            }
        }

        // find account object by username (unique)
        [Route("find/{username}")]
        public IActionResult Find(string username)
        {
            try
            {
                return new JsonResult(db.AccountObjects.Where(
                    a => a.Username.Equals(username)).SingleOrDefault());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error account obj: " + e.Message);
                return null;
            }
        }

        [HttpPost]
        [Route("checkPassword")]
        public bool CheckPassword(MinAccount _account)
        {
            var account = db.AccountObjects.Where(a => a.Username.Equals(_account.Username)).SingleOrDefault();

            return BCrypt.Net.BCrypt.Verify(_account.Password, account.Password);
        }
    }
}
