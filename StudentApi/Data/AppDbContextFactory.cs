using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentApi.Data
{
    /// <summary>
    /// Allows EF Core CLI tools (dotnet ef migrations add) to instantiate
    /// AppDbContext without needing a live MySQL connection.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Port=3306;Database=StudentDb;User=root;Password=placeholder;",
                ServerVersion.Parse("8.0.0-mysql"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
