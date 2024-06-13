using SchoolManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Services
{
    public interface IRoleService : IGenericService<Role>
    {
        Task<IdentityResult> CreateRoleAsync(Role role);
        Task<IdentityResult> UpdateRoleAsync(Role role);
        Task<IdentityResult> DeleteRoleAsync(Role role);
    }
}
