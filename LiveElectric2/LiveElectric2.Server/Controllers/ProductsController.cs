using LiveElectric2.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LiveElectric2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProductsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {

            return await _context.Products
                .Include(p => p.Ingredients)   // Загрузка связанных ингредиентов
                .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Ingredients)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(string name, List<int> ingredientIds)
        {
            // Проверяем, передано ли название продукта
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            // Проверяем, что передан список ID ингредиентов
            if (ingredientIds == null || !ingredientIds.Any())
            {
                return BadRequest("Ingredient list cannot be empty.");
            }

            // Находим ингредиенты по переданным ID
            var ingredients = await _context.Ingredients
                .Where(i => ingredientIds.Contains(i.Id))
                .ToListAsync();

            // Проверяем, все ли ингредиенты были найдены
            if (ingredients.Count != ingredientIds.Count)
            {
                return BadRequest("Some ingredients were not found.");
            }

            // Создаем новый продукт с переданными ингредиентами
            Product product = new Product
            {
                Name = name,
                Ingredients = ingredients
            };

            // Добавляем продукт в базу данных
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Возвращаем созданный продукт
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }



        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

