using KineStat.Data;
using KineStat.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
// Configuration of the DbContext

builder.Services.AddDbContext<KineDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("KineDbContext")));


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add sessions support for authentication
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add HttpContextAccessor for accessing session in controllers
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<PatientAnonymizationService>();

builder.Services.AddHostedService<PatientAnonymizationBackgroundService>();

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

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
