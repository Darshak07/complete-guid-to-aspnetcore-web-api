using Microsoft.AspNetCore.Identity;

namespace my_books.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Custom { get; set; }
    }
}
