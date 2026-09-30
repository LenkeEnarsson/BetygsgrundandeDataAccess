using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Models.DTO;
using Services;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GuestController : Controller
    {
        #region fields & constructors
        readonly IUserService _userservice;
        readonly IAdminService _adminservice;
        readonly ILogger<GuestController> _logger = null;

        public GuestController(IUserService userservice,
                IAdminService guestservice,
                ILogger<GuestController> logger)
        {
            _userservice = userservice;
            _adminservice = guestservice;
            _logger = logger;
        }
        #endregion

        [HttpGet()]
        [ActionName(nameof(InfoDb))]
        [ProducesResponseType(200, Type = typeof(CountRowsInTablesDbDto))]
        public async Task<IActionResult> InfoDb()
        {
            try
            {
                var info = await _adminservice.GuestDbInfoAsync();

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
        [ActionName(nameof(ReadUserCuDto))]
        [ProducesResponseType(200, Type = typeof(UserCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadUserCuDto(string id = null)
        {
            try
            {
                if(id is null) return Ok(new UserCuDto());
                
                var idArg = Guid.Parse(id);

                var response = await _userservice.ReadUserAsync(idArg, false);
                if (response is null) throw new ArgumentException($"User with id {id} does not exist.");

                return Ok(new UserCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadUserCuDto)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }

        //Add user to database
        [HttpPost()]
        [ActionName(nameof(Register))]
        [ProducesResponseType(200, Type = typeof(UserCuDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] UserCuDto item)
        {
            try
            {
                item.EnsureValidity();

                var response = await _userservice.CreateUserAsync(item);
                if (response is null) throw new ArgumentException($"User could not be created.");

                return Ok(new UserCuDto(response.Item));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Register)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
        }
    }
}

