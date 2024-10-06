using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiveElectric2.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;



namespace LiveElectric2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _context;


        public UsersController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            // Получаем email текущего пользователя
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            return Ok(new { email });
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }



        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(User user)
        {
            // Находим пользователя
            User? person = await _context.Users.FirstOrDefaultAsync(p => p.Email == user.Email && p.Password == user.Password);
            if (person is null) return Unauthorized();

            // Создаем JWT-токен
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Login) };
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            Response.Cookies.Append("access_token", encodedJwt, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { username = person.Email });
        }
  

        [HttpGet("check")]
        public async Task<IActionResult> CheckConnection()
        {
            try
            {
                // Попробуйте выполнить запрос к базе данных.
                await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                _context.Database.Migrate();
                return Ok("Connection successful");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Database connection failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
