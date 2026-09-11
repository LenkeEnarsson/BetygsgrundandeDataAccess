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
    [Table("Users", Schema = "dbo")]
    public class UserDbM : User, ISeed<UserDbM>
    {
        [Key] 
        public override Guid UserId { get; set; }
        [NotMapped] 
        public override List<IReview> Reviews { get => ReviewsDbM?.ToList<IReview>(); set => throw new NotImplementedException(); } //Får ej lägga till Review på denna nivå
        [JsonIgnore]
        public List<ReviewDbM> ReviewsDbM { get; set; }

        public new UserDbM Seed(SeedGenerator seeder)
        {
            base.Seed(seeder);
            return this;
        }
    }
}
