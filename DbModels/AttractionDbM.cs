using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using models.Dto;

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

        Name = org.Name;
        Description = org.Description;
    }

/// <summary>
/// Fill scalar properties in AttractionDbM from AttractionCuDto. 
/// Needs navProp_AttractionCUdto_to_AttractionDbM to fill object references.
/// </summary>
/// <param name="org"></param>
/// <returns></returns>
/// <exception cref="ArgumentException"></exception>
    public AttractionDbM UpdateFromDTO(AttractionCuDto org)
    {
        if(org.AttractionId != this.AttractionId) throw new ArgumentException($"Update object and database object does not have the same id.");
        
        Name = org.Name;
        Description = org.Description;

        return this;
    }
}
