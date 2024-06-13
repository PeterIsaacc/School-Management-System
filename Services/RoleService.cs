using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Services
{
    public class RoleService : GenericService<Role>, IRoleService
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleService(IRoleRepository repository, RoleManager<Role> roleManager)
            : base(repository)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(Role role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(Role role)
        {
            var existingRole = await _roleManager.FindByIdAsync(role.Id);
            if (existingRole == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role not found" });
            }
            existingRole.Name = role.Name;
            return await _roleManager.UpdateAsync(existingRole);
        }

        public async Task<IdentityResult> DeleteRoleAsync(Role role)
        {
            return await _roleManager.DeleteAsync(role);
        }
    }
}
