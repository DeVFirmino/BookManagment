/*
------------------------------------------------------------
 ------------------------------------------------------------
Purpose
• Configures dependency injection, EF Core, session, AutoMapper,
  and the ASP.NET Core middleware pipeline for the app.

Key registrations
• DbContext: AppDbContext with SQL Server (DefaultConnection).
• Services: Book, User, Home (auth), Borrow, Report, Session,
  plus AuthenticationService for hashing/verification.
• HttpContextAccessor: singleton for session access.
• AutoMapper: scans assemblies for mapping profiles.
• Session: enables server-side session state (cookie is essential).

Middleware pipeline
• Error handling + HSTS (Production).
• HTTPS redirection + static files (serves /wwwroot, images, css, js).
• Routing → Session → Authorization.
• MVC route: {controller=Home}/{action=Index}/{id?}.
• MapStaticAssets/WithStaticAssets: supports static asset mapping.

Result
• Application is ready to serve MVC pages with authentication,
  session-managed login, reports, and book lending features.
------------------------------------------------------------
*/

using Microsoft.EntityFrameworkCore;
using BooksApi.Data;
using BooksApi.Services.Authentication;
using BooksApi.Services.BookService;
using BooksApi.Services.BorrowService;
using BooksApi.Services.HomeService;
using BooksApi.Services.ReportService;
using BooksApi.Services.UserService;
using BooksApi.Services.SessionService;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IBookInterface, BookService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<ISessionInterface, SessionService>();
builder.Services.AddScoped<IAuthenticationInterface, AuthenticationService>();
builder.Services.AddScoped<IHomeInterface, HomeService>();
builder.Services.AddScoped<IBorrowInterface, BorrowService>();
builder.Services.AddScoped<IReportInterface, ReportService>();

builder.Services.AddAutoMapper(typeof(Program)); //mapping t

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseStaticFiles(); // ← Save images

app.UseRouting();          // <--
app.UseSession();          // <-- session needs routing before it
app.UseAuthorization();


app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();