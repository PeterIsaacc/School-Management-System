using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class School
    {
        [Key]
        [Display(Name = "School ID")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
        public ICollection<Activity>? Activities { get; set; } 
    }
}