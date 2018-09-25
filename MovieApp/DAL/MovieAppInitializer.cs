using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MovieApp.Models;

namespace MovieApp.DAL
{
    public class MovieAppInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MovieAppContext>
    {
        protected override void Seed(MovieAppContext context)
        {
            var movies = new List<Movie> {
                new Movie{ID = 1,Title = "When Harry Met Sally",ReleaseDate = DateTime.Parse("1989-1-11")},
                new Movie{ID = 2,Title = "Ghostbusters ",ReleaseDate = DateTime.Parse("1984-3-13")},
                new Movie{ID = 3,Title = "Ghostbusters 2",ReleaseDate = DateTime.Parse("1986-2-23")},
                new Movie{ID = 4,Title = "Rio Bravo",ReleaseDate = DateTime.Parse("1959-4-15")}
            };
            movies.ForEach(s => context.Movies.Add(s));
            context.SaveChanges();

            var actors = new List<Actor> {
                new Actor{ID = 1,Name = "Robert De Niro "},
                new Actor{ID = 2,Name = "Jack Nicholson"},
                new Actor{ID = 3,Name = " Tom Hanks"},
                new Actor{ID = 4,Name = " Elizabeth Taylor"}
            };
            actors.ForEach(s => context.Actors.Add(s));
            context.SaveChanges();

            var categories = new List<Category> {
                new Category{ID = 1,Name = "Horror"},
                new Category{ID = 2,Name = "Comedy"},
                new Category{ID = 3,Name = "Drama"},
                new Category{ID = 4,Name = "Fantasy"}
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var haveCategories = new List<HaveCategory> {
                new HaveCategory{ID = 1,MovieID = 2,CategoryID = 1},
                new HaveCategory{ID = 2,MovieID = 2,CategoryID = 2},
                new HaveCategory{ID = 3,MovieID = 4,CategoryID = 3},
                new HaveCategory{ID = 4,MovieID = 1,CategoryID = 4}
            };
            haveCategories.ForEach(s => context.HaveCategories.Add(s));
            context.SaveChanges();

            var leads = new List<Lead> {
                new Lead{ID = 1,MovieID = 2,ActorID = 1},
                new Lead{ID = 2,MovieID = 2,ActorID = 2},
                new Lead{ID = 3,MovieID = 4,ActorID = 3},
                new Lead{ID = 4,MovieID = 1,ActorID = 4}
            };
            leads.ForEach(s => context.Leads.Add(s));
            context.SaveChanges();
        }
    }
}