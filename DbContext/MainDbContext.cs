using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Configuration;
using DbModels;
using Models.DTO;
using Microsoft.Extensions.Hosting.Internal;
using DbContext.Extensions;

namespace DbContext;

//DbContext namespace is a fundamental EFC layer of the database context and is
//used for all Database connection as well as for EFC CodeFirst migration and database updates 
public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
{
        DatabaseConnections _databaseConnections;

#if DEBUG
    // remove password from connection string in debug mode
    // this is useful for debugging and logging purposes, but should not be used in production code
    public string dbConnection => System.Text.RegularExpressions.Regex.Replace(
        this.Database.GetConnectionString() ?? "", @"(pwd|password)=[^;]*;?", "",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase);
#endif

    #region C# model of database tables
    public DbSet<AttractionDbM> Attractions { get; set; }
    public DbSet<CategoryDbM> Categories{ get; set; }
    public DbSet<CityDbM> Cities { get; set; }
    public DbSet<CountryDbM> Countries{ get; set; }
    public DbSet<ReviewDbM> Reviews { get; set; }
    public DbSet<UserDbM> Users { get; set; }
    #endregion

    #region Views
    public DbSet<GstUsrInfoDbDto> VwInfoDb { get; set; }

    #endregion

    #region constructors
    public MainDbContext() { }
    public MainDbContext(DbContextOptions options, DatabaseConnections databaseConnections) : base(options)
    { 
        _databaseConnections = databaseConnections;
    }
    #endregion

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        #region Model views
        modelBuilder.Entity<GstUsrInfoDbDto>().ToView(nameof(VwInfoDb), "gstusr").HasNoKey();
        #endregion

        #region Override foreign key deletion behaviour
        /*
        modelBuilder.Entity(nameof(DbModels.ReviewDbM), b => //Each review in the review-table
        {
            b.HasOne(nameof(DbModels.UserDbM), nameof(UserDbM)) //has one user
                .WithMany(nameof(UserDbM.ReviewsDbM)) //which has many reviews
                .HasForeignKey("UserIdDbM") //which has a foreign key
                .OnDelete(DeleteBehavior.Cascade); //when this foreign key is deleted, delete the review object
            b.Navigation(nameof(UserDbM));
        });
*/
/*
        modelBuilder.Entity<ReviewDbM>()
            .HasOne(review => review.UserDbM) //has one user
            .WithMany(user => user.ReviewsDbM) //which has many reviews
            .HasForeignKey(nameof(ReviewDbM.UserDbM)) //which has a foreign key
            .OnDelete(DeleteBehavior.Cascade); //when this foreign key is deleted, delete the review object
*/
/*
        modelBuilder.Entity(nameof(DbModels.ReviewDbM), b =>
        {
            b.HasOne(nameof(DbModels.AttractionDbM), nameof(AttractionDbM))
                .WithMany(nameof(UserDbM.ReviewsDbM))
                .HasForeignKey(nameof(ReviewDbM.AttractionDbM))
                .OnDelete(DeleteBehavior.Cascade); 
            b.Navigation(nameof(AttractionDbM));
        });

        modelBuilder.Entity("DbModels.ReviewDbM", b =>
        {
            b.HasOne("DbModels.AttractionDbM", "AttractionDbM")
                .WithMany("ReviewDbM")
                .HasForeignKey("AttractionId")
                .OnDelete(DeleteBehavior.Cascade);
            b.Navigation("AttractionDbM");
        });
*/
        #endregion
        base.OnModelCreating(modelBuilder);
    }

    #region DbContext SQL Server
    public class SqlServerDbContext : MainDbContext
    {
        public SqlServerDbContext() { }
        public SqlServerDbContext(DbContextOptions options, DatabaseConnections databaseConnections) 
            : base(options, databaseConnections) { }


        //Used only for CodeFirst Database Migration and database update commands
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = optionsBuilder.ConfigureForDesignTime(
                    (options, connectionString) => options.UseSqlServer(connectionString, options => options.EnableRetryOnFailure()));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HaveColumnType("money");
            configurationBuilder.Properties<string>().HaveColumnType("nvarchar(200)");

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Descriptions and reviews character limit set to 1000
            modelBuilder.Entity<AttractionDbM>()
                .Property(a => a.Description).HasColumnType("nvarchar(1000)");
            modelBuilder.Entity<ReviewDbM>()
                .Property(a => a.Comment).HasColumnType("nvarchar(1000)");
            
            base.OnModelCreating(modelBuilder);
        }
    }
    #endregion

    #region DbContext MySQL and Postgres
    public class MySqlDbContext : MainDbContext
    {
        public MySqlDbContext() { }
        public MySqlDbContext(DbContextOptions options) : base(options, null) { }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = optionsBuilder.ConfigureForDesignTime(
                    (options, connectionString) =>
                        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                            b => b.SchemaBehavior(Microting.EntityFrameworkCore.MySql.Infrastructure.MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}")));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");

            base.ConfigureConventions(configurationBuilder);

        }
    }

    public class PostgresDbContext : MainDbContext
    {
        public PostgresDbContext() { }
        public PostgresDbContext(DbContextOptions options) : base(options, null){ }


        //Used only for CodeFirst Database Migration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder = optionsBuilder.ConfigureForDesignTime(
                    (options, connectionString) => options.UseNpgsql(connectionString));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveColumnType("varchar(200)");
            base.ConfigureConventions(configurationBuilder);
        }
    }
    #endregion
}
