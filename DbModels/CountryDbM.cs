using models.CuDto;
using Models;
using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace DbModels
{
    [Table("Countries", Schema = "suprusr")]
    public class CountryDbM : Country, ISeed<CountryDbM>
    {
        [Key] 
        public override Guid CountryId { get; set; }
        [NotMapped] 
        public override List<ICity> Cities { get => CitiesDbM?.ToList<ICity>(); set => throw new NotImplementedException(); } //Får ej lägga till Review på denna nivå
        [JsonIgnore] 
        public List<CityDbM> CitiesDbM { get; set; }

        public CountryDbM(){}
        public new CountryDbM Seed(SeedGenerator seeder)
        {
            base.Seed(seeder);
            return this;
        }

        public CountryDbM (CountryCuDto org)
        {
            if(org.CountryId is not null) CountryId = (Guid)org.CountryId;
            else CountryId = Guid.NewGuid();

            Name = org.Name;
        }
        public CountryDbM UpdateFromDTO(CountryCuDto org)
        {
            if(org.CountryId != this.CountryId) throw new ArgumentException($"Update object and database object does not have the same id.");
            
            Name = org.Name;

            return this;
        }
    }
}
