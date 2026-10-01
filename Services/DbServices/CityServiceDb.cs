using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class CityServiceDb : ICityService
{
    #region fields & constructors
    private readonly CityDbRepos _repo = null;
    private readonly ILogger<CityServiceDb> _logger = null;

    public CityServiceDb(CityDbRepos repo)
    {
        _repo = repo;
    }
    public CityServiceDb(CityDbRepos repo, ILogger<CityServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<ICity>> ReadCityListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadCityListAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<ICity>> ReadCityAsync(Guid id, bool flat)
    => _repo.ReadCityAsync(id, flat);
    public Task<ResponseItemDto<ICity>> DeleteCityAsync(Guid id)
    => _repo.DeleteCityAsync(id);
    public Task<ResponseItemDto<ICity>> CreateCityAsync(CityCuDto item)
    => _repo.CreateCityAsync(item);
    public Task<ResponseItemDto<ICity>> UpdateCityAsync(CityCuDto item)
    => _repo.UpdateCityAsync(item);
}