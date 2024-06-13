using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class RoleRepository : SqlDbRepo<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }
    }
}
