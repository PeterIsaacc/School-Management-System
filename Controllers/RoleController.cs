using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using SchoolManagementSystem.ViewModels;

[Authorize(Roles = "Super Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<Role> _roleManager;

    public RoleController(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    // GET: Roles
    public async Task<IActionResult> Index()
    {
        return View(await _roleManager.Roles.ToListAsync());
    }

    // GET: Roles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Roles/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RoleViewModel roleViewModel)
    {
        if (ModelState.IsValid)
        {
            var role = new Role { Name = roleViewModel.RoleName };
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role created successfully";
                return RedirectToAction("Index");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(roleViewModel);
    }

    // GET: Roles/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    // POST: Roles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Role role)
    {
        var existingRole = await _roleManager.FindByIdAsync(id);
        if (existingRole != null)
        {
            existingRole.Name = role.Name;
            IdentityResult result = await _roleManager.UpdateAsync(existingRole);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(role);
    }

    // GET: Roles/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    // POST: Roles/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View("Delete", role);
    }
}