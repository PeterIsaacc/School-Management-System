using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Repositories
{
    public class ActivityRepository : SqlDbRepo<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext context) : base(context) { }
    }
}
