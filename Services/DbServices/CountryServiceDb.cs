using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class CountryServiceDb : ICountryService
{
    #region fields & constructors
    private readonly CountryDbRepos _repo = null;
    private readonly ILogger<CountryServiceDb> _logger = null;


    public CountryServiceDb(CountryDbRepos repo)
    {
        _repo = repo;
    }
    public CountryServiceDb(CountryDbRepos repo, ILogger<CountryServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<ICountry>> ReadCountryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadCountryListAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<ICountry>> ReadCountryAsync(Guid id, bool flat)
    => _repo.ReadCountryAsync(id, flat);
    public Task<ResponseItemDto<ICountry>> DeleteCountryAsync(Guid id)
    => _repo.DeleteCountryAsync(id);
    public Task<ResponseItemDto<ICountry>> CreateCountryAsync(CountryCuDto item)
    => _repo.CreateCountryAsync(item);
    public Task<ResponseItemDto<ICountry>> UpdateCountryAsync(CountryCuDto item)
    => _repo.UpdateCountryAsync(item);
}

