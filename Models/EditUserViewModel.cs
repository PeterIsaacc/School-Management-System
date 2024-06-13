using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModels
{
    public class EditUserViewModel
    {
    public string UserId { get; set; }
    public string? UserName { get; set; }
    public int UserRoleId { get; set; } 
    [EmailAddress]
    public string? Email { get; set; }   

    [Display(Name = "Role")]
    [Required]
    public string RoleName { get; set; }

    [Display(Name = "School Name")]
    public string? SchoolName { get; set; } 

    [Display(Name = "Activity Name")]
    public string? ActivityName { get; set; } 

    [Display(Name = "School")]
    public int? SchoolId { get; set; } 

    [Display(Name = "Activity")]
    public int? ActivityId { get; set; }

    [Display(Name = "Selected School")]
    public string? SelectedSchool { get; set; }   
    }
}