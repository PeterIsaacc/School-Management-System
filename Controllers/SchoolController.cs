using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SchoolManagementSystem.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ApplicationDbContext _context;
    
        public SchoolController(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public async Task<IActionResult> Index()
        {
            var schools = await _context.Schools.ToListAsync();
            return View(schools);
        }
    
        public IActionResult Create()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolId,SchoolName")] School school)
        {
            if (ModelState.IsValid)
            {
                if (_context.Schools.Any(s => s.SchoolName == school.SchoolName))
                {
                    ModelState.AddModelError("SchoolName", "School name already exists");
                    return View(school);
                }
    
                _context.Add(school);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }
    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var school = await _context.Schools.Include(s => s.Activities).FirstOrDefaultAsync(s => s.Id == id);
            if (school == null)
            {
                return NotFound();
            }

            var activities = await _context.Activities.ToListAsync();

            var activitySelectItems = activities.Select(a => new SelectListItem
            {
                Value = a.ActivityId.ToString(),
                Text = a.ActivityName,
                Selected = school.Activities != null && school.Activities.Any(sa => sa.ActivityId == a.ActivityId)
            }).ToList();

            ViewBag.Activities = activitySelectItems;

            return View(school);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SchoolName")] School school, List<int> selectedActivities)
        {
            if (id != school.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Schools.Any(s => s.SchoolName == school.SchoolName && s.Id != school.Id))
                    {
                        ModelState.AddModelError("SchoolName", "School name already exists");
                        return View(school);
                    }

                    var existingSchool = await _context.Schools.Include(s => s.Activities).FirstOrDefaultAsync(s => s.Id == school.Id);

                    existingSchool.SchoolName = school.SchoolName;

                    existingSchool.Activities.Clear();

                    foreach (var activityId in selectedActivities)
                    {
                        existingSchool.Activities.Add(await _context.Activities.FindAsync(activityId));
                    }

                    _context.Update(existingSchool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolExists(school.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(school);
        }
    
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
    
            var school = await _context.Schools
                .FirstOrDefaultAsync(m => m.Id == id);
            if (school == null)
            {
                return NotFound();
            }
    
            return View(school);
        }
    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var school = await _context.Schools.FindAsync(id);
            _context.Schools.Remove(school);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    
        private bool SchoolExists(int id)
        {
            return _context.Schools.Any(e => e.Id == id);
        }
    }
}