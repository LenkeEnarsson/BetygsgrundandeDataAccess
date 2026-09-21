using Microsoft.Extensions.Logging;

using Models;
using DbRepos;
using Models.DTO;
using models.CuDto;

namespace Services;

public class ReviewServiceDb : IReviewService
{
    #region fields & constructors
    private readonly ReviewDbRepos _repo = null;
    private readonly ILogger<ReviewServiceDb> _logger = null;


    public ReviewServiceDb(ReviewDbRepos repo)
    {
        _repo = repo;
    }
    public ReviewServiceDb(ReviewDbRepos repo, ILogger<ReviewServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public Task<ResponsePageDto<IReview>> ReadReviewListAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    => _repo.ReadReviewListAsync(seeded, flat, filter, pageNumber, pageSize);
    public Task<ResponseItemDto<IReview>> ReadReviewAsync(Guid id, bool flat)
    => _repo.ReadReviewAsync(id, flat);
    public Task<ResponseItemDto<IReview>> DeleteReviewAsync(Guid id)
    => _repo.DeleteReviewAsync(id);
    public Task<ResponseItemDto<IReview>> CreateReviewAsync(ReviewCuDto item)
    => _repo.CreateReviewAsync(item);
    public Task<ResponseItemDto<IReview>> UpdateReviewAsync(ReviewCuDto item)
    => _repo.UpdateReviewAsync(item);
}

