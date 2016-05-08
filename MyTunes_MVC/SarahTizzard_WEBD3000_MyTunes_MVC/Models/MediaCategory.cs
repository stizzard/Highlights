using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{
    
    public partial class MediaCategory
    {
        public MediaCategory()
        {
            MediaTypes = new HashSet<MediaType>();
        }

        public int MediaCategoryId { get; set; }

        [Required]
        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Media Category name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual ICollection<MediaType> MediaTypes { get; set; }
    }
}
