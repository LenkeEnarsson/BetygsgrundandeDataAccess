using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class CityDbRepos
{
    #region fields and constructors
    private ILogger<CityDbRepos> _logger;
    private readonly MainDbContext _dbContext;


    public CityDbRepos(ILogger<CityDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    #endregion

    public async Task<ResponseItemDto<ICity>> ReadCityAsync(Guid id, bool flat)
    {
        var query = _dbContext.Cities.AsNoTracking()
            .Where(c => c.CityId == id);

        if (!flat)
            query = query
                .Include(i => i.CountryDbM)
                .Include(c => c.AttractionsDbM.Where(a => a.ReviewsDbM.Any())) // Include only if attraction has 1+ reviews (is recommended)
                .ThenInclude(a => a.CategoriesDbM);

        var item = await query.FirstOrDefaultAsync<ICity>();

        if (item is null) throw new ArgumentException($"City {id} does not exist.");
        
        return new ResponseItemDto<ICity>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponsePageDto<ICity>> ReadCityListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        var query = _dbContext.Cities.AsNoTracking();
        
        if (!flat)
            query = query
                .Include(i => i.CountryDbM)
                .Include(i => i.AttractionsDbM);

        if (!string.IsNullOrEmpty(filter))
            query = query.Where(i => i.CityName.ToLower().Contains(filter));
        
        query = query.Where(i => i.Seeded == seeded);

        return new ResponsePageDto<ICity>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),
            
            PageItems = await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync<ICity>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
    }
    
    public async Task<ResponseItemDto<ICity>> DeleteCityAsync(Guid id)
    {
        var item = await _dbContext.Cities.FirstOrDefaultAsync(i => i.CityId == id);
        
        if (item is null) throw new ArgumentException($"City {id} does not exist.");
        
        _dbContext.Cities.Remove(item);
        
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<ICity>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICity>> CreateCityAsync(CityCuDto itemDto)
    {
        if (itemDto.CityId is not null)
            throw new ArgumentException($"{nameof(itemDto.CityId)} must be null when creating a new object.");

        var item = new CityDbM(itemDto);
        await navProp_CityCuDto_to_CityDbM(itemDto, item);

        _dbContext.Cities.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadCityAsync(item.CityId, false);
    }

    public async Task<ResponseItemDto<ICity>> UpdateCityAsync(CityCuDto itemDto)
    {
        if (itemDto.CityId is null)
            throw new ArgumentException($"{nameof(itemDto.CityId)} is null.");

        var item = await _dbContext.Cities
            .Include(i => i.CountryDbM)
            .Include(i => i.AttractionsDbM)
            .Where(i => i.CityId == (Guid)itemDto.CityId)
            .FirstOrDefaultAsync<CityDbM>();

        if (item is null) throw new ArgumentException($"City {itemDto.CityId} does not exist.");

        item.UpdateFromDTO(itemDto);
        await navProp_CityCuDto_to_CityDbM(itemDto, item);

        _dbContext.Cities.Update(item);
        await _dbContext.SaveChangesAsync();
        
        return await ReadCityAsync(item.CityId, false);
    }

    private async Task navProp_CityCuDto_to_CityDbM(CityCuDto itemDtoSrc, CityDbM itemDst)
    {
        //Assign Country
        itemDst.CountryDbM = itemDtoSrc.CountryId is not null
            ? await _dbContext.Countries.FirstOrDefaultAsync(i => i.CountryId == itemDtoSrc.CountryId)
            : null;

        //Assign list of Attractions
        List<AttractionDbM> attractions = null;
        if (itemDtoSrc.AttractionsId is not null)
        {
            attractions = new List<AttractionDbM>();
            foreach (var id in itemDtoSrc.AttractionsId)
            {
                var attraction = await _dbContext.Attractions.FirstOrDefaultAsync(i => i.AttractionId == id);
                if (attraction is null) throw new ArgumentException($"Attraction id {id} does not exist.");
                
                attractions.Add(attraction);
            }
        }
        itemDst.AttractionsDbM = attractions;
    }
}