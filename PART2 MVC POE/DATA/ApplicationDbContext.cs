using Microsoft.EntityFrameworkCore;
using PART2_MVC_POE.Models;
using UploadFileToDb.Models;

namespace UploadFileToDb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        // Correct definition of FileCreations
        public DbSet<FileCreations> FileCreations { get; set; } // Public auto-property
        public DbSet<ApprovalForm> ApprovalForms { get; set; }
    }
}
