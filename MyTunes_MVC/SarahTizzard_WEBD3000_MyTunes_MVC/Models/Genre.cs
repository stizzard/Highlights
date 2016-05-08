using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{

    public partial class Genre
    {
        public Genre()
        {
            Tracks = new HashSet<Track>();
        }

        public int GenreId { get; set; }

        [Required]
        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Genre name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
