using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class CategoryServiceDb : ICategoryService
{
    #region fields & constructors
    private readonly CategoryDbRepos _repo = null;
    private readonly ILogger<CategoryServiceDb> _logger = null;


    public CategoryServiceDb(CategoryDbRepos repo)
    {
        _repo = repo;
    }
    public CategoryServiceDb(CategoryDbRepos repo, ILogger<CategoryServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<ICategory>> ReadCategoryListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadCategoryListAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<ICategory>> ReadCategoryAsync(Guid id, bool flat)
    => _repo.ReadCategoryAsync(id, flat);
    public Task<ResponseItemDto<ICategory>> DeleteCategoryAsync(Guid id)
    => _repo.DeleteCategoryAsync(id);
    public Task<ResponseItemDto<ICategory>> CreateCategoryAsync(CategoryCuDto item)
    => _repo.CreateCategoryAsync(item);
    public Task<ResponseItemDto<ICategory>> UpdateCategoryAsync(CategoryCuDto item)
    => _repo.UpdateCategoryAsync(item);
}

