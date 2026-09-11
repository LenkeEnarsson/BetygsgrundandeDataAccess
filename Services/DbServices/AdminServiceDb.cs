using Microsoft.Extensions.Logging;

using DbRepos;

namespace Services;
    
public class AdminServiceDb : IAdminService
{
    private readonly AdminDbRepos _repo = null;
    private readonly ILogger<AdminServiceDb> _logger = null;

    public Task SeedAsync() => _repo.SeedAsync();
    public Task RemoveSeedAsync(bool seeded) => _repo.RemoveSeedAsync(seeded);

    #region constructors
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
}

