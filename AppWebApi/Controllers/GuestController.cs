using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using System.Text.RegularExpressions;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GuestController : Controller
    {
        #region fields & constructors
        readonly IAdminService _service;
        //readonly ILoginService _loginService;
        readonly ILogger<GuestController> _logger = null;

        public GuestController(IAdminService service,
                ILogger<GuestController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        [HttpGet()]
        [ActionName(nameof(InfoDb))]
        [ProducesResponseType(200, Type = typeof(GstUsrInfoDbDto))]
        public async Task<IActionResult> InfoDb()
        {
            try
            {
                var info = await _service.GuestDbInfoAsync();

                _logger.LogInformation($"{nameof(InfoDb)}:\n{JsonConvert.SerializeObject(info)}");
                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(InfoDb)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //Fetch CuDto template for Update or Create
        [HttpGet()] 
        [ActionName(nameof(ReadItemCuDto))]
        [ProducesResponseType(200, Type = typeof(UserCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItemCuDto(string id = null)
        {
            try
            {
                var idArg = Guid.Parse(id);

                var response = await _service.ReadUserAsync(idArg, false);
                if (response is null) throw new ArgumentException($"User with id {id} does not exist.");

                return Ok(new UserCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        //Add user to database
        [HttpGet()]
        [ActionName(nameof(SignUp))]
        [ProducesResponseType(200, Type = typeof(UserCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> SignUp([FromBody] UserCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _service.CreateUserAsync(item);
                if (response is null) throw new ArgumentException($"User could not be created.");

                return Ok(new UserCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(SignUp)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        [HttpGet()]
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

