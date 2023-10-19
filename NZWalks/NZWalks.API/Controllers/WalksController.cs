using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        //CREATE WALK
        //POST:/api/walks

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to domain model 
            if (ModelState.IsValid)
            {
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);
                walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);

                var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy=null,
            [FromQuery]bool? isAscending=null,[FromQuery] int pageNumber=1,[FromQuery] int pageSize=1000)
        {
            var walks = await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending??true,pageNumber,pageSize);

            throw new Exception("This is a new exception");
            _mapper.Map<List<WalkDto>>(walks); 
            return Ok(walks);
        }

        [HttpGet]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid Id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(Id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<WalkDto>(walkDomainModel));
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid Id,[FromBody]UpdateWalkRequestDto updateWalkRequestDto)
        {
            if (ModelState.IsValid)
            {
                var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
                walkDomainModel = await _walkRepository.UpdateAsync(Id, walkDomainModel);
                if (walkDomainModel == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<WalkDto>(walkDomainModel));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var walkDomainModel = await _walkRepository.DeleteAsync(Id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }
    }
}
