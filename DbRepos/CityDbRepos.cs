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
        ICity item;
        if (flat)
        {
            var query = _dbContext.Cities.AsNoTracking()
                .Where(i => i.CityId == id);

            item = await query.FirstOrDefaultAsync<ICity>();
        }
        else
        {
            var query = _dbContext.Cities.AsNoTracking() //No tracking for reading
                .Include(i => i.CountryDbM)
                .Include(i => i.AttractionsDbM)
                .Where(i => i.CityId == id);

            item = await query.FirstOrDefaultAsync<ICity>();
        }
        
        if (item == null) throw new ArgumentException($"City {id} does not exist");
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
        IQueryable<CityDbM> query;
        if (flat)
        {
            query = _dbContext.Cities.AsNoTracking();
        }
        else
        {
            query = _dbContext.Cities.AsNoTracking()
                .Include(i => i.CountryDbM)
                .Include(i => i.AttractionsDbM);
        }

        if (string.IsNullOrEmpty(filter))
            return new ResponsePageDto<ICity>()
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

                .ToListAsync<ICity>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
        else
            return new ResponsePageDto<ICity>()
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif

                DbItemsCount = await query
                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                                i.Name.ToLower().Contains(filter)).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.Name.ToLower().Contains(filter))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<ICity>(),

                PageNr = pageNumber,
                PageSize = pageSize
            };
    }

    public async Task<ResponseItemDto<ICity>> DeleteCityAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Cities
            .Where(i => i.CityId == id);
        var item = await query1.FirstOrDefaultAsync<CityDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"City {id} does not exist.");

        //delete in the database model
        _dbContext.Cities.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<ICity>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<ICity>> CreateCityAsync(CityCuDto itemCuDto)
    {
        if (itemCuDto.CityId != null)
            throw new ArgumentException($"{nameof(itemCuDto.CityId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties City
        var item = new CityDbM(itemCuDto);

        //Update navigation properties
        await navProp_CityCUdto_to_CityDbM(itemCuDto, item);

        //Note changes in DbContext and changetracker
        _dbContext.Cities.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadCityAsync(item.CityId, false);
    }

    public async Task<ResponseItemDto<ICity>> UpdateCityAsync(CityCuDto itemDto)
    {
        if (itemDto.CityId != null)
            throw new ArgumentException($"{nameof(itemDto.CityId)} must be null when creating a new object");

        //Update individual properties City
        var item = new CityDbM(itemDto);

        //Update navigation properties
        await navProp_CityCUdto_to_CityDbM(itemDto, item);

        //Note changes in DbContext
        _dbContext.Cities.Add(item);

        //write to database
        await _dbContext.SaveChangesAsync();

        //return the updated database item in non-flat mode
        return await ReadCityAsync(item.CityId, false);
    }

    private async Task navProp_CityCUdto_to_CityDbM(CityCuDto itemDtoSrc, CityDbM itemDst)
    {
        //Assign Country
        itemDst.CountryDbM = (itemDtoSrc.CountryId is not null) ? await _dbContext.Countries.FirstOrDefaultAsync(
            a => (a.CountryId == itemDtoSrc.CountryId)) : null;

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
