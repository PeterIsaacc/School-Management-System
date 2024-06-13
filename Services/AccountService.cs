// using Microsoft.AspNetCore.Identity;
// using SchoolManagementSystem.Models;
// using SchoolManagementSystem.Repositories;
// using SchoolManagementSystem.ViewModels;
// using System.Linq;
// using System.Threading.Tasks;

// namespace SchoolManagementSystem.Services
// {
//     public class AccountService : IAccountService
//     {
//         private readonly UserManager<User> _userManager;
//         private readonly SignInManager<User> _signInManager;
//         private readonly IRoleRepository _roleRepository;
//         private readonly IUserRoleRepository _userRoleRepository;
//         private readonly ISchoolRepository _schoolRepository;
//         private readonly IActivityRepository _activityRepository;

//         public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, ISchoolRepository schoolRepository, IActivityRepository activityRepository)
//         {
//             _userManager = userManager;
//             _signInManager = signInManager;
//             _roleRepository = roleRepository;
//             _userRoleRepository = userRoleRepository;
//             _schoolRepository = schoolRepository;
//             _activityRepository = activityRepository;
//         }

//         public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
//         {
//             var role = await _roleRepository.FindByNameAsync(model.Name);
//             if (role == null)
//             {
//                 return IdentityResult.Failed(new IdentityError { Description = $"Role '{model.Name}' not found." });
//             }

//             var user = new User
//             {
//                 UserName = model.Email,
//                 NormalizedUserName = model.Email.ToUpperInvariant(),
//                 Email = model.Email,
//                 NormalizedEmail = model.Email.ToUpperInvariant(),
//                 EmailConfirmed = true,
//                 Role = role
//             };

//             var result = await _userManager.CreateAsync(user, model.Password);
//             if (!result.Succeeded)
//             {
//                 return result;
//             }

//             await _userManager.AddToRoleAsync(user, role.Name);

//             var userRole = new UserRole
//             {
//                 User = user,
//                 Role = role
//             };

//             if (role.Name == "School Admin")
//             {
//                 userRole.SchoolId = _schoolRepository.Find(s => s.SchoolName == model.SchoolName)?.Id;
//             }
//             else if (role.Name == "Activity Admin")
//             {
//                 userRole.SchoolId = _schoolRepository.Find(s => s.SchoolName == model.SchoolName)?.Id;
//                 userRole.ActivityId = _activityRepository.Find(a => a.ActivityName == model.ActivityName)?.ActivityId;
//             }

//             await _userRoleRepository.AddAsync(userRole);
//             await _userRoleRepository.SaveAsync();

//             return IdentityResult.Success;
//         }

//         public async Task<SignInResult> LoginAsync(LoginViewModel model)
//         {
//             var user = await _userManager.FindByEmailAsync(model.Email);
//             if (user == null || !user.EmailConfirmed)
//             {
//                 return SignInResult.Failed;
//             }

//             return await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
//         }

//         public async Task LogoutAsync()
//         {
//             await _signInManager.SignOutAsync();
//         }
//     }
// }
