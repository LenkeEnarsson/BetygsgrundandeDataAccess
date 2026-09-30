using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using DbModels;
using DbContext;
using Models.DTO;
using models.CuDto;

namespace DbRepos;

public class AttractionPropertyDbRepos
{
    #region fields and constructors
    private ILogger<AttractionPropertyDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionPropertyDbRepos(ILogger<AttractionPropertyDbRepos> logger, MainDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }
    #endregion

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
                                i.CityName.ToLower().Contains(filter)).CountAsync(),

                PageItems = await query

                //Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            i.CityName.ToLower().Contains(filter))

                //Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)

                .ToListAsync<ICity>(),

                PageNr = pageNumber,
                PageSize = pageSize
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
}
