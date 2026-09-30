using System.Text.RegularExpressions;
using Models;

namespace models.CuDto;


public class CategoryCuDto
{
    public Guid? CategoryId {get; set;}
    public string Name { get; set; }
    public virtual List<Guid> AttractionIds { get; set; } = null;

#if DEBUG
        public string ConnectionString { get; init; }
#endif

    public CategoryCuDto() {}
    public CategoryCuDto(ICategory org)
    {
        CategoryId = org.CategoryId;
        Name = org.CatName;
        AttractionIds = org.Attractions?.Select(i => i.AttractionId).ToList();
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrEmpty(Name) || Name.Length > 200)
            throw new ArgumentException("Name must contain 1-200 characters.");
        
        if (!string.IsNullOrEmpty(Name) && !Regex.IsMatch(Name, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Name may only contain latin letters, numbers (0-9), ´`'.- and spaces.");
    }
}

public class CountryCuDto
{
    public Guid? CountryId {get; set;}
    public string Name { get; set; }
    public virtual List<Guid> CityIds { get; set; } = null;

#if DEBUG
        public string ConnectionString { get; init; }
#endif

    public CountryCuDto() {}
    public CountryCuDto(ICountry org)
    {
        CountryId = org.CountryId;
        Name = org.CountryName;
        CityIds = org.Cities?.Select(i => i.CityId).ToList();
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrEmpty(Name) || Name.Length > 200)
            throw new ArgumentException("Name must contain 1-200 characters.");
        
        if (!string.IsNullOrEmpty(Name) && !Regex.IsMatch(Name, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Name may only contain latin letters, numbers (0-9), ´`'.- and spaces.");
    }
}

public class CityCuDto
{
    public Guid? CityId {get; set;}
    public string Name { get; set; }
    public Guid? CountryId { get; set;}
    public virtual List<Guid> AttractionIds { get; set; } = null;

#if DEBUG
        public string ConnectionString { get; init; }
#endif

    public CityCuDto() {}
    public CityCuDto(ICity org)
    {
        CityId = org.CityId;
        Name = org.CityName;
        AttractionIds = org.Attractions?.Select(i => i.AttractionId).ToList();
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrEmpty(Name) || Name.Length > 200)
            throw new ArgumentException("Name must contain 1-200 characters.");
        
        if (!string.IsNullOrEmpty(Name) && !Regex.IsMatch(Name, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Name may only contain latin letters, numbers (0-9), ´`'.- and spaces.");
    }
}

public class AttractionCuDto
{
    public Guid? AttractionId {get; set;}
    public Guid? CityId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }


    public List<Guid> CategoriesId { get; set; } = null;
    public List<Guid> ReviewsId { get; set; } = null;

#if DEBUG
        public string ConnectionString { get; init; }
#endif
    public AttractionCuDto() {}
    public AttractionCuDto(IAttraction org)
    {
        AttractionId = org.AttractionId;
        CityId = org?.City?.CityId;
        Name = org.Title;
        Description = org.Description;

        CategoriesId = org.Categories?.Select(i => i.CategoryId).ToList();
        ReviewsId = org.Reviews?.Select(i => i.ReviewId).ToList();
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrEmpty(Name) || Name.Length > 200 || string.IsNullOrEmpty(Description) || Description.Length > 200)
            throw new ArgumentException("Name and Description must contain 1-200 characters.");
       
        if (!string.IsNullOrEmpty(Name) && !Regex.IsMatch(Name, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Name may only contain latin letters, numbers (0-9), ´`'.- and spaces.");

        if (!string.IsNullOrEmpty(Description) && !Regex.IsMatch(Description, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Description may only contain latin letters, numbers (0-9), ´`'.- and spaces.");
    }

    
}

public record UserCuDto
{
        public Guid? UserId { get; init; }
        public string Email { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public virtual List<Guid> ReviewIds { get; set; } = null;


#if DEBUG
        public string ConnectionString { get; init; }
#endif

    public UserCuDto() {}
    public UserCuDto(IUser org)
    {
        UserId = org.UserId;
        Email = org?.Email;
        FirstName = org.FirstName;
        LastName = org.LastName;
        Password = org.Password;

        ReviewIds = org.Reviews?.Select(i => i.ReviewId).ToList();
    }

    public void EnsureValidity()
    {
        if (string.IsNullOrEmpty(FirstName) || FirstName.Length > 200 
         || string.IsNullOrEmpty(LastName) || LastName.Length > 200
         || string.IsNullOrEmpty(Email) || Email.Length > 200
         || string.IsNullOrEmpty(Password) || Password.Length > 200)
            throw new ArgumentException("Name, email and password must contain 1-200 characters.");
  
        if (!string.IsNullOrEmpty(Email) && !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email.");
       
        if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(FirstName)
        && !Regex.IsMatch(FirstName, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$")
        || !Regex.IsMatch(LastName, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Name may only contain latin letters, numbers (0-9), ´`'.- and spaces.");

    }
}

public class ReviewCuDto
{
    public Guid? ReviewId {get; set;}
    public Guid? AttractionId { get; set; }
    public string Comment { get; set; }
    public byte? Score { get; set; }
    public Guid? AuthorId { get; set; }
    public DateTime? DateMade { get; set; }

#if DEBUG
    public string ConnectionString { get; init; }
#endif

    public ReviewCuDto() {}
    public ReviewCuDto(IReview org)
    {
        ReviewId = org.ReviewId;
        AttractionId = org?.AttractionInterface.AttractionId;
        Comment = org.Comment;
        Score = org.Score;
        AuthorId = org.UserInterface.UserId;
        DateMade = org.DateMade;
    }

    public void EnsureValidity()
    {
        if (AttractionId is null || AuthorId is null)
            throw new ArgumentException("Review must be connected to an attraction and an author.");
        
        if (string.IsNullOrEmpty(Comment) || Comment.Length > 1000 || Score is null || Score <1 || Score >5)
            throw new ArgumentException("Comment must contain 1-1000 characters. Score must be 1-5");
       
        if (!string.IsNullOrEmpty(Comment) && !Regex.IsMatch(Comment, @"^[a-zA-Z0-9\u00C0-\u024F\u1E00-\u1EFF\s´`'\.\-]{1,50}$"))
            throw new ArgumentException("Comment may only contain latin letters, numbers (0-9), ´`'.- and spaces.");
    }
}
