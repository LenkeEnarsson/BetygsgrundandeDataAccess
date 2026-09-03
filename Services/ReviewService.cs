using Microsoft.Extensions.Logging;

using Models;
using DbRepos;

namespace Services;

public class ReviewServiceDb : IReviewService
{
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
}

