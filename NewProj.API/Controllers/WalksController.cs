using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewProj.API.CustomActionFilters;
using NewProj.API.Models.Domain;
using NewProj.API.Models.DTO;
using NewProj.API.Repositories;

namespace NewProj.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) 
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // Create Walk
        //POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto) 
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

            // Map Domain model to DTO

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }


        // Get Walks
        //GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&IsAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        // filterOn - column für filter; filterQuary - body for filter
        // sortBy - column für sorting, bool - von oben nach unten oder dagegen.
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuary, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuary, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Map Domain to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        //Get Walk by Id
        //GET: /api/walks/id
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.GetByIdAsync(id); 

            if (walksDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }

        // Update Walks
        // UPDATE: /api/walks/id
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walksDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            walksDomainModel = await walkRepository.UpdateByIdAsync(id, walksDomainModel);

            if (walksDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }

        //Delete Walks
        //DELETE: /api/walks/id
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.DeleteByIdAsync(id);

            if (walksDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walksDomainModel));
        }
    }
}
