using System.ComponentModel.DataAnnotations;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class Review : IReview, IEquatable<Review>, ISeed<Review>
{
    public virtual Guid ReviewId { get; set; }
    public virtual IAttraction Attraction { get; set; }
    public string Comment { get; set; }
    public byte Score { get; set; }
    public virtual IUser UserId { get; set; }
    public DateTime DateMade { get; set; }

    #region Constructors & Equals
    public Review() { }
    public Review(Review org) //Deepcopy
    {
        ReviewId = org.ReviewId;
        Attraction = org.Attraction;
        Comment = org.Comment;
        Score = org.Score;
        UserId = org.UserId;
        DateMade = org.DateMade;

        Seeded = org.Seeded;
    }
    public bool Equals(Review other) => (this.Attraction, this.Comment, this.Score, this.UserId) == (other.Attraction, other.Comment, other.Score, other.UserId);
    #endregion

    #region Seeding
    public bool Seeded {get;set;}
    public Review Seed (SeedGenerator seeder)
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
