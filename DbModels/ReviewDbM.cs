using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

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

    [NotMapped] public override IUser Author{ get => UserDbM; set => throw new NotImplementedException(); }
    [Required] public UserDbM UserDbM { get; set; }

    public new ReviewDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
}
