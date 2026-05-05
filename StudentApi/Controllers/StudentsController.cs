using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    /// <summary>
    /// REST API controller for managing student records.
    /// All routes are prefixed with /api/students.
    ///
    /// Endpoints:
    ///   GET    /api/students        → list all students
    ///   GET    /api/students/{id}   → get one student by ID
    ///   POST   /api/students        → create a new student
    ///   PUT    /api/students/{id}   → update an existing student
    ///   DELETE /api/students/{id}   → delete a student
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        // Database context injected by ASP.NET Core's dependency injection system.
        // Used to query and save student data.
        private readonly AppDbContext _db;

        /// <summary>
        /// Constructor — ASP.NET Core automatically passes the AppDbContext instance here.
        /// </summary>
        public StudentsController(AppDbContext db) => _db = db;

        /// <summary>
        /// GET /api/students
        /// Returns a list of all students in the database.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Student>> GetAll() =>
            await _db.Students.ToListAsync(); // ToListAsync runs the SQL SELECT asynchronously

        /// <summary>
        /// GET /api/students/{id}
        /// Returns a single student by their ID.
        /// Returns 404 Not Found if no student with that ID exists.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var s = await _db.Students.FindAsync(id); // FindAsync looks up by primary key
            return s is null ? NotFound() : Ok(s);
        }

        /// <summary>
        /// POST /api/students
        /// Creates a new student record from the JSON body.
        /// Returns the saved student (including the generated ID) with 200 OK.
        ///
        /// Example request body:
        /// {
        ///   "fullName": "Jane Doe",
        ///   "email": "jane@example.com",
        ///   "enrollmentDate": "2024-09-01T00:00:00Z"
        /// }
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(Student s)
        {
            _db.Students.Add(s);          // Marks the student for insertion
            await _db.SaveChangesAsync(); // Executes the INSERT SQL
            return Ok(s);                 // Returns the student with the DB-assigned ID
        }

        /// <summary>
        /// PUT /api/students/{id}
        /// Updates the name, email, and enrollment date of an existing student.
        /// Returns 404 Not Found if the student doesn't exist.
        ///
        /// Example request body:
        /// {
        ///   "fullName": "Jane Smith",
        ///   "email": "janesmith@example.com",
        ///   "enrollmentDate": "2024-09-01T00:00:00Z"
        /// }
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student updated)
        {
            var s = await _db.Students.FindAsync(id);
            if (s is null) return NotFound();

            // Overwrite only the editable fields — we never change the ID
            s.FullName = updated.FullName;
            s.Email = updated.Email;
            s.EnrollmentDate = updated.EnrollmentDate;

            await _db.SaveChangesAsync(); // Executes the UPDATE SQL
            return Ok(s);
        }

        /// <summary>
        /// DELETE /api/students/{id}
        /// Removes a student record from the database permanently.
        /// Returns 404 Not Found if the student doesn't exist.
        /// Returns 200 OK on successful deletion.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _db.Students.FindAsync(id);
            if (s is null) return NotFound();

            _db.Students.Remove(s);       // Marks the record for deletion
            await _db.SaveChangesAsync(); // Executes the DELETE SQL
            return Ok();
        }
    }
}
