using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Services
{
    public class SchoolService : GenericService<School>, ISchoolService
    {
        public SchoolService(ISchoolRepository repository) : base(repository) { }
    }
}
