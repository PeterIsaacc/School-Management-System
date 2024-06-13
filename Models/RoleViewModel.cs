using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public List<User> Users { get; set; } = new List<User>();

    }
}
