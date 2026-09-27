using Microsoft.AspNetCore.Mvc;
using AthleteFitnessApp.Data;
using AthleteFitnessApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AthleteFitnessApp.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutController(ApplicationDbContext context) => _context = context;

        public IActionResult Dashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(userId);
            return View(user);
        }

        public IActionResult Admin()
        {
            string? userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != "admin@admin.com") return RedirectToAction("Dashboard");

            var allMembers = _context.Users.ToList();
            return View(allMembers);
        }

        public IActionResult LevelSelection()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");
            var user = _context.Users.Find(userId);
            return View(user);
        }

        [HttpPost]
        public IActionResult SavePersonalInfo(double? weight, double? height, int? age, string goal)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.Weight = weight;
                user.Height = height;
                user.Age = age;
                user.Goal = goal ?? string.Empty;
                _context.SaveChanges();
            }
            return RedirectToAction("LevelSelection");
        }

        public IActionResult Hub(string level, int day = 1)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            var schedule = Get7DayPlan(level);
            var currentDaySchedule = schedule.FirstOrDefault(d => d.DayNumber == day) ?? schedule[0];

            ViewBag.Level = level;
            ViewBag.CurrentDay = day;
            ViewBag.FullSchedule = schedule;
            ViewBag.User = _context.Users.Find(userId);

            return View(currentDaySchedule);
        }

        [HttpPost]
        public IActionResult CompleteWorkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Json(new { success = false });

            var user = _context.Users.Find(userId);
            if (user == null) return Json(new { success = false });

            var today = DateTime.Today;

            if (user.LastWorkoutDate == null || user.LastWorkoutDate.Value.Date == today.AddDays(-1))
            {
                user.DailyStreak += 1;
            }
            else if (user.LastWorkoutDate.Value.Date < today.AddDays(-1))
            {
                user.DailyStreak = 1;
            }

            user.TotalWorkoutsCompleted += 1;
            user.LastWorkoutDate = today;

            _context.SaveChanges();

            return Json(new { success = true, newStreak = user.DailyStreak, totalCompleted = user.TotalWorkoutsCompleted });
        }

        private List<WorkoutSchedule> Get7DayPlan(string level)
        {
            var days = new List<WorkoutSchedule>();
            string[] muscleGroups = { "Chest & Triceps", "Back & Biceps", "Legs & Abs", "Shoulders & Core", "Full Body HIIT", "Rest Day", "Cardio & Recovery" };

            for (int i = 1; i <= 7; i++)
            {
                bool isRest = (i == 6);
                days.Add(new WorkoutSchedule
                {
                    DayNumber = i,
                    DayTitle = $"Day {i}: {muscleGroups[i - 1]}",
                    IsRestDay = isRest,
                    WorkTimeSeconds = level == "Pro" ? 45 : (level == "Intermediate" ? 35 : 25),
                    RestTimeSeconds = level == "Pro" ? 15 : (level == "Intermediate" ? 25 : 35),
                    Exercises = isRest ? "Active Recovery: Light Walking & Body Stretching" : $"{level} Routine: Pushups, Squats, Mountain Climbers, Planks (4 Sets)",
                    VegNutrition = isRest ? "Rest Day Veg: Oatmeal, Almonds, Greek Yogurt." : "Workout Veg: Tofu Stir-Fry, Paneer Rice, Peanut Butter Toast, Protein Shake.",
                    NonVegNutrition = isRest ? "Rest Day Non-Veg: Boiled Eggs, Salmon Salad." : "Workout Non-Veg: Grilled Chicken Breast, Sweet Potato, Whey Shake."
                });
            }
            return days;
        }
    }
}