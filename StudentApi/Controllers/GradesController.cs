using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    /// <summary>
    /// REST API controller for managing student grades (notes).
    /// All routes are prefixed with /api/grades.
    ///
    /// Endpoints:
    ///   GET    /api/grades                   → list all grades
    ///   GET    /api/grades/student/{id}      → list grades for one student
    ///   POST   /api/grades                   → add a new grade
    ///   PUT    /api/grades/{id}              → update a grade
    ///   DELETE /api/grades/{id}              → delete a grade
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public GradesController(AppDbContext db) => _db = db;

        /// <summary>
        /// GET /api/grades
        /// Returns all grades, with the linked student name included.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<object>> GetAll() =>
            await _db.Grades
                .Include(g => g.Student)   // JOIN with Students table
                .Select(g => new {
                    g.Id, g.StudentId,
                    studentName = g.Student!.FullName,
                    g.Subject, g.Score, g.Note, g.Date
                })
                .ToListAsync();

        /// <summary>
        /// GET /api/grades/student/{studentId}
        /// Returns all grades belonging to a specific student.
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IEnumerable<Grade>> GetByStudent(int studentId) =>
            await _db.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

        /// <summary>
        /// POST /api/grades
        /// Adds a new grade record.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Grade g)
        {
            _db.Grades.Add(g);
            await _db.SaveChangesAsync();
            return Ok(g);
        }

        /// <summary>
        /// PUT /api/grades/{id}
        /// Updates subject, score, note, and date of an existing grade.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Grade updated)
        {
            var g = await _db.Grades.FindAsync(id);
            if (g is null) return NotFound();
            g.StudentId = updated.StudentId;
            g.Subject   = updated.Subject;
            g.Score     = updated.Score;
            g.Note      = updated.Note;
            g.Date      = updated.Date;
            await _db.SaveChangesAsync();
            return Ok(g);
        }

        /// <summary>
        /// DELETE /api/grades/{id}
        /// Removes a grade record permanently.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var g = await _db.Grades.FindAsync(id);
            if (g is null) return NotFound();
            _db.Grades.Remove(g);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
