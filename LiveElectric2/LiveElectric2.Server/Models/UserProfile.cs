using LiveElectric2.Server.Models;

namespace LiveElectric2.Server.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required User User { get; set; }  // Связь с сущностью User
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int Age { get; set; }
        public char Gender { get; set; }
        public double ActivityLevel { get; set; } // уровень активности (1.2 - малоподвижный, 1.375 - низкий, 1.55 - средний, 1.725 - высокий)

        // Суточная норма калорий (рассчитывается через формулу Харриса-Бенедикта)
        public double CalculateBMR()
        {
            if (Gender == 'M')
            {
                return 88.36 + (13.4 * Weight) + (4.8 * Height) - (5.7 * Age);
            }
            else
            {
                return 447.6 + (9.2 * Weight) + (3.1 * Height) - (4.3 * Age);
            }
        }

        // Умножаем на коэффициент активности для получения суточной нормы калорий
        public double CalculateDailyCalories()
        {
            return CalculateBMR() * ActivityLevel;
        }

        // Примерные пропорции КБЖУ (протеины, жиры, углеводы)
        public (double Protein, double Fat, double Carbs) CalculateDailyMacros()
        {
            double calories = CalculateDailyCalories();
            double protein = 0.3 * calories / 4; // 30% белков, 1 грамм белка = 4 ккал
            double fat = 0.3 * calories / 9; // 30% жиров, 1 грамм жира = 9 ккал
            double carbs = 0.4 * calories / 4; // 40% углеводов, 1 грамм углеводов = 4 ккал
            return (protein, fat, carbs);
        }
    }

}
