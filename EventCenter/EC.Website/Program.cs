using EC.ApplicationServices.Implementations;
using EC.ApplicationServices.Interfaces;
using EC.Data.Contexts;
using EC.Repositories.Implementations;
using EC.Repositories.Interfaces;
using Lucene.Net.Support;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
using System.Configuration;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Register the UsersManagementService and its dependencies
//builder.Services.AddScoped<IUsersManagementService, UsersManagementService>();
//builder.Services.AddScoped<IUsersRepository, UsersRepository>();

//builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IUsersManagementService, UsersManagementService>(c =>
c.BaseAddress = new Uri("https://localhost:7182/"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });




// Other service registrations
//builder.Services.AddControllers();
//builder.Services.AddTransient<EventCenterDbContext>();

builder.Services.AddScoped<DbContext, EventCenterDbContext>();

// Register the UsersManagementService and its dependencies
builder.Services.AddScoped<IUsersManagementService, UsersManagementService>();
builder.Services.AddScoped<IEventsManagementService, EventsManagementService>();
builder.Services.AddScoped<ITicketsManagementService, TicketsManagementService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();

// Other service registrations
builder.Services.AddControllers();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();






