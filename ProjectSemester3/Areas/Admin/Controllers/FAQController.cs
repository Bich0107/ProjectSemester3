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
    [Route("admin/faq")]
    public class FAQController : Controller
    {
        private DatabaseContext db;

        public FAQController(DatabaseContext _db)
        {
            db = _db;
        }
        #region Add, edit, delete
        [HttpPost]
        [Route("add")]
        public IActionResult Add(Faq faq)
        {
            try
            {
                db.Faqs.Add(faq);
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
            return new JsonResult(db.Faqs.Find(id));
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Update(Faq _faq)
        {
            try
            {
                var faq = db.Faqs.Find(_faq.Id);
                faq.Subject = _faq.Subject;
                faq.Description = _faq.Description;

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
                db.Faqs.Remove(db.Faqs.Find(id));
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
            ViewBag.tagName = "FAQ";
            ViewBag.activeTag = "faq";
            return View("index");
        }

        // when there are no keyword, search is as same as loadData
        [Route("search")]
        [Route("loadData")]
        public IActionResult LoadData()
        {
            try
            {
                return new JsonResult(db.Faqs.ToList());
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
                return new JsonResult(db.Faqs.Where(
                    a => a.Subject.Contains(keyword)
                ).ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
