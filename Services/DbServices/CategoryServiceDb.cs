using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class CategoryServiceDb : IAttractionPropertyService
{
    #region fields & constructors
    private readonly AttractionPropertyDbRepos _repo = null;
    private readonly ILogger<CategoryServiceDb> _logger = null;


    public CategoryServiceDb(AttractionPropertyDbRepos repo)
    {
        _repo = repo;
    }
    public CategoryServiceDb(AttractionPropertyDbRepos repo, ILogger<CategoryServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<ICategory>> ReadCategoryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadCategoryListAsync(seeded, flat, filter, pageNumber, pageSize);

    public Task<ResponsePageDto<ICity>> ReadCityListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadCityListAsync(seeded, flat, filter, pageNumber, pageSize);
    
    public Task<ResponsePageDto<ICountry>> ReadCountryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadCountryListAsync(seeded, flat, filter, pageNumber, pageSize);
}

