using Ci_Platform.Repositories.Interfaces;
using Ci_Platform.Repositories.Repositories;
using CI_Platform.Entities.DataModels;
using CI_Platform.Entities.ViewModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    )); ;
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IFilters, Filters>();
builder.Services.AddScoped<IMission, MissionRepository>();
builder.Services.AddScoped<IStory, StoryRepository>();
builder.Services.AddScoped<IUserProfile, UserProfile>();
builder.Services.AddScoped<ITimesheet, TimesheetRepository>();
builder.Services.AddScoped<IAdmin, AdminRepository>();

// for use of session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
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
