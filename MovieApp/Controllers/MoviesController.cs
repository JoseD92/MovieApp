using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieApp.DAL;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {
        private MovieAppContext db = new MovieAppContext();

        // GET: Movies
        public ActionResult Index(string sortOrder, string searchType, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var movies = from m in db.Movies
                           select m;
            if (!String.IsNullOrEmpty(searchType) && !String.IsNullOrEmpty(searchString)) {
                int auxID;
                switch (searchType) {
                    case "title":
                        movies = movies.Where(s => s.Title.Contains(searchString));
                        break;
                    case "cat":
                        var cat = from m in db.HaveCategories
                                  select m;
                        movies = movies.Where(
                            s => cat.Where(
                                c => c.MovieID == s.ID && c.Category.Name.Contains(searchString)).Any());
                        break;
                    case "act":
                        var lead = from m in db.Leads
                                  select m;
                        movies = movies.Where(
                            s => lead.Where(
                                l => l.MovieID == s.ID && l.Actor.Name.Contains(searchString)).Any());
                        break;
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.ReleaseDate);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.ReleaseDate);
                    break;
                default:
                    movies = movies.OrderBy(s => s.Title);
                    break;
            }
            return View(movies.ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.NotMovieCat = db.Database.SqlQuery<Category>(
                "SELECT * FROM Category WHERE NOT EXISTS " +
                "(SELECT * FROM HaveCategory WHERE " +
                "Category.ID = CategoryID AND MovieID = {0})",
                movie.ID);
            ViewBag.NotMovieLead = db.Database.SqlQuery<Actor>(
                "SELECT * FROM Actor WHERE NOT EXISTS " +
                "(SELECT * FROM Lead WHERE " +
                "Actor.ID = ActorID AND MovieID = {0})",
                movie.ID);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        [HttpPost]
        public ActionResult EditCat(int ID,string action, int target)
        {
            if (action== "DEL")
            {
                db.Database.ExecuteSqlCommand(
                    "DELETE FROM HaveCategory WHERE MovieID={0} AND CategoryID={1}",
                    ID,
                    target);
            }
            if (action == "ADD")
            {
                System.Diagnostics.Debug.WriteLine("ADD "+ID.ToString()+" "+target.ToString());
                db.Database.ExecuteSqlCommand(
                    "INSERT INTO HaveCategory (MovieID,CategoryID) VALUES ({0},{1})",
                    ID,
                    target);
            }
            return RedirectToAction("Edit/"+ID.ToString());
        }

        [HttpPost]
        public ActionResult EditAct(int ID, string action, int target)
        {
            if (action == "DEL")
            {
                db.Database.ExecuteSqlCommand(
                    "DELETE FROM Lead WHERE MovieID={0} AND ActorID={1}",
                    ID, 
                    target);
            }
            if (action == "ADD")
            {
                db.Database.ExecuteSqlCommand(
                    "INSERT INTO Lead (MovieID,ActorID) VALUES ({0},{1})",
                    ID,
                    target);
            }
            return RedirectToAction("Edit/" + ID.ToString());
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
