using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class SchoolRepository : SqlDbRepo<School>, ISchoolRepository
    {
        public SchoolRepository(ApplicationDbContext context) : base(context) { }
    }
}
