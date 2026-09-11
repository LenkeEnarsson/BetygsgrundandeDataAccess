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

        public new CountryDbM Seed(SeedGenerator seeder)
        {
            base.Seed(seeder);
            return this;
        }
    }
}
