using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnisTracker.Models;

public enum FitnisActivityEnum
{
    BasalMetabolicRate, //(BMR)
    Sedentary, // little or no exercise
    Light, // exercise 1-3 times/week
    Moderate, // exercise 4-5 times/week
    Active, // daily exercise or intense exercise 3-4 times/week
    VeryActive,// intense exercise 6-7 times/week
    ExtraActive// very intense exercise daily, or physical job

}
public partial class User
{
    [Key]
    public string UserId { get; set; } = null;

    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public double? StartingWeight { get; set; }

    [Required]
    public double? CurrentWeight { get; set; }

    [Required]
    public double? DesiredWeight { get; set; }

    [Required]
    public double? HeightIn { get; set; }

    [Required]
    public string? Gender { get; set; }

    [Required]
    public byte[]? Birthday { get; set; }

    public long Age
    {
        get
        {
            if (Birthday == null || Birthday.Length != 3) // Assuming 3 bytes: year, month, day
            {
                return -1; // Invalid age
            }

            int year = Birthday[0];
            int month = Birthday[1];
            int day = Birthday[2];

            DateTime birthdayDate = new DateTime(year, month, day);
            DateTime today = DateTime.Today;
            int age = today.Year - birthdayDate.Year;

            if (birthdayDate > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }

    [Required]
    public long? CalorieLimit { get; set; }

    [Required]
    public string? Activity { get; set; }


    public virtual ICollection<CaloryLog>? CaloryLogs { get; set; } = new List<CaloryLog>();

    public virtual ICollection<WeightLog>? WeightLogs { get; set; } = new List<WeightLog>();
}
