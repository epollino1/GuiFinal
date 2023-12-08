namespace FitnisTests;
using FitnisTracker.Models;

public class UnitTest1
{
    public UserModel Person;

    [Fact]
    public void Test1()
    {

    }

    // Test UserModel
    [Fact]
    public void TextConstructor()
    {
        
    }

    [Fact]
    public void TestDataSet1()
    {
        int age = 19;
        int feet = 5;
        int inch = 11;
        inch += feet * 12;
        double WeightLb = 130;
        Activity activity = Activity.Sedentary;
        int CaloriesMaintain = 1952;
        int CaloriesWeightLoss = 1452;

    }
}
