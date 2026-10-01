using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Services;
using Microsoft.AspNetCore.Authorization;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {
        #region fields & constructors
        readonly ICategoryService _service;
        readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService service, ILogger<CategoryController> logger)
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
        [ActionName(nameof(ReadCategory))]
        [ProducesResponseType(200, Type = typeof(CategoryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCategory(Guid id, bool flat = false)
        {
            try
            {
                var response = await _service.ReadCategoryAsync(id, flat);
                if (response is null) throw new ArgumentException($"No category with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCategory)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadItemCuDto))]
        [ProducesResponseType(200, Type = typeof(CategoryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemCuDto(string id = null)
        {
            try
            {
                if(id is null) return Ok(new CategoryCuDto());

                var idArg = Guid.Parse(id);
                var response = await _service.ReadCategoryAsync(idArg, false);
                if (response is null) throw new ArgumentException($"Category with id {id} does not exist.");

                return Ok(new CategoryCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpDelete()]
        [ActionName(nameof(DeleteCategory))]
        [ProducesResponseType(200, Type = typeof(CategoryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var response = await _service.DeleteCategoryAsync(id);
                if (response is null) throw new ArgumentException($"No category with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteCategory)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPut()]
        [ActionName(nameof(UpdateCategory))]
        [ProducesResponseType(200, Type = typeof(CategoryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] CategoryCuDto item)
        {
            try
            {
                item.EnsureValidity();
                item.CategoryId = Guid.Parse(id);

                var response = await _service.UpdateCategoryAsync(item);
                if (response is null) throw new ArgumentException($"Category with id {id} does not exist.");

                return Ok(new CategoryCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateCategory)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpPost()]
        [ActionName(nameof(CreateCategory))]
        [ProducesResponseType(200, Type = typeof(CategoryCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _service.CreateCategoryAsync(item);
                if (response is null) throw new ArgumentException($"Category could not be created.");

                return Ok(new CategoryCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateCategory)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
