using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    public class UniversitiesController : Controller
    {
        private readonly UniversityContext _context;

        public UniversitiesController(UniversityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var viewModel = new UniversityData();
            viewModel.Universities = await _context.Universities
                   .Include(i => i.Departments)
                  .ToListAsync();
            return View(viewModel);
        }

        // GET: Students/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await _context.Students
        //         .Include(s => s.Enrollments)
        //             .ThenInclude(e => e.Course)
        //         .AsNoTracking()
        //         .FirstOrDefaultAsync(m => m.ID == id);

        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(student);
        //}

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Adress")] University university)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(university);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(university);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Universities.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var universityToUpdate = await _context.Universities.FirstOrDefaultAsync(s => s.UniversityID == id);
            if (await TryUpdateModelAsync<University>(
                universityToUpdate,
                "",
                s => s.Name, s => s.Adress))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(universityToUpdate);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var university = await _context.Universities
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UniversityID == id);
            if (university == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(university);
        }
        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(University university)
        {
            try
            {
                if (await _context.Universities.AnyAsync(m => m.UniversityID == university.UniversityID))
                {
                    var departments = await _context.Departments
                    .Where(d => d.UniversityID == university.UniversityID)
                    .ToListAsync();
                    departments.ForEach(d => d.UniversityID = null);
                    _context.Universities.Remove(university);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = university.UniversityID });
            }
        }

        private bool UniversityExists(int id)
        {
            return _context.Universities.Any(e => e.UniversityID == id);
        }
    }
}
