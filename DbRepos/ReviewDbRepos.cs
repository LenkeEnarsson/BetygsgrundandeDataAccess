using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class ReviewDbRepos
{
    #region fields and constructors
    private ILogger<ReviewDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public ReviewDbRepos(ILogger<ReviewDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IReview>> ReadReviewAsync(Guid id, bool flat)
    {
        IReview item;
        if (flat)
        {
            var query = _dbContext.Reviews.AsNoTracking()
                .Where(i => i.ReviewId == id);

            item = await query.FirstOrDefaultAsync<IReview>();
        }
        else
        {
            var query = _dbContext.Reviews.AsNoTracking() //No tracking for reading
                .Include(i => i.AttractionDbM).ThenInclude(a => a.CityDbM)
                .Include(i => i.UserDbM)
                .Where(i => i.ReviewId == id);

            item = await query.FirstOrDefaultAsync<IReview>();
        }
        
        if (item == null) throw new ArgumentException($"Review {id} does not exist");
        return new ResponseItemDto<IReview>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

public async Task<ResponsePageDto<IReview>> ReadReviewListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        IQueryable<ReviewDbM> query;
        if (flat)
        {
            query = _dbContext.Reviews.AsNoTracking();
        }
        else
        {
            query = _dbContext.Reviews.AsNoTracking()
                .Include(i => i.AttractionDbM)
                .Include(i => i.UserDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<IReview>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                //Adding filter functionality
                .Where(i => (i.Seeded == seeded)).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IReview>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
        {
            bool isScore = byte.TryParse(filter, out byte score);
            return new ResponsePageDto<IReview>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif


                DbItemsCount = await query
                .Where(i => (i.Seeded == seeded) &&
                    i.Score == (isScore ? score : (byte?)null))
                .CountAsync(),

                PageItems = await query
                .Where(i => (i.Seeded == seeded) &&
                    i.Score == (isScore ? score : (byte?)null))
                //Paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync<IReview>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        }
    }

    public async Task<ResponseItemDto<IReview>> DeleteReviewAsync(Guid id)
    {
        //Find id
        var query1 = _dbContext.Reviews
            .Where(i => i.ReviewId == id);
        var item = await query1.FirstOrDefaultAsync<ReviewDbM>();

        //If id not found
        if (item == null) throw new ArgumentException($"Review {id} does not exist.");

        //delete in database
        _dbContext.Reviews.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<IReview>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<IReview>> CreateReviewAsync(ReviewCuDto itemCuDto)
    {
        if (itemCuDto.ReviewId != null)
            throw new ArgumentException($"{nameof(itemCuDto.ReviewId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Review
        var item = new ReviewDbM(itemCuDto);

        //Update navigation properties
        await navProp_ReviewCuDto_to_ReviewDbM(itemCuDto, item);

        //Note changes in DbContext and changetracker
        _dbContext.Reviews.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadReviewAsync(item.ReviewId, false);
    }

    public async Task<ResponseItemDto<IReview>> UpdateReviewAsync(ReviewCuDto itemDto)
    {
        if (itemDto.ReviewId != null)
            throw new ArgumentException($"{nameof(itemDto.ReviewId)} must be null when creating a new object");

        //Update individual properties Review
        var item = new ReviewDbM(itemDto);

        //Update navigation properties
        await navProp_ReviewCuDto_to_ReviewDbM(itemDto, item);

        //Note changes in DbContext
        _dbContext.Reviews.Add(item);

        //write to database
        await _dbContext.SaveChangesAsync();

        //return the updated database item in non-flat mode
        return await ReadReviewAsync(item.ReviewId, false);
    }

    private async Task navProp_ReviewCuDto_to_ReviewDbM(ReviewCuDto itemDtoSrc, ReviewDbM itemDst)
    {
        //Assign Author
        itemDst.UserDbM = (itemDtoSrc.AuthorId is not null) ? await _dbContext.Users.FirstOrDefaultAsync(
            a => (a.UserId == itemDtoSrc.AuthorId)) : null;

        //Assign Attraction
        itemDst.AttractionDbM = (itemDtoSrc.AttractionId is not null) ? await _dbContext.Attractions.FirstOrDefaultAsync(
            a => (a.AttractionId == itemDtoSrc.AttractionId)) : null;
    }

}
