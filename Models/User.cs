using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class User : IUser, ISeed<User>
    {
        public virtual Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<IReview> Reviews { get; set; } = [];


        #region Seeding
        public bool Seeded { get; set; }
        public User Seed(SeedGenerator seeder)
        {
            Seeded = true;
            UserId = Guid.NewGuid();
            FirstName = seeder.FirstName;
            LastName = seeder.LastName;

            Reviews = seeder.ItemsToList<Review>(seeder.Next(1, 55)).ToList<IReview>();
            foreach (var r in Reviews)
                r.Author = this;



            return this;
        }
        #endregion
    }
}
