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

    public async Task<ResponsePageDto<ICategory>> ReadCategoryListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        var query = _dbContext.Categories.AsNoTracking();
        
        if (!flat)
            query = query
                .Include(i => i.AttractionsDbM);

        if (!string.IsNullOrEmpty(filter)) 
            query = query
            .Where(i => i.CategoryName.ToLower().Contains(filter));

        query = query.Where(i => i.Seeded == seeded);

        return new ResponsePageDto<ICategory>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),

            PageItems = await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync<ICategory>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ResponseItemDto<ICategory>> ReadCategoryAsync(Guid id, bool flat)
    {
        var query = _dbContext.Categories.AsNoTracking()
            .Where(i => i.CategoryId == id);

        if (!flat)
            query = query
                .Include(i => i.AttractionsDbM)
                .Where(i => i.CategoryId == id);

        var item = await query.FirstOrDefaultAsync<ICategory>();
        
        if (item is null) throw new ArgumentException($"Category {id} does not exist.");

        return new ResponseItemDto<ICategory>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICategory>> DeleteCategoryAsync(Guid id)
    {
        var item = await _dbContext.Categories.FirstOrDefaultAsync(i => i.CategoryId == id);
        if (item is null) throw new ArgumentException($"Category {id} does not exist.");
        
        _dbContext.Categories.Remove(item);
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<ICategory>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICategory>> CreateCategoryAsync(CategoryCuDto itemDto)
    {
        if (itemDto.CategoryId is not null)
            throw new ArgumentException($"{nameof(itemDto.CategoryId)} must be null when creating a new object.");

        var item = new CategoryDbM(itemDto);
        await navProp_CategoryCuDto_to_CategoryDbM(itemDto, item);

        _dbContext.Categories.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadCategoryAsync(item.CategoryId, false);
    }

    public async Task<ResponseItemDto<ICategory>> UpdateCategoryAsync(CategoryCuDto itemDto)
    {
        if (itemDto.CategoryId is null)
            throw new ArgumentException($"{nameof(itemDto.CategoryId)} is null.");

        var query = _dbContext.Categories
            .Where(i => i.CategoryId == itemDto.CategoryId);
        var item = await query
            .Include(i => i.AttractionsDbM)
            .Where(i => i.CategoryId == (Guid)itemDto.CategoryId)
            .FirstOrDefaultAsync<CategoryDbM>();

        if (item is null) throw new ArgumentException($"Category {itemDto.CategoryId} does not exist.");

        item.UpdateFromDTO(itemDto);
        await navProp_CategoryCuDto_to_CategoryDbM(itemDto, item);

        _dbContext.Categories.Update(item);
        await _dbContext.SaveChangesAsync();

        return await ReadCategoryAsync(item.CategoryId, false);
    }

    private async Task navProp_CategoryCuDto_to_CategoryDbM(CategoryCuDto itemDto, CategoryDbM item)
    {
        //Assign Attractions
        List<AttractionDbM> attractions = null;
        if (itemDto.AttractionsId is not null)
        {
            attractions = new List<AttractionDbM>();
            foreach (var id in itemDto.AttractionsId)
            {
                var attraction = await _dbContext.Attractions.FirstOrDefaultAsync(i => i.AttractionId == id);
                if (attraction is null) throw new ArgumentException($"Attraction id {id} does not exist.");
                
                attractions.Add(attraction);
            }
        }
        item.AttractionsDbM = attractions;
    }
}