namespace Models;

public interface ICountry
{
    public Guid CountryId { get; set; }
    public string Name { get; set; }

    public List<ICity> Cities { get; set; }
}
public interface ICity
{
    public Guid CityId { get; set; }
    public ICountry Country { get; set; }
    public string Name { get; set; }

    public List<IAttraction> Attractions { get; set; }
}

public interface IAttraction
{
    public Guid AttractionId { get; set; }
    public ICity City { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }


    public List<ICategory> Categories{ get; set; }
    public List<IReview> Reviews{ get; set; }
}

public interface ICategory
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }

    public List<IAttraction> Attractions { get; set; }
}

public interface IReview
{
    public Guid ReviewId { get; set; }
    public IAttraction Attraction { get; set; }
    public string Comment { get; set; }
    public byte Score { get; set; }
    public IUser Author { get; set; }
    public DateTime DateMade { get; set; }

}

public interface IUser
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<IReview> Reviews{ get; set; }
}