using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{
    public virtual Guid AttractionId { get; set; }
    public virtual ICity City { get; set; }
    public string Name { get; set; }

    public virtual List<ICategory> Categories { get; set; } = [];
    public virtual List<IReview> Reviews { get; set; } = [];


    #region Seeding
    public bool Seeded {get;set;}
    public Attraction Seed (SeedGenerator seeder) //called by City.cs, City set by caller.
    {
        Seeded = true;
        AttractionId = Guid.NewGuid();
        Name = seeder.MusicAlbumName;

        Categories = seeder.ItemsToList<Category>(seeder.Next(1, 4)).ToList<ICategory>();
        foreach (var c in Categories)
            c.Attractions.Add(this);

        //Reviews set by User.cs

        return this;
    }
    #endregion
}
