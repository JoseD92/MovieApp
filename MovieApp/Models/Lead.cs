using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApp.Models
{
    public class Lead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required] public int MovieID { get; set; }
        [Required] public int ActorID { get; set; }

        [ForeignKey("MovieID")]
        public virtual Movie Movie { get; set; }
        [ForeignKey("ActorID")]
        public virtual Actor Actor { get; set; }
    }
}