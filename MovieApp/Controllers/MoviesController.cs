using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MovieApp.DAL;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepo;

        public MoviesController(IMovieRepository movieRepo) {
            _movieRepo = movieRepo;

        }

        // GET: Movies
        public ActionResult Index(string sortOrder, string searchType, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            List<Movie> movies = new List<Movie>();
            if (!String.IsNullOrEmpty(searchType) && !String.IsNullOrEmpty(searchString))
            {
                switch (searchType)
                {
                    case "title":
                        movies = _movieRepo.GetMoviesWithTitle(searchString);
                        break;
                    case "cat":
                        movies = _movieRepo.GetMoviesWithCategory(searchString);
                        break;
                    case "act":
                        movies = _movieRepo.GetMoviesWithActor(searchString);
                        break;
                }
            }
            else movies = _movieRepo.GetAllMovies();
            switch (sortOrder)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(s => s.Title).ToList();
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.ReleaseDate).ToList();
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.ReleaseDate).ToList();
                    break;
                default:
                    movies = movies.OrderBy(s => s.Title).ToList();
                    break;
            }
            return View(movies);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = _movieRepo.GetByID((int)id);
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
                _movieRepo.AddMovie(movie);
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
            Movie movie = _movieRepo.GetByID((int)id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.NotMovieCat = _movieRepo.GetCategoriesMovieDontHave(movie);
            ViewBag.NotMovieLead = _movieRepo.GetActorsMovieDontHave(movie);
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
                _movieRepo.EditMovie(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        [HttpPost]
        public ActionResult EditCat(int ID,string action, int target)
        {
            if (action== "DEL")
            {
                _movieRepo.MovieDelCategory(ID, target);
            }
            if (action == "ADD")
            {
                //System.Diagnostics.Debug.WriteLine("ADD "+ID.ToString()+" "+target.ToString());
                _movieRepo.MovieAddCategory(ID, target);
            }
            return RedirectToAction("Edit/"+ID.ToString());
        }

        [HttpPost]
        public ActionResult EditAct(int ID, string action, int target)
        {
            if (action == "DEL")
            {
                _movieRepo.MovieDelActor(ID, target);
            }
            if (action == "ADD")
            {
                _movieRepo.MovieAddActor(ID, target);
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
            Movie movie = _movieRepo.GetByID((int)id);
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
            Movie movie = _movieRepo.GetByID((int)id);
            _movieRepo.DelMovie(movie);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _movieRepo.DisposeDB();
            }
            base.Dispose(disposing);
        }
    }
}
