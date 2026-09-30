using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class CategoryDbRepos
{
    #region fields and constructors
    private ILogger<CategoryDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CategoryDbRepos(ILogger<CategoryDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponseItemDto<ICategory>> ReadCategoryAsync(Guid id, bool flat)
    {
        ICategory item;
        if (flat)
        {
            var query = _dbContext.Categories.AsNoTracking()
                .Where(i => i.CategoryId == id);

            item = await query.FirstOrDefaultAsync<ICategory>();
        }
        else
        {
            var query = _dbContext.Categories.AsNoTracking() //No tracking for reading
                .Include(i => i.AttractionsDbM)
                .Where(i => i.CategoryId == id);

            item = await query.FirstOrDefaultAsync<ICategory>();
        }
        
        if (item == null) throw new ArgumentException($"Category {id} does not exist");
        return new ResponseItemDto<ICategory>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

public async Task<ResponsePageDto<ICategory>> ReadCategoryListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        IQueryable<CategoryDbM> query;
        if (flat)
        {
            query = _dbContext.Categories.AsNoTracking();
        }
        else
        {
            query = _dbContext.Categories.AsNoTracking()
                .Include(i => i.AttractionsDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<ICategory>()
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

                .ToListAsync<ICategory>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
            return new ResponsePageDto<ICategory>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                                i.CategoryName.ToLower().Contains(filter)).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.CategoryName.ToLower().Contains(filter))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<ICategory>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
    }

    public async Task<ResponseItemDto<ICategory>> DeleteCategoryAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Categories
            .Where(i => i.CategoryId == id);
        var item = await query1.FirstOrDefaultAsync<CategoryDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Category {id} does not exist.");

        //delete in the database model
        _dbContext.Categories.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<ICategory>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICategory>> CreateCategoryAsync(CategoryCuDto itemCuDto)
    {
        if (itemCuDto.CategoryId != null)
            throw new ArgumentException($"{nameof(itemCuDto.CategoryId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Category
        var item = new CategoryDbM(itemCuDto);

        //Update navigation properties
        await navProp_CategoryCUdto_to_CategoryDbM(itemCuDto, item);

        //Note changes in DbContext and changetracker
        _dbContext.Categories.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadCategoryAsync(item.CategoryId, false);
    }

    public async Task<ResponseItemDto<ICategory>> UpdateCategoryAsync(CategoryCuDto itemDto)
    {
        if (itemDto.CategoryId != null)
            throw new ArgumentException($"{nameof(itemDto.CategoryId)} must be null when creating a new object");

        //Update individual properties Category
        var item = new CategoryDbM(itemDto);

        //Update navigation properties
        await navProp_CategoryCUdto_to_CategoryDbM(itemDto, item);

        //Note changes in DbContext
        _dbContext.Categories.Add(item);

        //write to database
        await _dbContext.SaveChangesAsync();

        //return the updated database item in non-flat mode
        return await ReadCategoryAsync(item.CategoryId, false);
    }

    private async Task navProp_CategoryCUdto_to_CategoryDbM(CategoryCuDto itemDtoSrc, CategoryDbM itemDst)
    {
        //Assign list of Attractions
        List<AttractionDbM> attractions = null;
        if (itemDtoSrc.AttractionIds is not null)
        {
            attractions = new List<AttractionDbM>();
            foreach (var id in itemDtoSrc.AttractionIds)
            {
                var p = await _dbContext.Attractions.FirstOrDefaultAsync(i => i.AttractionId == id);
                if (p is null)
                    throw new ArgumentException($"Attraction id {id} does not exist.");

                attractions.Add(p);
            }
        }
        itemDst.AttractionsDbM = attractions;
    }

}
