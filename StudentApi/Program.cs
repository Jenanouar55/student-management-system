using Microsoft.EntityFrameworkCore;
using StudentApi.Data;

// ── Builder ───────────────────────────────────────────────────────────────────
// WebApplication.CreateBuilder sets up configuration, logging, and DI container.
var builder = WebApplication.CreateBuilder(args);

// Read the MySQL connection string from appsettings.json → ConnectionStrings → DefaultConnection
var conn = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Register AppDbContext with the DI container.
// UseMySql tells EF Core to use the Pomelo MySQL provider.
// ServerVersion.AutoDetect connects briefly to read the server version at startup.
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySql(conn, ServerVersion.AutoDetect(conn)));

// Register MVC controllers (scans for classes marked with [ApiController])
builder.Services.AddControllers();

// ── App / Pipeline ────────────────────────────────────────────────────────────
var app = builder.Build();

// Run any pending EF Core migrations automatically when the app starts.
// This creates or updates the Students table without needing to run
// "dotnet ef database update" manually.
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();

// Serve wwwroot/index.html when the browser requests "/"
app.UseDefaultFiles();

// Serve all static files inside the wwwroot folder (HTML, CSS, JS, images, etc.)
app.UseStaticFiles();

// Map incoming HTTP requests to the matching controller action methods
app.MapControllers();

// Start the web server and begin listening for requests
app.Run();
