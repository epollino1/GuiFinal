using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnisTracker.Models;

namespace FitnisTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FitnisContext _context;

    public HomeController(ILogger<HomeController> logger, FitnisContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Index()
    {
        ViewData["UserName"] = User.Identity.Name;
        User user = _context.Users.Where(a => a.Email.Equals(User.Identity.Name)).ToArray()[0];
        WeightLog log = _context.WeightLogs.Where(a => a.UserId.Equals(user.UserId)).OrderByDescending(a => a.LoggedAt).First();
        ViewData["WeightStart"] = user.StartingWeight;
        ViewData["WeightGoal"] = user.DesiredWeight;
        ViewData["WeightCurr"] = log.CurrentWeight;

        return View(user);
    }
    public IActionResult HomePage()
    {
        //if (User.Identity.IsAuthenticated)
        //{
        //    return RedirectToAction("LoggedHome", "CaloryLog");
        //}
        return View();
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

