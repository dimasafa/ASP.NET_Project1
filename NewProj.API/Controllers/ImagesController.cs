using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewProj.API.Models.Domain;
using NewProj.API.Models.DTO;
using NewProj.API.Repositories;

namespace NewProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: api/Images/Upload
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            // Validieren request. If falsche Validation, dann in ModelState werden die Errors - und führt zum Bad Request. Else, alles im Ordnung und wir können addieren zum Db.
            ValidateFileUpload(request);

            if (ModelState.IsValid) 
            {
                // convert DTO to Domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtention = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };
                // User Repositry zu Upload File

                await imageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);

        }

        // Validate eingetragene Daten
        private void ValidateFileUpload(ImageUploadRequestDto request) 
        {
            var allowedExtention = new string[] { ".jpg", ".jpeg", ".png" };

            if (allowedExtention.Contains(Path.GetExtension(request.File.FileName)) == false)
            {
                ModelState.AddModelError("file", "Falsche File Extention");
            }

            // Überprüfen ob file größer als 10mb.
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File mehr als 10mb nicht erlauben");
            }
        }
    }
}
