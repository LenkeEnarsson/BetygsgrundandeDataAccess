using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Models;
using Services;
using Microsoft.AspNetCore.Authorization;
using models.Dto;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionController : Controller
    {
        readonly IAttractionService _service;
        readonly ILogger<AttractionController> _logger;

        public AttractionController(IAttractionService service, ILogger<AttractionController> logger)
        {
            _service = service;
            _logger = logger;
        }
    
        [HttpGet()]
        [ActionName(nameof(ReadAttractionList))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAttractionList(bool seeded, bool flat, string filter, int pageNumber, int pageSize = 10)
        {
            try
            {
                var response = await _service.ReadAttractionListAsync(seeded, flat, filter, pageNumber, pageSize);
                if (response is null) throw new ArgumentException($"No attractions exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttractionList)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadAttraction))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAttraction(Guid id, bool flat = true)
        {
            try
            {
                var response = await _service.ReadAttractionAsync(id, flat);
                if (response is null) throw new ArgumentException($"No attractions exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttraction)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(DeleteAttraction))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteAttraction(Guid id)
        {
            try
            {
                var response = await _service.DeleteAttractionAsync(id);
                if (response is null) throw new ArgumentException($"No attraction with id {id} the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteAttraction)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadItemCuDto))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemCuDto(string id = null)
        {
            try
            {
                var idArg = Guid.Parse(id);

                var response = await _service.ReadAttractionAsync(idArg, false);
                if (response is null) throw new ArgumentException($"Attraction with id {id} does not exist.");

                //Lägg in i interface också

                return Ok(new AttractionCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(UpdateAttraction))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateAttraction(string id, [FromBody] AttractionCuDto item)
        {
            try
            {
                item.EnsureValidity();
                item.AttractionId = Guid.Parse(id);

                var response = await _service.UpdateAttractionAsync(item);
                if (response is null) throw new ArgumentException($"Attraction with id {id} does not exist.");

                return Ok(new AttractionCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateAttraction)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(CreateAttraction))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateAttraction([FromBody] AttractionCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _service.CreateAttractionAsync(item);
                if (response is null) throw new ArgumentException($"Attraction could not be created.");

                return Ok(new AttractionCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateAttraction)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
