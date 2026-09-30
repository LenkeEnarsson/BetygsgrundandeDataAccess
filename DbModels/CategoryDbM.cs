using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using models.CuDto;
using Microsoft.EntityFrameworkCore;

namespace DbModels
{
    [Table("Categories", Schema = "suprusr")]
    [Index(nameof(CatName))]
    public class CategoryDbM : Category, ISeed<CategoryDbM>
    {
        [Key]
        public override Guid CategoryId { get; set; }

        [NotMapped]
        public override List<IAttraction> Attractions{ get => AttractionsDbM?.ToList<IAttraction>(); set => throw new NotImplementedException(); } //Får ej lägga till Review på denna nivå
        [JsonIgnore]
        public List<AttractionDbM> AttractionsDbM { get; set; }

        public CategoryDbM() {}
        public new CategoryDbM Seed(SeedGenerator seeder)
        {
            base.Seed(seeder);
            return this;
        }
        public CategoryDbM (CategoryCuDto org)
        {
            if(org.CategoryId is not null) CategoryId = (Guid)org.CategoryId;
            else CategoryId = Guid.NewGuid();

            CatName = org.Name;
        }

        public CategoryDbM UpdateFromDTO(CategoryCuDto org) //Only updating individual proprties, needs navProp for references
        {
            if(org.CategoryId != this.CategoryId) throw new ArgumentException($"Update object and database object does not have the same id.");
            
            CatName = org.Name;

            return this;
        }

    }
}