using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace SchoolManagementSystem.Models
{
public class User : IdentityUser
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }

}
