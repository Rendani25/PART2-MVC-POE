using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PART2_MVC_POE.Models;
using UploadFileToDb.Data;
using UploadFileToDb.Models;

namespace UploadFileToDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        // Employee Management Actions
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees); // View expects a model of IEnumerable<Employee>
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // Create Employee Action
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Email,Password,PhoneNumber,HireDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to employee list
            }
            return View(employee); // Return to the view with validation errors
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,Password,PhoneNumber,HireDate")] Employee employee)
        {
            if (id != employee.EmployeeId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id) => _context.Employees.Any(e => e.EmployeeId == id);

        // File Upload Actions
        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var systemFiles = await _context.FileCreations.ToListAsync(); // Fetch system files asynchronously

            var vm = new FileUploadViewModel.FileUploadViewModel
            {
                SystemFiles = systemFiles,
                Description = string.Empty
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(FileUploadViewModel.FileUploadViewModel vm, IFormFile file)
        {
            // Check for file validation
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please upload a file.");
                vm.SystemFiles = await _context.FileCreations.ToListAsync();
                return View(vm);
            }

            var filename = $"{DateTime.Now:yyyyMMddHHmmss}_{file.FileName}";
            var path = _configuration.GetValue<string>("FileManagement:SystemFileUploads");
            var filepath = Path.Combine(path, filename);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            try
            {
                // Save the file to the server
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create a new FileCreations object
                var uploadfile = new FileCreations
                {
                    FileName = filename,
                    CreatedOn = DateTime.Now,
                    FileType = file.ContentType,
                    Description = vm.Description,
                    Extention = Path.GetExtension(file.FileName),
                };

                // Add the new file record to the database
                _context.FileCreations.Add(uploadfile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging mechanism)
                ModelState.AddModelError("", $"An error occurred while uploading the file: {ex.Message}");
                vm.SystemFiles = await _context.FileCreations.ToListAsync(); // Re-populate system files
                return View(vm);
            }

            return RedirectToAction(nameof(Index)); // Redirect to file upload index
        }

        // Academic Manager Actions

        // View all forms
        public async Task<IActionResult> ViewForms()
        {
            var forms = await _context.ApprovalForms.ToListAsync();
            return View(forms); // View expects a model of IEnumerable<ApprovalForm>
        }

        // Approve form
        public async Task<IActionResult> Approve(int id)
        {
            var form = await _context.ApprovalForms.FindAsync(id);
            if (form == null) return NotFound();

            form.Status = "Approved"; // Update the status
            _context.Update(form);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewForms));
        }

        // Reject form
        public async Task<IActionResult> Reject(int id)
        {
            var form = await _context.ApprovalForms.FindAsync(id);
            if (form == null) return NotFound();

            form.Status = "Rejected"; // Update the status
            _context.Update(form);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewForms));
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
