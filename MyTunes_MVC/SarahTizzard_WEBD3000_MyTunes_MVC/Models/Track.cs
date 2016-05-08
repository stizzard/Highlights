using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{
    public class Track
    {
        public int TrackId { get; set; }

        [Required]
        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Track name must be between 3 and 50 characters.")]
        public string Name { get; set; }

        [Required]
        public int? AlbumId { get; set; }

        [Required]
        public int MediaTypeId { get; set; }

        [Required]
        public int? GenreId { get; set; }

        [RegularExpression(@"^.{3,50}$", ErrorMessage = "Composer name must be between 3 and 50 characters.")]
        public string Composer { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{1,9}$", ErrorMessage = "Exceeds maximum length restrictions.")]
        public int Milliseconds { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{1,11}$", ErrorMessage = "Exceeds maximum size restrictions.")]
        public int? Bytes { get; set; }

        [Required]
        [RegularExpression(@"^\d+(\.\d\d)?$", ErrorMessage = "Please use numeric currency format: 0.00")]
        public decimal UnitPrice { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual Album Album { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual MediaType MediaType { get; set; }
    }
}