using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieApp.DAL;
using System.Data.Entity.Infrastructure;

namespace MovieApp.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<HaveCategory> HaveCategories { get; set; }
        public virtual ICollection<Lead> Leads { get; set; }

        public DbRawSqlQuery<Category> NotMovieCat() {
            MovieAppContext db = new MovieAppContext();
            return db.Database.SqlQuery<Category>(
                "SELECT * FROM Category WHERE NOT EXISTS " +
                "(SELECT * FROM HaveCategory WHERE " +
                "Category.ID = CategoryID AND MovieID = {0})",
                ID);
        }
        public DbRawSqlQuery<Actor> NotMovieLead()
        {
            MovieAppContext db = new MovieAppContext();
            return db.Database.SqlQuery<Actor>(
                "SELECT * FROM Actor WHERE NOT EXISTS " +
                "(SELECT * FROM Lead WHERE " +
                "Actor.ID = ActorID AND MovieID = {0})",
                ID);
        }
    }
}