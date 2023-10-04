using System.ComponentModel.DataAnnotations.Schema;

namespace NewProj.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        // ohne map. IFormFile - type für file aus .net
        [NotMapped]
        public IFormFile File { get; set; }

        public string FileName { get; set; }

        public string? FileDescription { get; set; }

        public string FileExtention {  get; set; }

        public long FileSizeInBytes { get; set; }

        public string FilePath { get; set; }
    }
}
