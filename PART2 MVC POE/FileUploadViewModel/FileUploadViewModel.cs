using UploadFileToDb.Models;

namespace UploadFileToDb.FileUploadViewModel
{
    public class FileUploadViewModel : FileCreation
    {
        internal string Description;

        public byte[] data { get; set; }

        public required List<FileCreations> SystemFiles { get; set; }


    }

    public class FileCreation
    {
    }
}
