using SchoolManagementSystem.Models;
using SchoolManagementSystem.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
