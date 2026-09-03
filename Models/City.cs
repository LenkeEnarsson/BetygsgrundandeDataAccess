using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class City : ICity, ISeed<City>
    {
        public virtual Guid CityId { get; set; }
        public virtual ICountry Country { get; set; }
        public string Name { get; set; }

        public virtual List<IAttraction> Attractions { get; set; } = [];

        #region Seeding
        public bool Seeded { get; set; }
        public City Seed(SeedGenerator seeder) //Method called from Country.cs, Name & Country set by caller
        {
            Seeded = true;
            CityId = Guid.NewGuid();

            Attractions = seeder.ItemsToList<Attraction>(seeder.Next(1, 55)).ToList<IAttraction>();
            foreach (var a in Attractions)
                a.City = this;

            return this;
        }
        #endregion
    }
}
