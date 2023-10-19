using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper,ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll action method was invoked");
            var regionsDomain = await _regionRepository.GetAllAsync();
            _logger.LogInformation($"Finished GetAllRegions request with data:{JsonSerializer.Serialize(regionsDomain)}");
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            return Ok(regionsDto);
        }

        //Get Single Region (Get Region By Id)
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or convert dto to domain model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);
        }

        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionModel = _mapper.Map<Region>(updateRegionRequestDto);

            regionModel = await _regionRepository.UpdateAsync(id, regionModel);
            if (regionModel == null)
            {
                return NotFound();
            }

            var regionDto = _mapper.Map<RegionDto>(regionModel);
            return Ok(regionDto);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //check if region exists
            var regionDomainModel = await _regionRepository.DeleteByIdAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
