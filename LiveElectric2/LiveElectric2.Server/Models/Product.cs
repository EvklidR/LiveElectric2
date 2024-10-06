using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveElectric2.Server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Ingredient>? Ingredients { get; set; } = new List<Ingredient>();

        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        public int TotalCalories()
        {
            int ingredientCalories = Ingredients.Sum(i => i.Calories);
            return ingredientCalories;
        }

        public double TotalProteins()
        {
            double ingredientProteins = Ingredients.Sum(i => i.Proteins);
            return ingredientProteins;
        }

        public double TotalFats()
        {
            double ingredientFats = Ingredients.Sum(i => i.Fats);
            return ingredientFats;
        }

        public double TotalCarbohydrates()
        {
            double ingredientCarbs = Ingredients.Sum(i => i.Carbohydrates);
            return ingredientCarbs;
        }
    }

}
