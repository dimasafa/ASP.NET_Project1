using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewProj.API.Models.Domain;
using NewProj.API.Models.DTO;
using NZWalks.API.Data;

namespace NewProj.API.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        // создаем конструктор для подключения к базе данных
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        // Получить все регионы
        // GET: https://localhost:1234/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            // создаем переменную и кладем туда таблицу регионов из бд, преобразуя в лист. Это модель предметной области. Никогда не возвращем обратно,
            // вместо этого мы преобразуем в DTO и возвращаем уже его.
            var regionsDomain = dbContext.Regions.ToList();

            // Map Damain Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    id = region.id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            return Ok(regionsDto);
        }


        // Получить регион по идентификтору
        // GET: https://localhost:1234/api/regions/{id}
        [HttpGet]
        // создаем роут, который будет проверять id с тем, что вводят в метод, и далее связываем с аргументом в методе через [FromRoute]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            // suchen in DB unsere id
            var region = dbContext.Regions.Find(id);

            // по другому строку выше можно записать так - var region = dbContext.Regions.FirstOrDefault(x => x.Id == id)

            //if nicht gefunden, dann return NotFound (404)
            if (region == null)
            {
                return NotFound();
            }
            // if gefunden, dann return ok(200)
            return Ok(region);
        }
    }
}
