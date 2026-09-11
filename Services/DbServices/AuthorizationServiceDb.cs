using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;

namespace Services;

public class AuthorizationServiceDb : IAuthService
{
    private readonly AuthorizationDbRepos _repo = null;
    private readonly ILogger<AuthorizationServiceDb> _logger = null;

    public Task Login(UserLoginDto user)
    {
        throw new NotImplementedException();
    }
    public Task SignUp(UserSignUpDto user)
    {
        throw new NotImplementedException();
    }
    
    public AuthorizationServiceDb(AuthorizationDbRepos repo)
    {
        _repo = repo;
    }
    public AuthorizationServiceDb(AuthorizationDbRepos repo, ILogger<AuthorizationServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
}

