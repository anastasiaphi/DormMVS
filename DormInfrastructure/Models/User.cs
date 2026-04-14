using Microsoft.AspNetCore.Identity;

namespace DormInfrastructure.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}

