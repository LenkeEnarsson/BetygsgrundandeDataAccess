using System;
using System.Collections.Generic;
using System.Text;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    using Seido.Utilities.SeedGenerator;
    using Models;

namespace DbModels
{
    [Table("Categories", Schema = "suprusr")]
    public class CategoryDbM : Category, ISeed<CategoryDbM>
    {
        [Key]
        public override Guid CategoryId { get; set; }

        [NotMapped]
        public override List<IAttraction> Attractions{ get => AttractionsDbM?.ToList<IAttraction>(); set => throw new NotImplementedException(); } //Får ej lägga till Review på denna nivå
        [JsonIgnore]
        public List<AttractionDbM> AttractionsDbM { get; set; }

        public new CategoryDbM Seed(SeedGenerator seeder)
        {
            base.Seed(seeder);
            return this;
        }
    }

}
