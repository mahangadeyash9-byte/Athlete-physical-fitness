using System;
using System.ComponentModel.DataAnnotations;

namespace AthleteFitnessApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public int DailyStreak { get; set; } = 0;
        public int TotalWorkoutsCompleted { get; set; } = 0;
        public DateTime? LastWorkoutDate { get; set; }
        
        // Personal Information
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public int? Age { get; set; }
        public string Goal { get; set; } = string.Empty;
    }

    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class WorkoutSchedule
    {
        public int DayNumber { get; set; }
        public string DayTitle { get; set; } = string.Empty;
        public bool IsRestDay { get; set; }
        public string Exercises { get; set; } = string.Empty;
        public int WorkTimeSeconds { get; set; }
        public int RestTimeSeconds { get; set; }
        public string VegNutrition { get; set; } = string.Empty;
        public string NonVegNutrition { get; set; } = string.Empty;
    }
}
