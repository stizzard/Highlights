using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
        }

        public int ArtistId { get; set; }

        [Required]
        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Artist name must be between 3 and 50 characters.")]
        //[Remote("doesArtistExist", "Artist", HttpMethod = "POST", ErrorMessage = "Artist name already exists")]
        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

       // public virtual ICollection<Artist> Artists { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
       // public virtual ICollection<Track> Tracks { get; set; }
    }
}
