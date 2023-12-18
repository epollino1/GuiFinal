using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnisTracker.Models;

namespace FitnisTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index()
    {
        return View();
    }
    public IActionResult HomePage()
    {
        return View("HomePage");
    }
    public IActionResult Registration()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Registration(UserModel user)
    {
        if(!ModelState.IsValid)
        {
            return View(user);
        }
        return View("RegiResponse", user);
    }
   

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

