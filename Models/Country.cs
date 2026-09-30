using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Country : ICountry, IEquatable<Country>, ISeed<Country>
    {
        public virtual Guid CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual List<ICity> Cities { get; set; } = [];

        #region Constructors & Equals
        public Country() { }
        public Country(Country org) //Deepcopy
        {
            CountryId = org.CountryId;
            CountryName = org.CountryName;

            foreach (var c in org.Cities)
                Cities.Add(c);

            Seeded = org.Seeded;
        }
        public bool Equals(Country other) => this.CountryName == other.CountryName;
        #endregion

        #region Seeding
        public bool Seeded { get; set; }
        public Country Seed(SeedGenerator seeder)
        {
            Seeded = true;
            CountryId = Guid.NewGuid();
            CountryName = seeder.Country;

            return this;
        }
        #endregion
    }
}
