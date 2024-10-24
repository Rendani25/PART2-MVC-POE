namespace UploadFileToDb.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public required string Password { get; set; } // Consider hashing this in production
        public required string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
    }
   
}