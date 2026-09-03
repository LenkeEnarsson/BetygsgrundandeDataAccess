using Seido.Utilities.SeedGenerator;

namespace Models;

public class Review : IReview, ISeed<Review>
{
    public virtual Guid ReviewId { get; set; }
    public virtual IAttraction Attraction { get; set; }
    public string Comment { get; set; }
    public byte Score { get; set; }
    public virtual IUser Author { get; set; }
    public DateTime DateMade { get; set; }


    #region Seeding
    public bool Seeded {get;set;}
    public Review Seed (SeedGenerator seeder) //called by Attractions.cs, Attraction set by caller.
    {
        Seeded = true;
        ReviewId = Guid.NewGuid();
        Comment = seeder.LatinSentence;
        Score = (byte)seeder.Next(1, 5);
        DateMade = seeder.DateAndTime(1970, DateTime.Today.Year);

        return this;
    }
    #endregion
}
