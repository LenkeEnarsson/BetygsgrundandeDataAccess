using Models;

namespace models.Dto;

//TODO: ÖVersätt till egen modell
public class AttractionCuDto
{
    public Guid AttractionId {get; set;}
    public Guid? CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }


    public List<Guid> CategoriesId { get; set; } = null;
    public List<Guid> ReviewsId { get; set; } = null;

    public AttractionCuDto() {}
    public AttractionCuDto(IAttraction org)
    {
        AttractionId = org.AttractionId;
        CityId = org?.City?.CityId;
        Name = org.Name;
        Description = org.Description;

        CategoriesId = org.Categories?.Select(i => i.CategoryId).ToList();
        ReviewsId = org.Reviews?.Select(i => i.ReviewId).ToList();
    }
}