using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class AttractionDbRepos
{
    #region fields and constructors
    private ILogger<AttractionDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionDbRepos(ILogger<AttractionDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat)
    {
        IAttraction item;
        if (flat)
        {
            var query = _dbContext.Attractions.AsNoTracking()
                .Where(i => i.AttractionId == id);

            item = await query.FirstOrDefaultAsync<IAttraction>();
        }
        else
        {
            var query = _dbContext.Attractions.AsNoTracking() //No tracking for reading
                .Include(i => i.CityDbM).ThenInclude(c => c.CountryDbM)
                .Include(i => i.ReviewsDbM).ThenInclude(r => r.UserDbM)
                .Include(i => i.CategoriesDbM)
                .Where(i => i.AttractionId == id);

            item = await query.FirstOrDefaultAsync<IAttraction>();
        }
        
        if (item == null) throw new ArgumentException($"Attraction {id} does not exist");
        return new ResponseItemDto<IAttraction>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

public async Task<ResponsePageDto<IAttraction>> ReadAttractionListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
         IQueryable<AttractionDbM> query;
        if (flat)
        {
            query = _dbContext.Attractions.AsNoTracking();
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(i => i.CityDbM).ThenInclude(c => c.CountryDbM)
                .Include(i => i.ReviewsDbM).ThenInclude(r => r.UserDbM)
                .Include(i => i.CategoriesDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<IAttraction>()
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

                .ToListAsync<IAttraction>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
            return new ResponsePageDto<IAttraction>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                                  (i.CategoriesDbM.Any(c => c.CategoryName.ToLower().Contains(filter))
                                || i.Title.ToLower().Contains(filter) 
                                || i.Description.ToLower().Contains(filter)
                                || i.CityDbM.CityName.ToLower().Contains(filter)
                                || i.CityDbM.CountryDbM.CountryName.ToLower().Contains(filter))).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                                  (i.CategoriesDbM.Any(c => c.CategoryName.ToLower().Contains(filter))
                                || i.Title.ToLower().Contains(filter) 
                                || i.Description.ToLower().Contains(filter)
                                || i.CityDbM.CityName.ToLower().Contains(filter)
                                || i.CityDbM.CountryDbM.CountryName.ToLower().Contains(filter)))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IAttraction>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
    }

public async Task<ResponsePageDto<IAttraction>> ReadAttractionListNoReviewsAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        IQueryable<AttractionDbM> query;
        if (flat)
        {
            query = _dbContext.Attractions.AsNoTracking();
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(i => i.CityDbM)
                .Include(i => i.CategoriesDbM)
                .Include(i => i.ReviewsDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<IAttraction>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                .Where(i => (i.Seeded == seeded) 
                    && i.ReviewsDbM.Count == 0)
                .CountAsync(),

                PageItems = await query
                .Where(i => (i.Seeded == seeded) 
                    && i.ReviewsDbM.Count == 0)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync<IAttraction>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
            return new ResponsePageDto<IAttraction>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                                i.Title.ToLower().Contains(filter)).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.Title.ToLower().Contains(filter))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<IAttraction>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
    }

    public async Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Attractions
            .Where(i => i.AttractionId == id);
        var item = await query1.FirstOrDefaultAsync<AttractionDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Attraction {id} does not exist.");

        //delete in the database model
        _dbContext.Attractions.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<IAttraction>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<IAttraction>> CreateAttractionAsync(AttractionCuDto itemCuDto)
    {
        if (itemCuDto.AttractionId != null)
            throw new ArgumentException($"{nameof(itemCuDto.AttractionId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Attraction
        var item = new AttractionDbM(itemCuDto);

        //Update navigation properties
        await navProp_AttractionCuDto_to_AttractionDbM(itemCuDto, item);

        //Note changes in DbContext and changetracker
        _dbContext.Attractions.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadAttractionAsync(item.AttractionId, false);
    }
    
    public async Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto itemDto)
    {
        if (itemDto.AttractionId is null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} is null.");

        //Find object in database with nav props
        var query = _dbContext.Attractions
            .Where(i => i.AttractionId == itemDto.AttractionId);
        var item = await query
                .Include(i => i.CityDbM)
                .Include(i => i.ReviewsDbM)
                .Include(i => i.CategoriesDbM)
                .Where(i => i.AttractionId == (Guid)itemDto.AttractionId)
            .FirstOrDefaultAsync<AttractionDbM>();

        if (item == null) throw new ArgumentException($"Item {itemDto.AttractionId} does not exist");

        //Update
        item.UpdateFromDTO(itemDto);
        await navProp_AttractionCuDto_to_AttractionDbM(itemDto, item);

        //Note change and save
        _dbContext.Attractions.Update(item);
        await _dbContext.SaveChangesAsync();

        //Return updated DbM
        return await ReadAttractionAsync(item.AttractionId, false);
    }

    private async Task navProp_AttractionCuDto_to_AttractionDbM(AttractionCuDto itemDtoSrc, AttractionDbM itemDst)
    {
        //Assign City
        itemDst.CityDbM = (itemDtoSrc.CityId is not null) ? await _dbContext.Cities.FirstOrDefaultAsync(
            a => (a.CityId == itemDtoSrc.CityId)) : null;

        //Assign list of Reviews
        List<ReviewDbM> reviews = null;
        if (itemDtoSrc.ReviewsId is not null)
        {
            reviews = new List<ReviewDbM>();
            foreach (var id in itemDtoSrc.ReviewsId)
            {
                var p = await _dbContext.Reviews.FirstOrDefaultAsync(i => i.ReviewId == id);
                if (p is null)
                    throw new ArgumentException($"Review id {id} does not exist.");

                reviews.Add(p);
            }
        }
        itemDst.ReviewsDbM = reviews;

        //Assign Categories
        List<CategoryDbM> categories = null;
        if (itemDtoSrc.CategoriesId is not null)
        {
            categories = new List<CategoryDbM>();
            foreach (var id in itemDtoSrc.CategoriesId)
            {
                var q = await _dbContext.Categories.FirstOrDefaultAsync(i => i.CategoryId == id);
                if (q == null)
                    throw new ArgumentException($"Category id {id} does not exist.");

                categories.Add(q);
            }
        }
        itemDst.CategoriesDbM = categories;
    }

}
