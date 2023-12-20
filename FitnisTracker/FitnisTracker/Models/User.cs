using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnisTracker.Models;

public partial class User
{
    
    public string UserId { get; set; } = null!;
    
    public string? Username { get; set; }
   
    public string? Email { get; set; }
   
    public double? StartingWeight { get; set; }
   
    public double? CurrentWeight { get; set; }
   
    public double? DesiredWeight { get; set; }
    
    public double? HeightIn { get; set; }
  
    public string? Gender { get; set; }
   
    public byte[]? Birthday { get; set; }
   
    public long? Age { get; set; }
   
    public long? CalorieLimit { get; set; }
    
    public string? Activity { get; set; }

    public virtual ICollection<CaloryLog> CaloryLogs { get; set; } = new List<CaloryLog>();

    public virtual ICollection<WeightLog> WeightLogs { get; set; } = new List<WeightLog>();

    public double CalculateBMR(string Gender, double currentWeight, double age, double height)
    {
      
        if (Gender == "male")
        {
            return 88.362 + (13.397 * currentWeight) +
                   (4.799 * height) - (5.677 * age);
        }
        else if (Gender == "female")
        {
            return 447.593 + (9.247 * currentWeight) +
                   (3.098 * height) - (4.330 * age);
        }
        return 0;

    }
    public void CalculateCalorieIntakeForWeightLoss()
    {
        double bmr = CalculateBMR(this.Gender, (double)this.CurrentWeight, (double)this.Age, (double)this.HeightIn);
        double activityFactor = 1.0;
        if (Activity == null)
        {
            activityFactor = 1.0;
        }
        else
        {
            switch (this.Activity.ToLower()) // Convert input to lowercase for easier comparison
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

        double calorieDeficitPerDay = 2 * 7700 / 7; // 2 lbs = 7700 calories
        double calorieIntakeForWeightLoss = (bmr * activityFactor) - calorieDeficitPerDay;

        CalorieLimit = (long)calorieIntakeForWeightLoss;
    }
}

