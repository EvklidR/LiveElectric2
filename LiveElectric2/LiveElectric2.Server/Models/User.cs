using LiveElectric2.Server.Models;

namespace LiveElectric2.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }

        public UserProfile? UserProfile { get; set; }
    }

}
