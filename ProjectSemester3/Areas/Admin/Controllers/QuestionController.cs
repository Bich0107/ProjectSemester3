using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Helpers;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/question")]
    public class QuestionController : Controller
    {
        private DatabaseContext db;

        public QuestionController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            return new JsonResult(db.Problems.Find(id));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Update(Problem _problem)
        {
            try
            {
                var problem = db.Problems.Find(_problem.Id);

                // get current login staff
                var username = HttpContext.Session.GetString("username");

                // testing only
                username = string.IsNullOrEmpty(username) ? "superAdmin" : username;

                var currentStaff = db.AccountObjects.Where(a => a.Username.Equals(username) && a.Staff).SingleOrDefault();

                // update database
                problem.AnswerDate = DateTime.Now;
                problem.AnswererId = currentStaff.Id;
                problem.Answer = _problem.Answer;

                // get default contact info
                var contact = db.Helps.Find(1);

                // send mail
                MailHelper mailHelper = new MailHelper();
                var result = mailHelper.Send(currentStaff.Name, contact.Email, contact.Password, problem.Name, problem.Sender, contact.Subject, problem.Answer);

                db.SaveChanges();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.tagName = "Customer problems";
            ViewBag.activeTag = "customerProb";

            // get current login staff
            var username = HttpContext.Session.GetString("username");

            // testing only
            username = string.IsNullOrEmpty(username) ? "superAdmin" : username;

            var currentStaff = db.AccountObjects.Where(a => a.Username.Equals(username) && a.Staff).SingleOrDefault();
            ViewBag.email = currentStaff.Email;
            return View("index");
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                // order by received date, lastest to oldest
                return new JsonResult(db.Problems.OrderByDescending(q => q.ReceivedDate).ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                return null;
            }
        }

        [Route("search/{keyword}")]
        public IActionResult Search(string keyword)
        {
            try
            {
                // order by received date, lastest to oldest
                return new JsonResult(db.Problems.
                    Where(p => p.Answer.Contains(keyword) || 
                        p.Question.Contains(keyword))
                    .OrderByDescending(q => q.ReceivedDate)
                    .ToList());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e.Message);
                return null;
            }
        }
    }
}
