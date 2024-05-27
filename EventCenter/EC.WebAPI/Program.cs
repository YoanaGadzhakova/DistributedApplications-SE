using EC.ApplicationServices.Implementations;
using EC.ApplicationServices.Interfaces;
using EC.Data.Contexts;
using EC.Data.Entities;
using EC.Repositories.Implementations;
using EC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
    .Build();
// Add services to the container.

var connectionString = configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<EventCenterDbContext>(options => options.UseSqlServer(connectionString,
            x => x.MigrationsAssembly("EC.WebAPI")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Start SERVICE DI
builder.Services.AddScoped<DbContext, EventCenterDbContext>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUsersManagementService, UsersManagementService>();
builder.Services.AddScoped<ITicketsManagementService, TicketsManagementService>();
builder.Services.AddScoped<IEventsManagementService, EventsManagementService>();


// End SERVICE DI

var app = builder.Build();
builder.Services.AddEndpointsApiExplorer();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

SeedAdminUser(app);

app.Run();

void SeedAdminUser(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EventCenterDbContext>();

    // Check if any admin user exists in the database
    var adminExists = dbContext.Users.Any(u => u.IsAdmin);

    // If no admin user exists, add an admin user
    if (!adminExists)
    {
        var admin = new User
        {
            Username = "admin",
            Password = "adminpass", // Note: You should hash passwords in a real-world scenario
            IsAdmin = true,
            CreatedBy = 1,
            CreatedOn = DateTime.Now,
            IsActivated = true,
            FirstName = "admin",
            LastName = "admin",
            Email = "admin"
        };
        dbContext.Users.Add(admin);
        dbContext.SaveChanges();
    }
}
