using MovieApp.Models;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MovieApp.DAL
{
    public interface IMovieRepository
    {
        List<Movie> GetAllMovies();
        Movie GetByID(int id);
        List<Movie> GetMoviesWithTitle(string searchString);
        List<Movie> GetMoviesWithCategory(string searchString);
        List<Movie> GetMoviesWithActor(string searchString);
        void AddMovie(Movie m);
        void DelMovie(Movie m);
        void EditMovie(Movie m);
        List<Category> GetCategoriesMovieDontHave(Movie movie);
        List<Actor> GetActorsMovieDontHave(Movie movie);
        void MovieAddCategory(int movieID, int catID);
        void MovieDelCategory(int movieID, int catID);
        void MovieAddActor(int movieID, int actorID);
        void MovieDelActor(int movieID, int actorID);
        void DisposeDB();
    }

    public class MovieRepository : IMovieRepository
    {
        private MovieAppContext db = new MovieAppContext();
        public List<Movie> GetAllMovies()
        {
            var movies = from m in db.Movies
                         select m;
            return movies.ToList();
        }
        public Movie GetByID(int id)
        {
            var movies = from m in db.Movies
                         select m;
            movies = movies.Where(m => m.ID == id);
            if (movies.Any()) return movies.First();
            else return null;
        }
        public List<Movie> GetMoviesWithTitle(string searchString) {
            var movies = from m in db.Movies
                         select m;
            return movies.Where(s => s.Title.Contains(searchString)).ToList();
        }
        public List<Movie> GetMoviesWithCategory(string searchString)
        {
            var movies = from m in db.Movies
                         select m;
            var cat = from m in db.HaveCategories
                      select m;
            movies = movies.Where(
                s => cat.Where(
                    c => c.MovieID == s.ID && c.Category.Name.Contains(searchString)).Any());
            return movies.ToList();
        }
        public List<Movie> GetMoviesWithActor(string searchString)
        {
            var movies = from m in db.Movies
                         select m;
            var lead = from m in db.Leads
                       select m;
            movies = movies.Where(
                s => lead.Where(
                    l => l.MovieID == s.ID && l.Actor.Name.Contains(searchString)).Any());
            return movies.ToList();
        }
        public void AddMovie(Movie m) {
            db.Movies.Add(m);
            db.SaveChanges();
        }
        public void DelMovie(Movie m) {
            db.Movies.Remove(m);
            db.SaveChanges();
        }
        public void EditMovie(Movie m) {
            db.Entry(m).State = EntityState.Modified;
            db.SaveChanges();
        }
        public List<Category> GetCategoriesMovieDontHave(Movie movie) {
            var temp = db.Database.SqlQuery<Category>(
                "SELECT * FROM Category WHERE NOT EXISTS " +
                "(SELECT * FROM HaveCategory WHERE " +
                "Category.ID = CategoryID AND MovieID = {0})",
                movie.ID);
            return temp.ToList();
        }
        public List<Actor> GetActorsMovieDontHave(Movie movie) {
            var temp = db.Database.SqlQuery<Actor>(
                "SELECT * FROM Actor WHERE NOT EXISTS " +
                "(SELECT * FROM Lead WHERE " +
                "Actor.ID = ActorID AND MovieID = {0})",
                movie.ID);
            return temp.ToList();
        }
        public void MovieDelCategory(int movieID, int catID) {
            db.Database.ExecuteSqlCommand(
                "DELETE FROM HaveCategory WHERE MovieID={0} AND CategoryID={1}",
                movieID,
                catID);
        }
        public void MovieAddCategory(int movieID, int catID) {
            db.Database.ExecuteSqlCommand(
                "INSERT INTO HaveCategory (MovieID,CategoryID) VALUES ({0},{1})",
                movieID,
                catID);
        }
        public void MovieDelActor(int movieID, int actorID) {
            db.Database.ExecuteSqlCommand(
                "DELETE FROM Lead WHERE MovieID={0} AND ActorID={1}",
                movieID,
                actorID);
        }
        public void MovieAddActor(int movieID, int actorID) {
            db.Database.ExecuteSqlCommand(
                "INSERT INTO Lead (MovieID,ActorID) VALUES ({0},{1})",
                movieID,
                actorID);
        }
        public void DisposeDB() {
            db.Dispose();
        }
    }
}