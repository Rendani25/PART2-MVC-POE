namespace UploadFileToDb.Models
{
    public class FileCreations
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Extention { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public IFormFile Upload { get; set; }

    }
}