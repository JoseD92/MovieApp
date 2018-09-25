using MovieApp.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MovieApp.DAL
{
    public class MovieAppContext : DbContext
    {

        public MovieAppContext() : base("MovieAppContext")
        {
        }

        public DbSet<HaveCategory> HaveCategories { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}