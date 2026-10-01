using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class CountryDbRepos
{
    #region fields and constructors
    private ILogger<CountryDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CountryDbRepos(ILogger<CountryDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

    public async Task<ResponsePageDto<ICountry>> ReadCountryListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        var query = _dbContext.Countries.AsNoTracking();
        if (!flat)
            query = query
                .Include(i => i.CitiesDbM);

        if (!string.IsNullOrEmpty(filter)) 
            query = query
                .Where(i => i.CountryName.ToLower().Contains(filter));
        
        query = query.Where(i => i.Seeded == seeded);

        return new ResponsePageDto<ICountry>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            DbItemsCount = await query.CountAsync(),

            PageItems = await query
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync<ICountry>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ResponseItemDto<ICountry>> ReadCountryAsync(Guid id, bool flat)
    {
        var query = _dbContext.Countries.AsNoTracking()
            .Where(i => i.CountryId == id);

        if (!flat)
            query = query
                .Include(i => i.CitiesDbM)
                .Where(i => i.CountryId == id);

        var item = await query.FirstOrDefaultAsync<ICountry>();

        if (item is null) throw new ArgumentException($"Country {id} does not exist.");

        return new ResponseItemDto<ICountry>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICountry>> DeleteCountryAsync(Guid id)
    {
        var item = await _dbContext.Countries.FirstOrDefaultAsync(i => i.CountryId == id);
        if (item is null) throw new ArgumentException($"Country {id} does not exist.");
        
        _dbContext.Countries.Remove(item);
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<ICountry>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICountry>> CreateCountryAsync(CountryCuDto itemDto)
    {
        if (itemDto.CountryId is not null)
            throw new ArgumentException($"{nameof(itemDto.CountryId)} must be null when creating a new object.");

        var item = new CountryDbM(itemDto);
        await navProp_CountryCuDto_to_CountryDbM(itemDto, item);

        _dbContext.Countries.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadCountryAsync(item.CountryId, false);
    }

    public async Task<ResponseItemDto<ICountry>> UpdateCountryAsync(CountryCuDto itemDto)
    {
        if (itemDto.CountryId is null)
            throw new ArgumentException($"{nameof(itemDto.CountryId)} is null.");

        var query = _dbContext.Countries
            .Where(i => i.CountryId == itemDto.CountryId);
        var item = await query
            .Include(i => i.CitiesDbM)
            .FirstOrDefaultAsync<CountryDbM>();

        if (item is null) throw new ArgumentException($"Country {itemDto.CountryId} does not exist.");

        item.UpdateFromDTO(itemDto);
        await navProp_CountryCuDto_to_CountryDbM(itemDto, item);

        _dbContext.Countries.Update(item);
        await _dbContext.SaveChangesAsync();

        return await ReadCountryAsync(item.CountryId, false);
    }

    private async Task navProp_CountryCuDto_to_CountryDbM(CountryCuDto itemDto, CountryDbM item)
    {
        //Assign Cities
        List<CityDbM> cities = null;
        if (itemDto.CitiesId is not null)
        {
            cities = new List<CityDbM>();
            foreach (var id in itemDto.CitiesId)
            {
                var city = await _dbContext.Cities.FirstOrDefaultAsync(i => i.CityId == id);
                if (city is null) throw new ArgumentException($"City id {id} does not exist.");
                
                cities.Add(city);
            }
        }
        item.CitiesDbM = cities;
    }
}