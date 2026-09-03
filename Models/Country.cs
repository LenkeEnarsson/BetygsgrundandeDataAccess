using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Country : ICountry, ISeed<Country>
    {
        public virtual Guid CountryId { get; set; }
        public string Name { get; set; }

        public virtual List<ICity> Cities { get; set; } = [];

        #region Seeding
        public bool Seeded { get; set; }
        public Country Seed(SeedGenerator seeder)
        {
            Seeded = true;
            CountryId = Guid.NewGuid();
            Name = seeder.Country;
            
            Cities = seeder.ItemsToList<City>(seeder.Next(1, 55)).ToList<ICity>();
            foreach (var ci in Cities)
            {
                ci.Name = seeder.City(this.Name);
                ci.Country = this;
            }

            return this;
        }
        #endregion
    }
}
