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
        public string Name { get; set; }

        public virtual List<IAttraction> Attractions { get; set; } = [];

        #region Constructors & Equals
        public City() { }
        public City(City org) //Deepcopy
        {
            CityId = org.CityId;
            Country = org.Country;
            Name = org.Name;
            foreach (var a in org.Attractions)
                Attractions.Add(a);

            Seeded = org.Seeded;
        }

        public bool Equals(City other) => (this.Name, this.Country) == (other.Name, other.Country);
        #endregion

        #region Seeding
        public bool Seeded { get; set; }
        public City Seed(SeedGenerator seeder) //Method called from Country.cs, Name & Country set by caller
        {
            Seeded = true;
            CityId = Guid.NewGuid();

            return this;
        }
        #endregion
    }
}
