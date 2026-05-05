using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Data
{
    /// <summary>
    /// The main database context for the Student Management System.
    /// Inherits from EF Core's DbContext and acts as the bridge between
    /// the application and the MySQL database.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>Represents the Students table in the database.</summary>
        public DbSet<Student> Students => Set<Student>();

        /// <summary>Represents the Grades table in the database.</summary>
        public DbSet<Grade> Grades => Set<Grade>();
    }
}
