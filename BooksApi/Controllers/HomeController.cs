using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers;

public class HomeController : Controller
{
 
    public IActionResult Index()
    {
        return View();
    }

    
}