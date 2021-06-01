using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSemester3.Controllers
{    
    [Route("contact")]
    public class ContactController : Controller
    {
        private DatabaseContext db;

        public ContactController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("save")]
        public IActionResult Save(Problem _problem)
        {
            try
            {
                var account = db.AccountObjects.FirstOrDefault(a => a.Username.Equals(HttpContext.Session.GetString("username")));
                if(_problem.Question == null)
                {
                    return Json(new { success = false, Error = "Please Write a question!" });
                }
                else
                {
                    var problem = new Problem();
                    problem.Question = _problem.Question;
                    problem.Name = account.Name;
                    problem.Sender = account.Email;
                    problem.ReceivedDate = DateTime.Now;
                    db.Problems.Add(problem);
                    db.SaveChanges();
                    
                }
                return Json(new { success = true, Error = "Your question has been submitted successfully!" });

            }
            catch (Exception e)
            {
                Debug.WriteLine("Send question Failed: " + e.InnerException.Message);
                return Json(new { success = false, Error = "Something went wrong. Please try again" });
            }
            
        }
    }
}
