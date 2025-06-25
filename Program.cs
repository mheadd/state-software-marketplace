global using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add EF Core and PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No connection string found. Please ensure either DB_CONNECTION_STRING environment variable or DefaultConnection in appsettings.json is set.");
}

Console.WriteLine($"Using connection string: {connectionString}");

builder.Services.AddDbContext<state_software_marketplace.Data.ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

var app = builder.Build();

// Apply migrations and then seed the database
try 
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<state_software_marketplace.Data.ApplicationDbContext>();
        context.Database.Migrate(); // Ensure migrations are applied
        state_software_marketplace.Data.SeedData.Initialize(services);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while migrating or seeding the database: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
    throw; // Re-throw to stop application startup if we can't set up the database
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
