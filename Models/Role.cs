using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Models
{
    public class Role : IdentityRole
    {
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
