using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Models
{
    public class UserRole: IdentityUserRole<string>
    {
        public int UserRoleId { get; set; }
        public Role Role { get; set; }
        public int? SchoolId { get; set; }
        public School? School { get; set; }
        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }
    }
}