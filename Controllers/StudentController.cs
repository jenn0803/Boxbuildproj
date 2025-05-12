//using Microsoft.AspNetCore.Mvc;
//using BoxBuildproj.Models;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using BoxBuildproj.Data;

//namespace BoxBuildproj.Controllers
//{
//    public class StudentController : Controller
//    {
//        private readonly BoxBuildprojContext _context;

//        public StudentController(BoxBuildprojContext context)
//        {
//            _context = context;
//        }

//        // GET: Student (List all students)
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.Students.ToListAsync());
//        }

//        // GET: Student/Create (Show form)
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Student/Create (Insert into DB)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Student student)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Students.Add(student);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(student);
//        }

//        // GET: Student/Edit/1 (Show edit form)
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null) return NotFound();

//            var student = await _context.Students.FindAsync(id);
//            if (student == null) return NotFound();

//            return View(student);
//        }

//        // POST: Student/Edit/1 (Update record)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, Student student)
//        {
//            if (id != student.StudentId) return NotFound();

//            if (ModelState.IsValid)
//            {
//                _context.Update(student);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(student);
//        }

//        // GET: Student/Delete/1 (Confirm delete)
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null) return NotFound();

//            var student = await _context.Students.FindAsync(id);
//            if (student == null) return NotFound();

//            return View(student);
//        }

//        // POST: Student/Delete/1 (Delete from DB)
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var student = await _context.Students.FindAsync(id);
//            if (student != null)
//            {
//                _context.Students.Remove(student);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }
//    }
//}
