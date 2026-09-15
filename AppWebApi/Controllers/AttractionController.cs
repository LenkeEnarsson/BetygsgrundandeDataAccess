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
    }

        [HttpGet()]
        [ActionName(nameof(ReadItemDto))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            try
            {
                var idArg = Guid.Parse(id);

                var response = await _service.ReadAttractionAsync(idArg, false);
                if (response is null) throw new ArgumentException($"Item with id {id} does not exist.");

                //Lägg in i interface också

                return Ok(new AttractionCuDto(response.item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
}

