using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class City : ICity, IEquatable<City>, ISeed<City>
    {
        public virtual Guid CityId { get; set; }
        public virtual ICountry Country { get; set; }
        public string CityName { get; set; }

        public virtual List<IAttraction> Attractions { get; set; } = [];

        #region Constructors & Equals
        public City() { }
        public City(City org) //Deepcopy
        {
            CityId = org.CityId;
            Country = org.Country;
            CityName = org.CityName;
            foreach (var a in org.Attractions)
                Attractions.Add(a);

            Seeded = org.Seeded;
        }

        public bool Equals(City other) => (this.CityName, this.Country) == (other.CityName, other.Country);
        #endregion

        #region Seeding
        public bool Seeded { get; set; }
        public City Seed(SeedGenerator seeder) //Method called from Country.cs, Name & Country set by caller
        {
            Seeded = true;
            CityId = Guid.NewGuid();
            CityName = seeder.FirstName;

            return this;
        }
        #endregion
    }
}
