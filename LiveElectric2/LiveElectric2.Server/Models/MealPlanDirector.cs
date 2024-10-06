namespace LiveElectric2.Server.Models
{
    public class MealPlanDirector
    {
        private readonly IMealPlanBuilder _builder;

        public MealPlanDirector(IMealPlanBuilder builder)
        {
            _builder = builder;
        }

        public void BuildDefaultMealPlan(int days)
        {
            // Настройка плана на заданное количество дней
            _builder.SetDays(days);

            // Строим план с процентами по умолчанию
            for (int i = 1; i <= days; i++)
            {
                _builder.AddDay(i, 1.0, 0.25, 0.25, 0.50); // по умолчанию 25% белков, 25% жиров, 50% углеводов
            }
        }

        public void BuildCustomMealPlan(int days, Dictionary<int, (double CaloriesPercent, double ProteinPercent, double FatPercent, double CarbsPercent)> percentages)
        {
            _builder.SetDays(days);

            foreach (var day in percentages)
            {
                _builder.AddDay(day.Key, day.Value.CaloriesPercent, day.Value.ProteinPercent, day.Value.FatPercent, day.Value.CarbsPercent);
            }
        }

        public MealPlan GetMealPlan()
        {
            return _builder.GetMealPlan();
        }
    }

}
