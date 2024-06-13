using SchoolManagementSystem.Models;
using SchoolManagementSystem.Repositories;

namespace SchoolManagementSystem.Services
{
    public class ActivityService : GenericService<Activity>, IActivityService
    {
        public ActivityService(IActivityRepository repository) : base(repository) { }
    }
}
