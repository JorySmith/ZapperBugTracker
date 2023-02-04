using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Data;
using ZapperBugTracker.Models;
using ZapperBugTracker.Services;
using ZapperBugTracker.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add Npgsql
builder.Services.AddDbContext<ApplicationDbContext>(options =>
     options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Change to just AddIdentity, pass type ZUser and IdentityRole
// Add default ui and token providers since removed AddDefaultIdentity
builder.Services.AddIdentity<ZUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Custom Services (register interfaces): User Roles, Company Info, and Project  
builder.Services.AddScoped<IZAPRolesService, ZAPRolesService>();
builder.Services.AddScoped<IZAPCompanyInfoService, ZAPCompanyInfoService>();
builder.Services.AddScoped<IZAPProjectService, ZAPProjectService>();
builder.Services.AddScoped<IZAPTicketService, ZAPTicketService>();
builder.Services.AddScoped<IZAPTicketHistoryService, ZAPTicketHistoryService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
