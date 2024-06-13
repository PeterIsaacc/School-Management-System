using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolManagementSystem.ViewModels;

namespace SchoolManagementSystem.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserRole>> GetAllUsersAsync();
        Task<UserRole> GetUserByIdAsync(string id);
        Task UpdateUserAsync(EditUserViewModel model);
        Task DeleteUserAsync(string id);
    }
}
