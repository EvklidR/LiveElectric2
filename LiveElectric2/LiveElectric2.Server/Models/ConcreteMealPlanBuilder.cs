using LiveElectric2.Server.Models;

namespace LiveElectric2.Server.Models
{
    public class ConcreteMealPlanBuilder : IMealPlanBuilder
    {
        private MealPlan _mealPlan;
        private UserProfile _user;

        public ConcreteMealPlanBuilder(UserProfile user)
        {
            _user = user;
        }

        public void SetDays(int days)
        {
            _mealPlan = new MealPlan(days);
        }

        public void AddDay(int day, double caloriesPercent, double proteinPercent, double fatPercent, double carbsPercent)
        {
            var dailyCalories = _user.CalculateDailyCalories();
            var (dailyProtein, dailyFat, dailyCarbs) = _user.CalculateDailyMacros();

            double calories = dailyCalories * caloriesPercent;
            double protein = dailyProtein * proteinPercent;
            double fat = dailyFat * fatPercent;
            double carbs = dailyCarbs * carbsPercent;

            _mealPlan.AddDay(day, calories, protein, fat, carbs);
        }

        public MealPlan GetMealPlan()
        {
            return _mealPlan;
        }
    }

}
