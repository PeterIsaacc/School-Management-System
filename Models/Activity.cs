using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class Activity
    {
        [Key]
        [Display(Name = "Activity ID")]
        public int ActivityId { get; set; }
        [Display(Name = "School")]
        public int? SchoolId { get; set; } 
        public School? School { get; set; } 
        [Required]
        [Display(Name = "Activity Name")]
        public string ActivityName { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}