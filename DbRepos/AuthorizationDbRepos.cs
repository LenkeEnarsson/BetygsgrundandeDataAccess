using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AuthorizationDbRepos
{
    private ILogger<AuthorizationDbRepos> _logger;
    private readonly MainDbContext _dbContext;
    private readonly Encryptions _encryption;

    public AuthorizationDbRepos(ILogger<AuthorizationDbRepos> logger, Encryptions encryption, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
        _encryption = encryption;
    }

/*
    public async Task<ResponseIItem<LoginUserSessionDto>> LoginUserAsync(LoginCredentialsDto usrCreds)
    {
        throw new NotImplementedException();
    }
    public async Task SignUpUserAsync(UserDbM user)
    {
        throw new NotImplementedException();
    }
*/
    }

