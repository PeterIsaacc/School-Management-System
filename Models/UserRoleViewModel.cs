using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.ViewModels
{
    public class UserRoleViewModel
    {
        public int UserRoleId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string SchoolName { get; set; }
        public string ActivityName { get; set; }
    }
}