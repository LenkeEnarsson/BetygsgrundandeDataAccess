using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using models.CuDto;
using Microsoft.EntityFrameworkCore;

namespace DbModels
{
    [Table("Countries", Schema = "suprusr")]
    [Index(nameof(CountryName))]
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

            CountryName = org.Name;
        }
        public CountryDbM UpdateFromDTO(CountryCuDto org)
        {
            if(org.CountryId != this.CountryId) throw new ArgumentException($"Update object and database object does not have the same id.");
            
            CountryName = org.Name;

            return this;
        }
    }
}
