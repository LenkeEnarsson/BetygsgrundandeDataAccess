using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class UserServiceDb : IUserService
{
    #region fields & constructors
    private readonly UserDbRepos _repo = null;
    private readonly ILogger<UserServiceDb> _logger = null;


    public UserServiceDb(UserDbRepos repo)
    {
        _repo = repo;
    }
    public UserServiceDb(UserDbRepos repo, ILogger<UserServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<IUser>> ReadUserListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadUserListAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id, bool flat)
    => _repo.ReadUserAsync(id, flat);
    public Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id)
    => _repo.DeleteUserAsync(id);
    public Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto item)
    => _repo.CreateUserAsync(item);
    public Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto item)
    => _repo.UpdateUserAsync(item);
}

