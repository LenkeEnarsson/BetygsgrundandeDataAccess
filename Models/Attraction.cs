using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{
    public virtual Guid AttractionId { get; set; }

    public string Name { get; set; }
    public bool Seeded {get;set;}


    #region Seeding
    public Attraction Seed (SeedGenerator seeder)
    {
        Seeded = true;
        AttractionId = Guid.NewGuid();
        Name = $"{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}";

        return this;
    }
    #endregion
}
