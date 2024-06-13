// using SchoolManagementSystem.Models;
// using SchoolManagementSystem.Repositories;
// using SchoolManagementSystem.ViewModels;
// using Microsoft.AspNetCore.Identity;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;

// namespace SchoolManagementSystem.Services
// {
//     public class UserService : GenericService<User>, IUserService
//     {
//         private readonly UserManager<User> _userManager;
//         private readonly RoleManager<Role> _roleManager;
//         private readonly ApplicationDbContext _context;

//         public UserService(IUserRepository repository, UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context) 
//             : base(repository)
//         {
//             _userManager = userManager;
//             _roleManager = roleManager;
//             _context = context;
//         }

//         public async Task<IEnumerable<UserRole>> GetUserRolesAsync()
//         {
//             return await _context.UserRoles
//                 .Include(u => u.Role)
//                 .Include(u => u.School)
//                 .Include(u => u.Activity)
//                 .ToListAsync();
//         }

//         public async Task<EditUserViewModel> GetEditUserViewModelAsync(int id)
//         {
//             var userRole = await _context.UserRoles
//                 .Include(ur => ur.User)
//                 .Include(ur => ur.Role)
//                 .Include(ur => ur.School)
//                 .Include(ur => ur.Activity)
//                 .FirstOrDefaultAsync(ur => ur.Id == id);

//             if (userRole == null)
//             {
//                 return null;
//             }

//             var model = new EditUserViewModel
//             {
//                 Id = userRole.User.Id,
//                 Email = userRole.User.Email,
//                 RoleName = userRole.Role.Name,
//                 SchoolId = userRole.SchoolId,
//                 ActivityId = userRole.ActivityId
//             };

//             if (model.RoleName == "School Admin" || model.RoleName == "Activity Admin")
//             {
//                 model.Schools = await _context.Schools.ToListAsync();
//             }

//             if (model.RoleName == "Activity Admin")
//             {
//                 model.Activities = await _context.Activities.ToListAsync();
//             }

//             return model;
//         }

//         public async Task<bool> UpdateUserAsync(EditUserViewModel model)
//         {
//             var user = await _userManager.FindByIdAsync(model.Id);
//             if (user == null)
//             {
//                 return false;
//             }

//             user.Email = model.Email;
//             var result = await _userManager.UpdateAsync(user);
//             if (!result.Succeeded)
//             {
//                 return false;
//             }

//             var currentUserRoles = await _userManager.GetRolesAsync(user);
//             var roleToRemove = currentUserRoles.FirstOrDefault();
//             if (!string.IsNullOrEmpty(roleToRemove))
//             {
//                 await _userManager.RemoveFromRoleAsync(user, roleToRemove);
//             }

//             var newRole = await _roleManager.FindByNameAsync(model.RoleName);
//             if (newRole != null)
//             {
//                 await _userManager.AddToRoleAsync(user, model.RoleName);
//             }

//             if (model.RoleName == "School Admin" || model.RoleName == "Activity Admin")
//             {
//                 var userRole = _context.UserRoles.FirstOrDefault(ur => ur.User.Id == user.Id);
//                 if (userRole != null)
//                 {
//                     userRole.SchoolId = model.SchoolId;
//                     if (model.RoleName == "Activity Admin")
//                     {
//                         userRole.ActivityId = model.ActivityId;
//                     }
//                     await _context.SaveChangesAsync();
//                 }
//             }

//             return true;
//         }

//         public async Task<bool> DeleteUserAsync(string id)
//         {
//             var user = await _userManager.FindByIdAsync(id);
//             if (user == null)
//             {
//                 return false;
//             }

//             var userRoles = _context.UserRoles.Where(ur => ur.User.Id == id);
//             _context.UserRoles.RemoveRange(userRoles);
//             await _context.SaveChangesAsync();

//             var result = await _userManager.DeleteAsync(user);
//             return result.Succeeded;
//         }
//     }
// }
