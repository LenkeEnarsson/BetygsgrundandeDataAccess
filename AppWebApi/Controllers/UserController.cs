using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        #region fields & constructors
        readonly IUserService _service;
        //readonly ILoginService _loginService;
        readonly ILogger<UserController> _logger = null;

        public UserController(IUserService service,
                ILogger<UserController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        [HttpGet()]
        [ActionName(nameof(ReadUserList))]
        [ProducesResponseType(200, Type = typeof(UserCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadUserList(bool seeded = true, bool flat = false, string filter = null, int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var response = await _service.ReadUserListAsync(seeded, flat, filter, pageNumber, pageSize);
                if (response is null) throw new ArgumentException($"No users exist in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadUserList)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpDelete()]
        [ActionName(nameof(DeleteUser))]
        [ProducesResponseType(200, Type = typeof(AttractionCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var response = await _service.DeleteUserAsync(id);
                if (response is null) throw new ArgumentException($"No user with id {id} in the database.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteUser)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}

