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
        var countries = seeder.ItemsToList<CountryDbM>(15);
        var cities = seeder.ItemsToList<CityDbM>(150);
        var attractions = seeder.ItemsToList<AttractionDbM>(10);
        var users = seeder.ItemsToList<UserDbM>(100);
        var reviews = seeder.ItemsToList<ReviewDbM>(30);

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

    public async Task<GstUsrInfoDbDto> GuestDbInfoAsync()
    {
        var info = await _dbContext.InfoDbView.FirstOrDefaultAsync();
        return info;
    }

    //CRUD Users
    public async Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id, bool flat)
    {
        IUser item;
        if (flat)
        {
            var query = _dbContext.Users.AsNoTracking()
                .Where(i => i.UserId == id);

            item = await query.FirstOrDefaultAsync<IUser>();
        }
        else
        {
            var query = _dbContext.Users.AsNoTracking() //No tracking for reading
                .Include(i => i.ReviewsDbM);

            item = await query.FirstOrDefaultAsync<IUser>();
        }
        
        if (item == null) throw new ArgumentException($"User {id} does not exist");
        return new ResponseItemDto<IUser>()
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }
    public async Task<ResponseItemDto<IUser>> CreateUserAsync(UserCuDto itemCuDto)
    {
        if (itemCuDto.UserId != null)
            throw new ArgumentException($"{nameof(itemCuDto.UserId)} must be null when creating a new object");

        //transfer any changes from DTO to database objects
        //Update individual properties Attraction
        var item = new UserDbM(itemCuDto);

        //Update navigation properties
        await navProp_UserCuDto_to_UserDbM(itemCuDto, item);

        //Note changes in DbContext and changetracker
        _dbContext.Users.Add(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();

        //return the updated item in non-flat mode
        return await ReadUserAsync(item.UserId, false);
    }
    public async Task<ResponseItemDto<IUser>> UpdateUserAsync(UserCuDto itemDto)
    {
        if (itemDto.UserId != null)
            throw new ArgumentException($"{nameof(itemDto.UserId)} must be null when creating a new object");

        //Update individual properties Attraction
        var item = new UserDbM(itemDto);

        //Update navigation properties
        await navProp_UserCuDto_to_UserDbM(itemDto, item);

        //Note changes in DbContext
        _dbContext.Users.Add(item);

        //write to database
        await _dbContext.SaveChangesAsync();

        //return the updated database item in non-flat mode
        return await ReadUserAsync(item.UserId, false);
    }
    public async Task<ResponseItemDto<IUser>> DeleteUserAsync(Guid id)
    {
        //Find the instance with matching id
        var query1 = _dbContext.Users
            .Where(i => i.UserId == id);
        var item = await query1.FirstOrDefaultAsync<UserDbM>();

        //If the item does not exists
        if (item == null) throw new ArgumentException($"User {id} does not exist.");

        //delete in the database model
        _dbContext.Users.Remove(item);

        //write to database in a UoW
        await _dbContext.SaveChangesAsync();
        return new ResponseItemDto<IUser>()
        {
    #if DEBUG
            ConnectionString = _dbContext.dbConnection,
    #endif
            Item = item
        };
    }
    private async Task navProp_UserCuDto_to_UserDbM(UserCuDto itemDtoSrc, UserDbM itemDst)
    {
        //Assign list of Reviews
        List<ReviewDbM> reviews = null;
        if (itemDtoSrc.ReviewIds is not null)
        {
            reviews = new List<ReviewDbM>();
            foreach (var id in itemDtoSrc.ReviewIds)
            {
                var p = await _dbContext.Reviews.FirstOrDefaultAsync(i => i.ReviewId == id);
                if (p is null)
                    throw new ArgumentException($"Review id {id} does not exist.");

                reviews.Add(p);
            }
        }
        itemDst.ReviewsDbM = reviews;
    }

}