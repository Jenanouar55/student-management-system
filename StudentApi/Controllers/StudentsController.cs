using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public StudentsController(AppDbContext db) => _db = db;
        [HttpGet]
        public async Task<IEnumerable<Student>> GetAll() =>
            await _db.Students.ToListAsync(); 
         [HttpGet("{id}")]
           public async Task<IActionResult> Get(int id)
        {
            var ziad = await _db.Students.FindAsync(id);
            return ziad is null ? NotFound() : Ok(ziad);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Student ziad)
        {
            _db.Students.Add(ziad);          
            await _db.SaveChangesAsync(); 
            return Ok(ziad);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Student updated)
        {
            var ziad = await _db.Students.FindAsync(id);
            if (ziad is null) return NotFound();
            ziad.FullName = updated.FullName;
            ziad.Email = updated.Email;
            ziad.EnrollmentDate = updated.EnrollmentDate;

            await _db.SaveChangesAsync();
            return Ok(ziad);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ziad = await _db.Students.FindAsync(id);
            if (ziad is null) return NotFound();

            _db.Students.Remove(ziad);   
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
