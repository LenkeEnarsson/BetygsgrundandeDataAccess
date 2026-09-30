using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;
using Models;
using models.CuDto;

namespace Services;
    
public class AdminServiceDb : IAdminService
{
    #region field and constructors
    private readonly AdminDbRepos _repo = null;
    private readonly ILogger<AdminServiceDb> _logger = null;

    public AdminServiceDb(AdminDbRepos repo)
    {
        _repo = repo;
    }
    public AdminServiceDb(AdminDbRepos repo, ILogger<AdminServiceDb> logger):this(repo)
    {
        _logger = logger;
        _repo = repo;
    }
    #endregion

    public Task SeedAsync() => _repo.SeedAsync();
    public Task<CountRowsInTablesDbDto> RemoveSeedAsync(bool seeded) => _repo.RemoveSeedAsync(seeded);
    
    //Guest Views
    public Task<CountRowsInTablesDbDto> GuestDbInfoAsync() => _repo.DbCountRowsAsync();

}

