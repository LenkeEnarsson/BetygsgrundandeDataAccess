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
    public Task RemoveSeedAsync(bool seeded) => _repo.RemoveSeedAsync(seeded);
    
    //Guestuser Views
    public Task<GstUsrInfoDbDto> GuestDbInfoAsync() => _repo.GuestDbInfoAsync();
    
    //Handling of users:
    public Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id, bool flat) => _repo.ReadUserAsync(id, flat);
    public Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto item) => _repo.CreateUserAsync(item);
    public Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto item) => _repo.UpdateUserAsync(item);
    public Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id) => _repo.DeleteUserAsync(id);

}

