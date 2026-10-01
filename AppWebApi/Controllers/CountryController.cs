using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Services;
using Microsoft.AspNetCore.Authorization;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CountryController : Controller
    {
        #region fields & constructors
        readonly ICountryService _service;
        readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService service, ILogger<CountryController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        [HttpGet()]
        [ActionName(nameof(ReadCountryList))]
        [ProducesResponseType(200, Type = typeof(CountryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCountryList(bool seeded = true, bool flat = true, string filter = null, int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var response = await _service.ReadCountryListAsync(seeded, flat, filter, pageNumber, pageSize);
                if (response is null) throw new ArgumentException($"No countries exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCountryList)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadCountry))]
        [ProducesResponseType(200, Type = typeof(CountryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCountry(Guid id, bool flat = false)
        {
            try
            {
                var response = await _service.ReadCountryAsync(id, flat);
                if (response is null) throw new ArgumentException($"No country with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCountry)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadItemCuDto))]
        [ProducesResponseType(200, Type = typeof(CountryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemCuDto(string id = null)
        {
            try
            {
                if(id is null) return Ok(new CountryCuDto());

                var idArg = Guid.Parse(id);
                var response = await _service.ReadCountryAsync(idArg, false);
                if (response is null) throw new ArgumentException($"Country with id {id} does not exist.");

                return Ok(new CountryCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpDelete()]
        [ActionName(nameof(DeleteCountry))]
        [ProducesResponseType(200, Type = typeof(CountryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            try
            {
                var response = await _service.DeleteCountryAsync(id);
                if (response is null) throw new ArgumentException($"No country with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteCountry)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPut()]
        [ActionName(nameof(UpdateCountry))]
        [ProducesResponseType(200, Type = typeof(CountryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateCountry(string id, [FromBody] CountryCuDto item)
        {
            try
            {
                item.EnsureValidity();
                item.CountryId = Guid.Parse(id);

                var response = await _service.UpdateCountryAsync(item);
                if (response is null) throw new ArgumentException($"Country with id {id} does not exist.");

                return Ok(new CountryCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateCountry)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [ActionName(nameof(CreateCountry))]
        [ProducesResponseType(200, Type = typeof(CountryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateCountry([FromBody] CountryCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _service.CreateCountryAsync(item);
                if (response is null) throw new ArgumentException($"Country could not be created.");

                return Ok(new CountryCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateCountry)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}