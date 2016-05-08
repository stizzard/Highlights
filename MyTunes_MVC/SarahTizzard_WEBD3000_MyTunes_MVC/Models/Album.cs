using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{
    public class Album
    {
        public int AlbumId { get; set; }

        [Required]
        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Album title must be between 3 and 50 characters.")]
        public string Title { get; set; }

        [Required]
        public int ArtistId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }

}
