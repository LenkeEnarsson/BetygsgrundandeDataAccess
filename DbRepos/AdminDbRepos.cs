using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync()
    {
        //Create a seeder
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);
        
        var rnd = new Random();

        //Seeding lists 
        var categories = seeder.ItemsToList<CategoryDbM>(50);
        var countries = seeder.ItemsToList<CountryDbM>(15);
        var cities = seeder.ItemsToList<CityDbM>(150);
        var attractions = seeder.ItemsToList<AttractionDbM>(1500);
        var users = seeder.ItemsToList<UserDbM>(100);
        var reviews = seeder.ItemsToList<ReviewDbM>(3000);

        //Add foreign key relations
        foreach (var c in cities)
            c.CountryDbM = countries[rnd.Next(countries. Count)];

        foreach (var a in attractions)
        {
            a.CityDbM = cities[rnd.Next(cities.Count)];
            a.CategoriesDbM = categories
                            .Select(c => new { Category = c, Order = rnd.Next() })
                            .OrderBy(x => x.Order)
                            .Take(rnd.Next(1,4))
                            .Select(x => x.Category)
                            .ToList();
        }

        foreach (var r in reviews)
        {
            r.AttractionDbM = attractions[rnd.Next(attractions.Count)];
            r.UserDbM = users[rnd.Next(users.Count)];
        }

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

    public async Task RemoveSeedAsync(bool seeded)
    {
        //remove existing items in the database
        _dbContext.Attractions.RemoveRange(_dbContext.Attractions.Where(i => i.Seeded == true));
        _dbContext.Categories.RemoveRange(_dbContext.Categories.Where(i => i.Seeded == true));
        _dbContext.Cities.RemoveRange(_dbContext.Cities.Where(i => i.Seeded == true));
        _dbContext.Countries.RemoveRange(_dbContext.Countries.Where(i => i.Seeded == true));
        _dbContext.Reviews.RemoveRange(_dbContext.Reviews.Where(i => i.Seeded == true));
        _dbContext.Users.RemoveRange(_dbContext.Users.Where(i => i.Seeded == true));

        _dbContext.SaveChanges();
    }

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
