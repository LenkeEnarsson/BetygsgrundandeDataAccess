using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class AttractionServiceDb : IAttractionService
{
    #region fields & constructors
    private readonly AttractionDbRepos _repo = null;
    private readonly ILogger<AttractionServiceDb> _logger = null;


    public AttractionServiceDb(AttractionDbRepos repo)
    {
        _repo = repo;
    }
    public AttractionServiceDb(AttractionDbRepos repo, ILogger<AttractionServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<IAttraction>> ReadAttractionListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadAttractionListAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponsePageDto<IAttraction>> ReadAttractionListNoReviewsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadAttractionListNoReviewsAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat)
    => _repo.ReadAttractionAsync(id, flat);
    public Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id)
    => _repo.DeleteAttractionAsync(id);
    public Task<ResponseItemDto<IAttraction>> CreateAttractionAsync(AttractionCuDto item)
    => _repo.CreateAttractionAsync(item);
    public Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto item)
    => _repo.UpdateAttractionAsync(item);
}

