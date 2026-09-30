using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Services;
using Microsoft.AspNetCore.Authorization;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionPropertyController : Controller
    {
        #region fields & constructors
        readonly IAttractionPropertyService _service;
        readonly ILogger<AttractionPropertyController> _logger;

        public AttractionPropertyController(IAttractionPropertyService service, ILogger<AttractionPropertyController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        [HttpGet()]
        [ActionName(nameof(ReadCategoryList))]
        [ProducesResponseType(200, Type = typeof(CategoryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCategoryList(bool seeded = true, bool flat = true, string filter = null, int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var response = await _service.ReadCategoryListAsync(seeded, flat, filter, pageNumber, pageSize);
                if (response is null) throw new ArgumentException($"No categories exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCategoryList)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

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
    }
}
