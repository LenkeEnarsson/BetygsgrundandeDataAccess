using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class UserDbRepos
{
    #region fields and constructors
    private ILogger<UserDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public UserDbRepos(ILogger<UserDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id, bool flat)
    {
        IUser item;
        if (flat)
        {
            var query = _dbContext.Users.AsNoTracking()
                .Where(i => i.UserId == id);

            item = await query.FirstOrDefaultAsync<IUser>();
        }
        else
        {
            var query = _dbContext.Users.AsNoTracking() //No tracking for reading
                .Include(i => i.ReviewsDbM)
                .Where(i => i.UserId == id);

            item = await query.FirstOrDefaultAsync<IUser>();
        }
        
        if (item == null) throw new ArgumentException($"User {id} does not exist");
        return new ResponseItemDto<IUser>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

public async Task<ResponsePageDto<IUser>> ReadUserListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        IQueryable<UserDbM> query;
        if (flat)
        {
            query = _dbContext.Users.AsNoTracking();
        }
        else
        {
            query = _dbContext.Users.AsNoTracking()
                .Include(i => i.ReviewsDbM).ThenInclude(i => i.AttractionDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<IUser>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                    .Where(i => (i.Seeded == seeded))
                    .CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IUser>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
            return new ResponsePageDto<IUser>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                .Where(i => (i.Seeded == seeded) &&
                            (i.FirstName.ToLower().Contains(filter) 
                          || i.LastName.ToLower().Contains(filter)))
                .CountAsync(),

                PageItems = await query
                .Where(i => (i.Seeded == seeded) &&
                            (i.FirstName.ToLower().Contains(filter) 
                          || i.LastName.ToLower().Contains(filter)))

                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IUser>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
    }

    public async Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id)
    {
        //Find object
        var query1 = _dbContext.Users
            .Where(i => i.UserId == id);
        var item = await query1.FirstOrDefaultAsync<UserDbM>();

        //If not found
        if (item == null) throw new ArgumentException($"User {id} does not exist.");

        //delete in database 
        _dbContext.Users.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<IUser>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto itemCuDto)
    {
        if (itemCuDto.UserId != null)
            throw new ArgumentException($"{nameof(itemCuDto.UserId)} must be null when creating a new object");

        //Add DTO info to database objects
        var item = new UserDbM(itemCuDto);
        await navProp_UserCuDto_to_UserDbM(itemCuDto, item);

        _dbContext.Users.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadUserAsync(item.UserId, false);
    }

    public async Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto itemDto)
    {
        if (itemDto.UserId != null)
            throw new ArgumentException($"{nameof(itemDto.UserId)} must be null when creating a new object");

        //Convert properties
        var item = new UserDbM(itemDto);
        await navProp_UserCuDto_to_UserDbM(itemDto, item);

        //Update database
        _dbContext.Users.Add(item);
        await _dbContext.SaveChangesAsync();

        //return the updated database item in non-flat mode
        return await ReadUserAsync(item.UserId, false);
    }

    private async Task navProp_UserCuDto_to_UserDbM(UserCuDto itemDtoSrc, UserDbM itemDst)
    {
        //Assign list of Reviews
        List<ReviewDbM> reviews = null;
        if (itemDtoSrc.ReviewIds is not null)
        {
            reviews = new List<ReviewDbM>();
            foreach (var id in itemDtoSrc.ReviewIds)
            {
                var p = await _dbContext.Reviews.FirstOrDefaultAsync(i => i.ReviewId == id);
                if (p is null)
                    throw new ArgumentException($"Review id {id} does not exist.");

                reviews.Add(p);
            }
        }
        itemDst.ReviewsDbM = reviews;
    }

}
