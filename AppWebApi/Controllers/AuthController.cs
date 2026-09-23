    /*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;
using Microsoft.Extensions.Options;
using Models.DTO;
using models.CuDto;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]   
    public class AuthController : Controller
    {
        readonly ILogger<AuthController> _logger;
        readonly AesEncryptionOptions _aesOptions;
        readonly JwtOptions _jwtOptions;
        readonly Encryptions _encryptions = null;
        readonly IAuthService _service = null;

        //GET: api/auth/Login
        [HttpGet()]
        [ActionName(nameof(Login))]
        [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
        public IActionResult Login(UserLoginDto user)
        {
            try
            {
                
                var result = _service.Login(user);

                _logger.LogInformation($"{nameof(Login)}:\n{JsonConvert.SerializeObject(result)}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Login)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
         }


        //GET: api/auth/signup
        [HttpGet()]
        [ActionName(nameof(SignUp))]
        [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
        public IActionResult SignUp(UserCuDto user)
        {
            try
            {
                var result = _service.SignUp(user);
                
                _logger.LogInformation($"{nameof(SignUp)}:\n{JsonConvert.SerializeObject(result)}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(SignUp)}: {ex.Message} - {ex.InnerException?.Message}");
                return BadRequest($"{ex.Message} - {ex.InnerException?.Message}");
            }
         }

        public AuthController(ILogger<AuthController> logger,
                    IOptions<AesEncryptionOptions> aesOptions,
                    IOptions<JwtOptions> jwtOptions,
                    Encryptions encryptions,
                    IAuthService authorization)
        {
            _logger = logger;

            _aesOptions = aesOptions.Value;
            _jwtOptions = jwtOptions.Value;

            _encryptions = encryptions;
            _service = authorization;
        }
    }
}

    */