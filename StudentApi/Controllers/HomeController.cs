using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;

namespace StudentApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) => _db = db;

        // GET /
        public async Task<IActionResult> Index()
        {
            ViewBag.StudentCount    = await _db.Students.CountAsync();
            ViewBag.CourseCount     = await _db.Courses.CountAsync();
            ViewBag.GradeCount      = await _db.Grades.CountAsync();
            ViewBag.AttendanceCount = await _db.Attendances.CountAsync();
            return View();
        }
    }
}
