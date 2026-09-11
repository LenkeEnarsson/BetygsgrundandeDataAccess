using Seido.Utilities.SeedGenerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class User : IUser, IEquatable<User>, ISeed<User>
    {
        public virtual Guid UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<IReview> Reviews { get; set; } = [];

        #region Constructors & Equals
        public User() { }
        public User(User org) //Deepcopy
        {
            UserId = org.UserId;
            FirstName = org.FirstName;
            LastName = org.LastName;

            foreach (var r in org.Reviews)
                Reviews.Add(r);

            Seeded = org.Seeded;
        }

        public bool Equals(User other) => (this.UserId) == (other.UserId);
        #endregion

        #region Seeding
        public bool Seeded { get; set; }
        public User Seed(SeedGenerator seeder)
        {
            Seeded = true;
            UserId = Guid.NewGuid();
            FirstName = seeder.FirstName;
            LastName = seeder.LastName;

            return this;
        }
        #endregion
    }
}
