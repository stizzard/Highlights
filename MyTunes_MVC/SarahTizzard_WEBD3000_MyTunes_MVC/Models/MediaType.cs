using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{
    public partial class MediaType
    {
        public MediaType()
        {
            Tracks = new HashSet<Track>();
        }

        public int MediaTypeId { get; set; }

        [Required]
        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Media Type name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required]
        public int MediaCategoryId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //[Required]
        public virtual MediaCategory MediaCategory { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
