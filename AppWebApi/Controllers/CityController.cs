using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Services;
using Microsoft.AspNetCore.Authorization;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CityController : Controller
    {
        #region fields & constructors
        readonly ICityService _service;
        readonly ILogger<CityController> _logger;

        public CityController(ICityService service, ILogger<CityController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        [HttpGet()]
        [ActionName(nameof(ReadCityList))]
        [ProducesResponseType(200, Type = typeof(CityCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCityList(bool seeded = true, bool flat = true, string filter = null, int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var response = await _service.ReadCityListAsync(seeded, flat, filter, pageNumber, pageSize);
                if (response is null) throw new ArgumentException($"No cities exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCityList)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadCity))]
        [ProducesResponseType(200, Type = typeof(CityCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCity(Guid id, bool flat = false)
        {
            try
            {
                var response = await _service.ReadCityAsync(id, flat);
                if (response is null) throw new ArgumentException($"No city with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCity)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadItemCuDto))]
        [ProducesResponseType(200, Type = typeof(CityCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemCuDto(string id = null)
        {
            try
            {
                if(id is null) return Ok(new CityCuDto());

                var idArg = Guid.Parse(id);
                var response = await _service.ReadCityAsync(idArg, false);
                if (response is null) throw new ArgumentException($"City with id {id} does not exist.");

                return Ok(new CityCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpDelete()]
        [ActionName(nameof(DeleteCity))]
        [ProducesResponseType(200, Type = typeof(CityCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            try
            {
                var response = await _service.DeleteCityAsync(id);
                if (response is null) throw new ArgumentException($"No city with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteCity)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPut()]
        [ActionName(nameof(UpdateCity))]
        [ProducesResponseType(200, Type = typeof(CityCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateCity(string id, [FromBody] CityCuDto item)
        {
            try
            {
                item.EnsureValidity();
                item.CityId = Guid.Parse(id);

                var response = await _service.UpdateCityAsync(item);
                if (response is null) throw new ArgumentException($"City with id {id} does not exist.");

                return Ok(new CityCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateCity)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [ActionName(nameof(CreateCity))]
        [ProducesResponseType(200, Type = typeof(CityCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateCity([FromBody] CityCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _service.CreateCityAsync(item);
                if (response is null) throw new ArgumentException($"City could not be created.");

                return Ok(new CityCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateCity)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}