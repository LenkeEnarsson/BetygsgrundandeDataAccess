using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;
using Models.DTO;
using Models;
using models.CuDto;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;
    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }

    public async Task SeedAsync()
    {
        //Create a seeder
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);
        
        var rnd = new Random();

        //Seeding lists 
        var categories = seeder.ItemsToList<CategoryDbM>(50);
        var countries = seeder.ItemsToList<CountryDbM>(10);
        var cities = seeder.ItemsToList<CityDbM>(101);
        var attractions = seeder.ItemsToList<AttractionDbM>(10);//TODO: 1000
        var users = seeder.ItemsToList<UserDbM>(51);
        var reviews = seeder.ItemsToList<ReviewDbM>(30); //TODO: 300

        #region Add foreign key relations

        foreach (var r in reviews)
        {
            r.AttractionDbM = attractions[rnd.Next(attractions.Count)];
            r.UserDbM = users[rnd.Next(users.Count)];
        }

        //Remove reviews from some attractions to guarantee attractions without reviews.
        var nr = seeder.Next((int)attractions.Count/3);
        for(int i = 0; i < nr; i++)
            attractions[i].ReviewsDbM = null;

        foreach (var u in users)
            u.ReviewsDbM = reviews
                .Where(r => r.UserDbM.UserId == u.UserId)
                .ToList();

        foreach (var a in attractions)
        {
            a.CityDbM = cities[rnd.Next(cities.Count)];
            a.CategoriesDbM = categories
                            .Select(c => new { Category = c, Order = rnd.Next() })
                            .OrderBy(x => x.Order)
                            .Take(rnd.Next(1,4))
                            .Select(x => x.Category)
                            .ToList();
            a.ReviewsDbM = reviews
                .Where(b => b.AttractionDbM.AttractionId == a.AttractionId)
                .ToList();
        }

        foreach (var c in categories)
            c.AttractionsDbM = attractions
                .Where(a => a.CategoriesDbM
                .Contains(c))
                .ToList();

        foreach (var c in cities)
        {
            c.CountryDbM = countries[rnd.Next(countries.Count)];
            c.AttractionsDbM = attractions
                .Where(a => a.CityDbM.CityId == c.CityId)
                .ToList();
        }
        foreach (var c in countries)
            c.CitiesDbM = cities
                .Where(ci => ci.CountryDbM.CountryId == c.CountryId)
                .ToList();


        #endregion

        //Add to database
        _dbContext.Categories.AddRange(categories);
        _dbContext.Countries.AddRange(countries);
        _dbContext.Cities.AddRange(cities);
        _dbContext.Attractions.AddRange(attractions);
        _dbContext.Users.AddRange(users);
        _dbContext.Reviews.AddRange(reviews);

        //Save changes to the database
        await _dbContext.SaveChangesAsync();
    }
    public async Task<CountRowsInTablesDbDto> RemoveSeedAsync(bool seeded = true)
    {
        var before = await DbCountRowsAsync();

        await _dbContext.Database.ExecuteSqlRawAsync("EXEC dbo.spDeleteSeeded");

        var after = await DbCountRowsAsync();
        var deleted = new CountRowsInTablesDbDto
        {
            NrUsers = before.NrUsers - after.NrUsers,
            NrAttractionsWithReviews = before.NrAttractionsWithReviews - after.NrAttractionsWithReviews,
            NrAttractionsWithoutReviews = before.NrAttractionsWithoutReviews - after.NrAttractionsWithoutReviews,
            NrTotalAttractions = before.NrTotalAttractions - after.NrTotalAttractions,
            NrCategories = before.NrCategories - after.NrCategories,
            NrCountries = before.NrCountries - after.NrCountries,
            NrCities = before.NrCities - after.NrCities,
            NrReviews = before.NrReviews - after.NrReviews
        };
        return deleted;
    }

    public async Task<CountRowsInTablesDbDto> DbCountRowsAsync()
    {
        var info = await _dbContext.VwInfoDb.FirstOrDefaultAsync();
        return info;
    }

}