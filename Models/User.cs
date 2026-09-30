using Seido.Utilities.SeedGenerator;
using Newtonsoft.Json;

namespace Models
{
    public class User : IUser, IEquatable<User>, ISeed<User>
    {
        public virtual Guid UserId { get; set; }
        public string Username { get; set; }
        [JsonIgnore]public string Email {get;set;}
        [JsonIgnore]public string Password {get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual List<IReview> Reviews { get; set; } = null;

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
            Username = seeder.LatinWords(1)[0];

            return this;
        }
        #endregion
    }
}
