using LiveElectric2.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveElectric2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public IngredientsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }

        // GET: api/Ingredients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ingredient>> GetIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            return ingredient;
        }

        // POST: api/Ingredients
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(string name, int calories, double proteins, double fats, double carbohydrates)
        {
            // Проверяем, что название не пустое
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Ingredient name cannot be empty.");
            }

            // Создаем новый объект Ingredient с переданными значениями
            Ingredient ingredient = new Ingredient
            {
                Name = name,
                Calories = calories,
                Proteins = proteins,
                Fats = fats,
                Carbohydrates = carbohydrates
            };

            // Добавляем ингредиент в контекст и сохраняем изменения
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            // Возвращаем созданный ингредиент
            return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.Id }, ingredient);
        }


        // PUT: api/Ingredients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient(int id, Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return BadRequest();
            }

            _context.Entry(ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Ingredients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.Id == id);
        }
    }
}
