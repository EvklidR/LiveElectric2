namespace LiveElectric2.Server.Models
{
    public interface IMealPlanBuilder
    {
        void SetDays(int days);
        void AddDay(int day, double caloriesPercent, double proteinPercent, double fatPercent, double carbsPercent);
        MealPlan GetMealPlan();
    }

}
