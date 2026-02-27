using Microsoft.AspNetCore.Identity;

namespace GameZone.Models
{
     public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string imgUrl { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public bool Gender { get; set; } = false;
    }
}
