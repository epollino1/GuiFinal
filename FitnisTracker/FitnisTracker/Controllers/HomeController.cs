using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnisTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        _logger.LogInformation("\n\n\nDog Shit\n\n");

        User user = _context.Users.FirstOrDefault(a => a.Email.Equals(User.Identity.Name));

        if (user != null)
        {
            _logger.LogInformation("\n\n\nUser Found\n\n");

            ViewData["UserName"] = user.Username;
            ViewData["WeightStart"] = user.StartingWeight;
            ViewData["WeightGoal"] = user.DesiredWeight;

            WeightLog log = _context.WeightLogs.Where(a => a.UserId.Equals(user.UserId))
                                                .OrderByDescending(a => a.LoggedAt)
                                                .FirstOrDefault();
            
            if (log != null)
            {
                ViewData["WeightCurr"] = log.CurrentWeight;

                _logger.LogInformation(
                    $"User: {ViewData["UserName"]}\n weight: {ViewData["WeightStart"]}\n goal: {ViewData["WeightGoal"]}");
                    
            }
            else
            {
                ViewData["ErrorMessage"] = "No weight log found for the user.";
                _logger.LogError(ViewData["ErrorMessage"].ToString());

            }
        }
        else
        {
            ViewData["ErrorMessage"] = "User not found.";

            return RedirectToAction("Login", "Account");
        }

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


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    private bool UserExists(string id)
    {
        return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
    }
    public double CalculateBMR(User user)
    {
        if (user.Gender == "Male")
        {
            return 88.362 + (13.397 * (double)user.CurrentWeight) +
                   (4.799 * (double)user.HeightIn) - (5.677 * (double)user.Age);
        }
        else if (user.Gender == "Female")
        {
            return 447.593 + (9.247 * (double)user.CurrentWeight) +
                   (3.098 * (double)user.HeightIn) - (4.330 * (double)user.Age);
        }

        return 0;
    }
    public long CalculateCalorieIntakeForWeightLoss(User user)
    {
        double bmr = CalculateBMR(user);

        
        double calorieDeficitPerDay = 2 * 7700 / 7; // 2 lbs = 7700 calories
        double calorieIntakeForWeightLoss = bmr - calorieDeficitPerDay;

        return (long)calorieIntakeForWeightLoss;
    }
}


