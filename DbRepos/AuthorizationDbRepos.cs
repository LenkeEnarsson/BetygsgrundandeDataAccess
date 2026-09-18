using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;

namespace DbRepos;

public class AuthorizationDbRepos
{
    private ILogger<AuthorizationDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AuthorizationDbRepos(ILogger<AuthorizationDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public async Task Login(UserDbM user)
    {
        throw new NotImplementedException();
    }
    public async Task SignUp(UserDbM user)
    {
        throw new NotImplementedException();
    }
    }

