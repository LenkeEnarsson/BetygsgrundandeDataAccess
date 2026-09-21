using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

using Models;
using Services;
using Microsoft.AspNetCore.Authorization;
using models.CuDto;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReviewController : Controller
    {
        #region fields & constructors
        readonly IReviewService _service;
        readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService service, ILogger<ReviewController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    
        [HttpGet()]
        [ActionName(nameof(ReadReviewList))]
        [ProducesResponseType(200, Type = typeof(ReviewCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadReviewList(bool seeded, bool flat, string filter, int pageNumber, int pageSize = 10)
        {
            try
            {
                var response = await _service.ReadReviewListAsync(seeded, flat, filter, pageNumber, pageSize);
                if (response is null) throw new ArgumentException($"No reviews exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadReviewList)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(ReadReview))]
        [ProducesResponseType(200, Type = typeof(ReviewCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadReview(Guid id, bool flat = true)
        {
            try
            {
                var response = await _service.ReadReviewAsync(id, flat);
                if (response is null) throw new ArgumentException($"No reviews exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadReview)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(DeleteReview))]
        [ProducesResponseType(200, Type = typeof(ReviewCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            try
            {
                var response = await _service.DeleteReviewAsync(id);
                if (response is null) throw new ArgumentException($"No review with id {id} exists in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteReview)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()] //Fetch CuDto template for Update or Create
        [ActionName(nameof(ReadItemCuDto))]
        [ProducesResponseType(200, Type = typeof(ReviewCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemCuDto(string id = null)
        {
            try
            {
                var idArg = Guid.Parse(id);

                var response = await _service.ReadReviewAsync(idArg, false);
                if (response is null) throw new ArgumentException($"Review with id {id} does not exist.");

                return Ok(new ReviewCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(UpdateReview))]
        [ProducesResponseType(200, Type = typeof(ReviewCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateReview(string id, [FromBody] ReviewCuDto item)
        {
            try
            {
                item.EnsureValidity();
                item.AttractionId = Guid.Parse(id);

                var response = await _service.UpdateReviewAsync(item);
                if (response is null) throw new ArgumentException($"Review with id {id} does not exist.");

                return Ok(new ReviewCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateReview)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
        [ActionName(nameof(CreateReview))]
        [ProducesResponseType(200, Type = typeof(ReviewCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _service.CreateReviewAsync(item);
                if (response is null) throw new ArgumentException($"Review could not be created.");

                return Ok(new ReviewCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateReview)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}
