using System.Text.RegularExpressions;
using Models;

namespace models.Dto;

public class AttractionCuDto
{
    public Guid? AttractionId {get; set;}
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

    public void EnsureValidity()
    {
        // RegEx check to ensure filter only contains a-z, 0-9, and spaces
        if (!string.IsNullOrEmpty(Name) && !Regex.IsMatch(Name, @"^[a-zA-Z0-9\s]*$")) //TODO: Korrekt Regex
        {
            throw new ArgumentException("Attraction name can only contain letters (a-z), numbers (0-9), and spaces.");
        }
        if (!string.IsNullOrEmpty(Description) && !Regex.IsMatch(Description, @"^[a-zA-Z0-9\s]*$"))
        {
            throw new ArgumentException("Description can only contain letters (a-z), numbers (0-9), and spaces.");
        }
    }

    
}