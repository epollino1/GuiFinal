using FitnisTracker.Models;
using System.Diagnostics;
using Xunit;

public class CalorieCalculationTests
{

    [Fact]
    public void TestCalorieMaintenanceCalculation()
    {
        User user = new User();
        user.HeightIn = 74;
        user.Age = 20;
        user.CurrentWeight = 230;
        user.DesiredWeight = 210;
        user.Activity = "sedentary";



        user.CalculateCalorieIntakeForWeightLoss();

        
        double expectedCalories = 2549; 
        assert.Equal(expectedCalories, user.CalorieLimit, 2); 
    }

    [Fact]
    public void TestCalorieWeightLossCalculation()
    {
        // Arrange
        int age = 25;
        double weightLb = 176; // 80 kg converted to pounds
        int heightInch = 71; // 180 cm converted to inches
        Activity activityLevel = Activity.High; // Adjust as needed

        // Act
        double calculatedCalories = CalorieCalculator.CalculateWeightLossCalories(age, weightLb, heightInch, activityLevel);

        // Assert
        double expectedCalories = 1800; // Replace with the expected value based on your algorithm
        Assert.Equal(expectedCalories, calculatedCalories, 2); // Adjust delta for precision
    }
}
