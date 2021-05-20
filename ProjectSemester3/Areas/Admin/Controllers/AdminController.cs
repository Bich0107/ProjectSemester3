using Microsoft.AspNetCore.Mvc;
using ProjectSemester3.Models;

namespace ProjectSemester3.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/admin")]
    public class AdminController : Controller
    {
        private DatabaseContext db;

        public AdminController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Title = db.Settings.Find(1).Title;
            return View();
        }
    }
}
