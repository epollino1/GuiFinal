using System;
using System.ComponentModel.DataAnnotations;

namespace FitnisTracker.Models
{

    public enum FitnisActivity
    {
        BasalMetabolicRate, //(BMR)
        Sedentary, // little or no exercise
        Light, // exercise 1-3 times/week
        Moderate, // exercise 4-5 times/week
        Active, // daily exercise or intense exercise 3-4 times/week
        VeryActive,// intense exercise 6-7 times/week
        ExtraActive// very intense exercise daily, or physical job

    }
    public class UserModel
    {
        [Required]
        public double CurrentWeight { get; set; }

        [Required]
        public double DesiredWeight { get; set; }

        [Required]
        public double Height_In { get; set; }

        [Required]
        public string Gender { get; set; }

        
        public String ActivityLevel { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - Birthday.Year;
                if (Birthday.Date > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        [Required]
        public int CalorieLimit { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public UserModel()
		{
			
		}
	}
}

