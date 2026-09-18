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
    [Table("Cities", Schema = "suprusr")]
    public class CityDbM : City, ISeed<CityDbM>
    {
        [Key] 
        public override Guid CityId { get; set; }
        [NotMapped] 
        public override ICountry Country { get => CountryDbM; set => throw new NotImplementedException(); } //Får ej lägga till adress på denna nivå
        [Required] 
        public CountryDbM CountryDbM { get; set; }
        [NotMapped]
        public override List<IAttraction> Attractions { get => AttractionsDbM?.ToList<IAttraction>(); set => throw new NotImplementedException(); } //Får ej lägga till Review på denna nivå
        [JsonIgnore]
        public List<AttractionDbM> AttractionsDbM { get; set; }

        public CityDbM() {}
        public new CityDbM Seed(SeedGenerator seeder)
        {
            base.Seed(seeder);
            return this;
        }
        public CityDbM (CityCuDto org)
        {
            if(org.CityId is not null) CityId = (Guid)org.CityId;
            else CityId = Guid.NewGuid();

            Name = org.Name;
        }
        public CityDbM UpdateFromDTO(CityCuDto org)
        {
            if(org.CityId != this.CityId) throw new ArgumentException($"Update object and database object does not have the same id.");
            
            Name = org.Name;

            return this;
        }
    }
}
