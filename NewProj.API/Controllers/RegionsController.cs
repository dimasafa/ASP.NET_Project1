using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewProj.API.Models.Domain;
using NewProj.API.Models.DTO;
using NewProj.API.Repositories;
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
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        // Получить все регионы
        // GET: https://localhost:1234/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // создаем переменную и кладем туда таблицу регионов из бд, преобразуя в лист. Это модель предметной области. Никогда не возвращем обратно,
            // вместо этого мы преобразуем в DTO и возвращаем уже его.
            var regionsDomain = await regionRepository.GetAllAsync();

            // Alte Code ohne Automapper (Selbst von Domain to DTO unstellen)
            // Map Damain Models to DTOs
            //var regionsDto = new List<RegionDto>();
            //foreach (var region in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        id = region.id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}


            // return DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        // Получить регион по идентификтору
        // GET: https://localhost:1234/api/regions/{id}
        [HttpGet]
        // создаем роут, который будет проверять id с тем, что вводят в метод, и далее связываем с аргументом в методе через [FromRoute]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            // suchen in DB unsere id
            var regionsDomain = await regionRepository.GetByIdAsync(id);
            // по другому строку выше можно записать так - var region = dbContext.Regions.FirstOrDefault(x => x.Id == id)

            //if nicht gefunden, dann return NotFound (404)
            if (regionsDomain == null)
            {
                return NotFound();
            }
            // if gefunden, dann return ok(200)


            //umstellen to DTO. Alte Code ohnr Automapper.
            //var regionsDto = new RegionDto
            //{
            //    id = regionsDomain.id,
            //    Code = regionsDomain.Code,
            //    Name = regionsDomain.Name,
            //    RegionImageUrl = regionsDomain.RegionImageUrl
            //};


            return Ok(mapper.Map<RegionDto>(regionsDomain));
        }

        // POST um die neue Region zu erstellen
        // POST: https://localhost:1234/api/regions

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Damain Model
            //var regionDomainModel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain Model mit DTO
            //var regionDto = new RegionDto
            //{
            //    id = regionDomainModel.id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.id }, regionDto);
        }

        // Update Region
        // PUT: https://localhost:1234/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};

            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }


            // Convert Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    id = regionDomainModel.id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        // Delete Region
        // DELETE: https://localhost:1234/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }


            // Convert Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    id = regionDomainModel.id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);

        }

    }
}
