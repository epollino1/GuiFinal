using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnisTracker.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Diagnostics;

namespace FitnisTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly FitnisContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(FitnisContext context, UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            //return RedirectToAction("Index", "Home");
            return _context.Users != null ?
                         View(await _context.Users.ToListAsync()) :
                         Problem("Entity set 'FitnisContext.Users'  is null.");


        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            return RedirectToAction("Index", "Home");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Email,StartingWeight,CurrentWeight,DesiredWeight,HeightIn,Gender,Birthday,Age,CalorieLimit,Activity")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit()
        {
            //var id = await _userManager.GetUserIdAsync(account);
            //var email = await _userManager.GetEmailAsync(account);
            string userEmail = User.FindFirst(ClaimTypes.Email)?.Value;


            //_logger.Log(LogLevel.Information, "ID: {id}", id);
            //_logger.Log(LogLevel.Information, "Email: {userEmail}", userEmail);
            //_logger.Log(LogLevel.Information, "Context: {_context.Users}", _context.Users);

            if (_context.Users == null || userEmail == null)
            {
                _logger.Log(LogLevel.Error, "something is null");
                return NotFound();
            }

            //var user = await _context.Users.FindAsync(id);
            //var user = await _context.Users.FindAsync(userEmail);
            User user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                _logger.LogError("No user found");
                return NotFound();
            }

            //_logger.LogInformation(user.UserId);

            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("UserId,Username,Email,HeightIn,Gender")] User user, [Bind("Birthday")] DateTime birthday)
        {

            if (ModelState.IsValid)
            {
                user.Birthday = BitConverter.GetBytes(birthday.Ticks);
                int age = CalculateAge(birthday);
                user.Age = age;


                try
                {
                    _logger.Log(LogLevel.Information, "Trying to update");

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(user);
            }
            return View(user);
        }


        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            return RedirectToAction("Index", "Home");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'FitnisContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;


            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }


        //get
        public async Task<IActionResult> Registration()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("StartingWeight,DesiredWeight,Activity")] User user)
        {

            User CurrUser = _context.Users.FirstOrDefault(a => a.Email.Equals(User.Identity.Name));
            CurrUser.StartingWeight = user.StartingWeight;
            CurrUser.DesiredWeight = user.DesiredWeight;
            CurrUser.CurrentWeight = user.StartingWeight;

            CurrUser.Activity = user.Activity;
            user = CurrUser;

            _logger.LogInformation(user.UserId);
            try
            {
                _logger.Log(LogLevel.Information, "Trying to update");

                _context.Update(user);
                await _context.SaveChangesAsync();


                CurrUser.CalorieLimit = CalculateCalorieIntakeForWeightLoss(CurrUser);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("RegiResponse");

            //return View();
        }

        public IActionResult RegiResponse()
        {
            string userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            _logger.Log(LogLevel.Information, "Email: {userEmail}", userEmail);

            if (_context.Users == null || userEmail == null)
            {
                _logger.Log(LogLevel.Error, "something is null");
                return NotFound();
            }
            User user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                _logger.LogError("No user found");
                return NotFound();
            }

            return View(user);
        }

        public double CalculateBMR(User user)
        {
            User CurrUser = _context.Users.FirstOrDefault(a => a.Email.Equals(User.Identity.Name));
            user.Age = CurrUser.Age;
            user.HeightIn = CurrUser.HeightIn;
            user.CurrentWeight = CurrUser.CurrentWeight;


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


        }
        public long CalculateCalorieIntakeForWeightLoss(User user)
        {
            User CurrUser = _context.Users.FirstOrDefault(a => a.Email.Equals(User.Identity.Name));
            user.Activity = CurrUser.Activity;

            double bmr = CalculateBMR(user);
            double calorieIntakeForWeightLoss;
            double calorieDeficitPerDay;

            double activityFactor = 1.0;
            if (user.Activity == null)
            {
                activityFactor = 1.0;
            }
            else
            {
                switch (user.Activity.ToLower()) // Convert input to lowercase for easier comparison
                {
                    case "sedentary":
                        activityFactor = 1.2;
                        break;
                    case "light":
                        activityFactor = 1.375;
                        break;
                    case "moderate":
                        activityFactor = 1.55;
                        break;
                    case "active":
                        activityFactor = 1.725;
                        break;
                    case "veryactive":
                        activityFactor = 1.9;
                        break;
                    case "extraactive":
                        activityFactor = 2.0;
                        break;
                    default:
                        activityFactor = 1.0; // base BMR
                        break;
                }

            }

            calorieDeficitPerDay = 2 * 7700 / 7; // 2 lbs = 7700 calories
            calorieIntakeForWeightLoss = (bmr * activityFactor) - calorieDeficitPerDay;

            return (long)calorieIntakeForWeightLoss;
        }
    }


}