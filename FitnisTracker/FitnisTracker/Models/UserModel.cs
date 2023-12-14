using System;
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

		public double CurrentWeight { get; private set; }
		public double DesiredWeight { get; private set; }
		public double Height_In { get; private set; }
		public String Gender { get; private set; }
		public FitnisActivity ActivityLevel { get; private set; } 
		public DateTime Birthday { get; private set; }
		public int Age { get; } // this will be calculated not requested
		public int CalorieLimit { get; private set; }
		public String Name { get; private set; }
		public String Email { get; private set; }
		// feel free to add more

        public UserModel()
		{
			Gender = "";
			Name = "";
			Email = "";
		}
	}
}

