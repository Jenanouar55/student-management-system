using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>Represents the Students table in the database.</summary>
        public DbSet<Student> Students => Set<Student>();

        /// <summary>Represents the Courses table in the database.</summary>
        public DbSet<Course> Courses => Set<Course>();

        /// <summary>Represents the Grades table in the database.</summary>
        public DbSet<Grade> Grades => Set<Grade>();

        /// <summary>Represents the Attendance table in the database.</summary>
        public DbSet<Attendance> Attendances => Set<Attendance>();
    }
}
