using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Attractions", Schema = "suprusr")]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>
{
    [Key] 
    public override Guid AttractionId { get; set; }
    [NotMapped]
    public override ICity City { get => CityDbM; set => throw new NotImplementedException(); } 
    [Required]
    public CityDbM CityDbM { get; set; }
    [NotMapped]
    public override List<IReview> Reviews { get => ReviewsDbM?.ToList<IReview>(); set => throw new NotImplementedException(); } 
    [JsonIgnore]
    public List<ReviewDbM> ReviewsDbM {get;set;}
    [NotMapped]
    public override List<ICategory> Categories{ get => CategoriesDbM?.ToList<ICategory>(); set => throw new NotImplementedException(); }
    [JsonIgnore]
    public List<CategoryDbM> CategoriesDbM { get; set; }


    public new AttractionDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
}
