using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using models.CuDto;

namespace DbModels;

[Table("Reviews", Schema = "usr")]
public class ReviewDbM : Review, ISeed<ReviewDbM>
{
    [Key]
    public override Guid ReviewId { get; set; }
    [NotMapped]
    public override IAttraction Attraction { get => AttractionDbM; set => throw new NotImplementedException(); }
    [Required]
    public AttractionDbM AttractionDbM {get;set;}

    [NotMapped] public override IUser UserId{ get => UserDbM; set => throw new NotImplementedException(); }
    [Required] public UserDbM UserDbM { get; set; }

    public ReviewDbM() {}
    public new ReviewDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
    public ReviewDbM (ReviewCuDto org)
    {
        if(org.ReviewId is not null) ReviewId = (Guid)org.ReviewId;
        else ReviewId = Guid.NewGuid();

        Comment = org.Comment;
        if(org.Score is not null) Score = (byte)org.Score;
        if(org.DateMade is not null) DateMade = (DateTime)org.DateMade;
    }
    public ReviewDbM UpdateFromDTO(ReviewCuDto org)
    {
        if(org.ReviewId != this.ReviewId) throw new ArgumentException($"Update object and database object does not have the same id.");
        
        if(org.ReviewId is not null) ReviewId = (Guid)org.ReviewId;
        else ReviewId = Guid.NewGuid();

        Comment = org.Comment;
        if(org.Score is not null) Score = (byte)org.Score;
        if(org.DateMade is not null) DateMade = (DateTime)org.DateMade;

        return this;
    }
}
