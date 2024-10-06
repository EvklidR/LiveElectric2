namespace LiveElectric2.Server.Models
{
    public class MealPlan
    {
        public int Days { get; set; } // количество дней плана
        public Dictionary<int, (double Calories, double Protein, double Fat, double Carbs)> DailyPlan { get; set; }

        public MealPlan(int days)
        {
            Days = days;
            DailyPlan = new Dictionary<int, (double, double, double, double)>();
        }

        public void AddDay(int day, double calories, double protein, double fat, double carbs)
        {
            DailyPlan[day] = (calories, protein, fat, carbs);
        }

    }

}
