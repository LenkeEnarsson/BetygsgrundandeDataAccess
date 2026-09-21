using Seido.Utilities.SeedGenerator;
using System.Net.Mail;

namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{
    public virtual Guid AttractionId { get; set; }
    public virtual ICity City { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public virtual List<ICategory> Categories { get; set; } = [];
    public virtual List<IReview> Reviews { get; set; } = [];

    #region Constructors
    public Attraction() { }
    public Attraction(Attraction org) 
    {
        AttractionId = org.AttractionId;
        City = org.City;
        Title = org.Title;
        Description = org.Description;

        foreach (var c in org.Categories)
            Categories.Add(c);
        foreach (var r in org.Reviews)
            Reviews.Add(r);
        
        Seeded = org.Seeded;
    }
    #endregion

    #region Seeding
    public bool Seeded {get;set;}
    public Attraction Seed (SeedGenerator seeder)
    {
        Seeded = true;
        AttractionId = Guid.NewGuid();
        Title = seeder.MusicGroupName;
        Description = seeder.LatinParagraph;

        return this;
    }
    #endregion
}
