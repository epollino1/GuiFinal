using System;
namespace FitnisTracker.Models
{
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

        [Required]
        public Activity ActivityLevel { get; set; }

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

