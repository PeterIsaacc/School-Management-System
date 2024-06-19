using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.ViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace SchoolManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string userId, int userRoleId)
        {
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .Include(ur => ur.School)
                .Include(ur => ur.Activity)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.UserRoleId == userRoleId);
            var userName = _userManager.FindByIdAsync(userId).Result.Email.Split('@')[0];

            if (userRole == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                UserId = userRole.UserId,
                UserName = userName,
                RoleName = userRole.Role.Name,
                SchoolName = userRole.School?.SchoolName,
                ActivityName = userRole.Activity?.ActivityName
            };

            ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name", model.RoleName);
            ViewBag.SchoolName = new SelectList(_context.Schools, "SchoolName", "SchoolName", model.SchoolName);
            ViewBag.ActivityName = new SelectList(_context.Activities, "ActivityName", "ActivityName", model.ActivityName);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRole = await _context.UserRoles.FindAsync(model.UserRoleId);
                if (userRole == null)
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(model.UserId);
                user.Email = user.Email;
                user.UserName = user.Email;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync(model.RoleName);
                    if (role != null)
                    {
                        userRole.RoleId = role.Id;

                        if (model.RoleName == "School Admin")
                        {
                            var school = _context.Schools.FirstOrDefault(s => s.SchoolName == model.SchoolName);
                            if (school == null)
                            {
                                ModelState.AddModelError("", "School not found.");
                                return View(model);
                            }
                            userRole.SchoolId = school.Id;
                        }
                        else if (model.RoleName == "Activity Admin")
                        {
                            var school = _context.Schools.FirstOrDefault(s => s.SchoolName == model.SchoolName);
                            if (school != null)
                            {
                                var activity = _context.Activities.FirstOrDefault(a => a.ActivityName == model.ActivityName && a.SchoolId == school.Id);
                                if (activity == null)
                                {
                                    ModelState.AddModelError("", "This school doesn't have the specified activity.");
                                    return View(model);
                                }
                                userRole.SchoolId = school.Id;
                                userRole.ActivityId = activity.ActivityId;
                            }
                            else
                            {
                                ModelState.AddModelError("", "School not found.");
                                return View(model);
                            }
                        }
                        else if (model.RoleName == "Super Admin")
                        {
                            userRole.SchoolId = null;
                            userRole.ActivityId = null;
                        }

                        try
                        {
                            _context.UserRoles.Update(userRole);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateException ex)
                        {
                            if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                            {
                                ModelState.AddModelError("", "User Role already exists.");
                                return View(model);
                            }
                            throw;
                        }
                        TempData["SuccessMessage"] = "User updated successfully.";
                        return RedirectToAction("UserRoles", "User", new { id = model.UserId });                
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name", model.RoleName);
            ViewBag.SchoolName = new SelectList(_context.Schools, "SchoolName", "SchoolName", model.SchoolName);
            ViewBag.ActivityName = new SelectList(_context.Activities, "ActivityName", "ActivityName", model.ActivityName);
            return View(model);
        }
        [HttpGet]
        public IActionResult GetActivities(List<string> schoolNames)
        {
            var activities = _context.Schools
                .Include(s => s.Activities)
                .Where(s => schoolNames.Contains(s.SchoolName))
                .SelectMany(s => s.Activities.Select(a => a.ActivityName))
                .Distinct()
                .ToList();

            return Json(activities);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            return View(user);
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await _context.UserRoles.Where(ur => ur.UserId == user.Id).ToListAsync();
                _context.UserRoles.RemoveRange(userRoles);
        
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(user);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
            }
        [HttpGet]
        public async Task<IActionResult> UserRoles(string Id)
        {
            var userRoles = await _context.UserRoles
                                          .Where(ur => ur.UserId == Id)
                                          .Include(ur => ur.Role)
                                          .Include(ur => ur.School)
                                          .Include(ur => ur.Activity)
                                          .ToListAsync();
            var user = await _userManager.FindByIdAsync(Id);
            var userName = _userManager.FindByIdAsync(user.Id).Result.Email.Split('@')[0];
            List<UserRoleViewModel> model;

            if (!userRoles.Any())
            {
                model = new List<UserRoleViewModel>
                {
                    new UserRoleViewModel
                    {
                        UserId = Id,
                        UserName = userName
                    }
                };
            }
            else
            {
                model = userRoles.Select(ur => new UserRoleViewModel
                {
                    UserRoleId = ur.UserRoleId,
                    UserId = ur.UserId,
                    UserName = userName,
                    RoleName = ur.Role.Name,
                    SchoolName = ur.School?.SchoolName,
                    ActivityName = ur.Activity?.ActivityName
                }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteUserRole(int userRoleId)
        {
            var userRole = await _context.UserRoles.FindAsync(userRoleId);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "User role deleted successfully.";
            return RedirectToAction("UserRoles", "User", new { Id = userRole.UserId });
        }
        [HttpGet]
        public async Task<IActionResult> Create(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userName = _userManager.FindByIdAsync(user.Id).Result.Email.Split('@')[0];

            var model = new EditUserViewModel
            {
                UserId = userId,
                UserName = userName

            };
        
            var schools = _context.Schools.ToList();
            var rolesInOrder = new List<string> { "Super Admin", "School Admin", "Activity Admin" };
            ViewBag.RoleId = new SelectList(rolesInOrder, model.RoleName);    
                
            var firstSchool = schools.FirstOrDefault();
            if (firstSchool != null)
            {
                ViewData["SchoolName"] = new SelectList(schools, "SchoolName", "SchoolName", firstSchool.SchoolName);
            }
            else
            {
                ViewData["SchoolName"] = new SelectList(schools, "SchoolName", "SchoolName");
            }
        
            ViewBag.ActivityName = new SelectList(_context.Activities, "ActivityName", "ActivityName");
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> Create(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.RoleName);
                if (role == null)
                {
                    ModelState.AddModelError("", "Role not found");
                    return View(model);
                }

                var userRole = new UserRole
                {
                    UserId = model.UserId,
                    RoleId = role.Id
                };

                if (model.RoleName == "School Admin" || model.RoleName == "Activity Admin")
                {
                    var school = _context.Schools.FirstOrDefault(s => s.SchoolName == model.SchoolName);
                    if (school == null)
                    {
                        ModelState.AddModelError("", "School not found.");
                        return View(model);
                    }
                    userRole.SchoolId = school.Id;
                }

                if (model.RoleName == "Activity Admin")
                {
                    var activity = _context.Activities.FirstOrDefault(a => a.ActivityName == model.ActivityName && a.SchoolId == userRole.SchoolId);
                    if (activity == null)
                    {
                        ModelState.AddModelError("", "This school doesn't have the specified activity.");
                        return View(model);
                    }
                    userRole.ActivityId = activity.ActivityId;
                }

                try
                {
                    _context.UserRoles.Add(userRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                    {
                        ModelState.AddModelError("", "User Role already exists.");
                        return View(model);
                    }
                    throw;
                }

                TempData["SuccessMessage"] = "User role created successfully.";
                return RedirectToAction("UserRoles", new { Id = model.UserId });
            }

            ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name");
            ViewBag.SchoolName = new SelectList(_context.Schools, "SchoolName", "SchoolName");
            ViewBag.ActivityName = new SelectList(_context.Activities, "ActivityName", "ActivityName");
            return View(model);
        }
    }
}