using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Country : ICountry, IEquatable<Country>, ISeed<Country>
    {
        public virtual Guid CountryId { get; set; }
        public string Name { get; set; }

        public virtual List<ICity> Cities { get; set; } = [];

        #region Constructors & Equals
        public Country() { }
        public Country(Country org) //Deepcopy
        {
            CountryId = org.CountryId;
            Name = org.Name;

            foreach (var c in org.Cities)
                Cities.Add(c);

            Seeded = org.Seeded;
        }
        public bool Equals(Country other) => this.Name == other.Name;
        #endregion

        #region Seeding
        public bool Seeded { get; set; }
        public Country Seed(SeedGenerator seeder)
        {
            Seeded = true;
            CountryId = Guid.NewGuid();
            Name = seeder.Country;

            return this;
        }
        #endregion
    }
}
