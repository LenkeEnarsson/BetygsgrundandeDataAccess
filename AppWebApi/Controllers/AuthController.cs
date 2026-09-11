using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;
using Microsoft.Extensions.Options;
using Models.DTO;

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
        readonly IAuthService _authorization = null;

        //GET: api/auth/Login
        [HttpGet()]
        [ActionName(nameof(Login))]
        [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
        public IActionResult Login(UserLoginDto user)
        {
            try
            {
                //TODO: Implementera JWT 
                var result = _authorization.Login(user);

                _logger.LogInformation($"{nameof(Login)}:\n{JsonConvert.SerializeObject(result)}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Login)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
         }

        //GET: api/auth/signup
        [HttpGet()]
        [ActionName(nameof(SignUp))]
        [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
        public IActionResult SignUp(UserSignUpDto user)
        {
            try
            {
                var result = _authorization.Login(user);
                
                _logger.LogInformation($"{nameof(SignUp)}:\n{JsonConvert.SerializeObject(result)}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(SignUp)}: {ex.Message}");
                return BadRequest(ex.Message);
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
            _authorization = authorization;
        }
    }
}

