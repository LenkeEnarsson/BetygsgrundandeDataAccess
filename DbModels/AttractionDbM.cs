using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using models.CuDto;
using Microsoft.EntityFrameworkCore;

namespace DbModels;

[Table("Attractions", Schema = "suprusr")]
[Index(nameof(Title))]
[Index(nameof(CityId), nameof(Title))]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>
{
    [Key] 
    public override Guid AttractionId { get; set; }
    [NotMapped]
    public override ICity City { get => CityDbM; set => throw new NotImplementedException(); } 
    public Guid CityId { get; set; }
    [Required]
    [JsonIgnore]
    [ForeignKey(nameof(CityId))]
    public CityDbM CityDbM { get; set; }
    [NotMapped]
    public override List<IReview> Reviews { get => ReviewsDbM?.ToList<IReview>(); set => throw new NotImplementedException(); } 
    [JsonIgnore]
    public List<ReviewDbM> ReviewsDbM {get;set;}
    [NotMapped]
    public override List<ICategory> Categories{ get => CategoriesDbM?.ToList<ICategory>(); set => throw new NotImplementedException(); }
    [JsonIgnore]
    public List<CategoryDbM> CategoriesDbM { get; set; }

    public AttractionDbM() {}
    public new AttractionDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }


    public AttractionDbM (AttractionCuDto org)
    {
        if(org.AttractionId is not null) AttractionId = (Guid)org.AttractionId;
        else AttractionId = Guid.NewGuid();

        Title = org.Name;
        Description = org.Description;
    }
    public AttractionDbM UpdateFromDTO(AttractionCuDto org)
    {
        if(org.AttractionId != this.AttractionId) throw new ArgumentException($"Update object and database object does not have the same id.");
        
        Title = org.Name;
        Description = org.Description;

        return this;
    }
}
