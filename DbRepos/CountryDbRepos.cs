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

    public async Task<ResponseItemDto<ICountry>> ReadCountryAsync(Guid id, bool flat)
    {
        ICountry item;
        if (flat)
        {
            var query = _dbContext.Countries.AsNoTracking()
                .Where(i => i.CountryId == id);

            item = await query.FirstOrDefaultAsync<ICountry>();
        }
        else
        {
            var query = _dbContext.Countries.AsNoTracking() //No tracking for reading
                .Include(i => i.CitiesDbM)
                .Where(i => i.CountryId == id);

            item = await query.FirstOrDefaultAsync<ICountry>();
        }
        
        if (item == null) throw new ArgumentException($"Country {id} does not exist");
        return new ResponseItemDto<ICountry>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

public async Task<ResponsePageDto<ICountry>> ReadCountryListAsync(bool seeded = false, bool flat = true, string filter = "", int pageNumber = 0, int pageSize = 10)
    {
        IQueryable<CountryDbM> query;
        if (flat)
        {
            query = _dbContext.Countries.AsNoTracking();
        }
        else
        {
            query = _dbContext.Countries.AsNoTracking()
                .Include(i => i.CitiesDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<ICountry>()
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

                .ToListAsync<ICountry>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
            return new ResponsePageDto<ICountry>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                                i.CountryName.ToLower().Contains(filter)).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.CountryName.ToLower().Contains(filter))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<ICountry>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
    }

    public async Task<ResponseItemDto<ICountry>> DeleteCountryAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Countries
            .Where(i => i.CountryId == id);
        var item = await query1.FirstOrDefaultAsync<CountryDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"Country {id} does not exist.");

        //delete in the database model
        _dbContext.Countries.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<ICountry>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICountry>> CreateCountryAsync(CountryCuDto itemCuDto)
    {
        if (itemCuDto.CountryId != null)
            throw new ArgumentException($"{nameof(itemCuDto.CountryId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Country
        var item = new CountryDbM(itemCuDto);

        //Update navigation properties
        await navProp_CountryCUdto_to_CountryDbM(itemCuDto, item);

        //Note changes in DbContext and changetracker
        _dbContext.Countries.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadCountryAsync(item.CountryId, false);
    }
    
    public async Task<ResponseItemDto<ICountry>> UpdateCountryAsync(CountryCuDto itemDto)
    {
        if (itemDto.CountryId != null)
            throw new ArgumentException($"{nameof(itemDto.CountryId)} must be null when creating a new object");

        //Update individual properties Country
        var item = new CountryDbM(itemDto);

        //Update navigation properties
        await navProp_CountryCUdto_to_CountryDbM(itemDto, item);

        //Note changes in DbContext
        _dbContext.Countries.Add(item);

        //write to database
        await _dbContext.SaveChangesAsync();

        //return the updated database item in non-flat mode
        return await ReadCountryAsync(item.CountryId, false);
    }

    private async Task navProp_CountryCUdto_to_CountryDbM(CountryCuDto itemDtoSrc, CountryDbM itemDst)
    {
        //Assign list of Cities
        List<CityDbM> cities = null;
        if (itemDtoSrc.CityIds is not null)
        {
            cities = new List<CityDbM>();
            foreach (var id in itemDtoSrc.CityIds)
            {
                var p = await _dbContext.Cities.FirstOrDefaultAsync(i => i.CityId == id);
                if (p is null)
                    throw new ArgumentException($"City id {id} does not exist.");

                cities.Add(p);
            }
        }
        itemDst.CitiesDbM = cities;
    }

}
