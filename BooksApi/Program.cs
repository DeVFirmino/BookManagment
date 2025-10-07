using Microsoft.EntityFrameworkCore;
using BooksApi.Data;
using BooksApi.Services.BookService;
using BooksApi.Data;
using BooksApi.Services.BookService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookInterface, BookService>();

builder.Services.AddAutoMapper(typeof(Program)); //mapping t

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Book}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();