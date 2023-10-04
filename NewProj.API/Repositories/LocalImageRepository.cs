using NewProj.API.Models.Domain;
using NZWalks.API.Data;

namespace NewProj.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            // gibt die Möglichkeit die Path von API erhalten.
            this.webHostEnvironment = webHostEnvironment;
            // gibt die actuelle Path bis API
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }



        public async Task<Image> Upload(Image image)
        {
            // Erstellen die Path für images. Path.Combine - verbinden. webHostEnvironment.ContentRootPath - path bis API,  "Images" - folder, und Name + Extention.
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtention}");

            // Erstellen FileStream um Daten zum local Server zu speichern. localFilePath - Path zum File, FileMode.Create - was soll zu tun, using - abschalten FileStream nach Beendung.
            using var stream = new FileStream(localFilePath, FileMode.Create);
            // Kopieren die daten aus erste stream image.File (Image aus Nutzer) zum zweiter stream(stream - local platz)
            await image.File.CopyToAsync(stream);

            // https://Localhost:1234/images/image.jpg
            // scheme    host   pathbase  / images / Filename. Fileextetion.

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtention}";

            image.FilePath = urlFilePath;

            // speichern zum folder Images
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
