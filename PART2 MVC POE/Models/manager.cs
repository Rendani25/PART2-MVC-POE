namespace PART2_MVC_POE.Models
{
    public class ApprovalForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
    }
}
