using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Models
{
    public class Model
    {
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Artist> Artists { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<MediaCategory> MediaCategories { get; set; }
        public virtual ICollection<MediaType> MediaTypes { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }



        //public class Album
        //{
        //    public int AlbumId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Album title must be between 3 and 50 characters.")]
        //    public string Title { get; set; }

        //    [Required]
        //    public int ArtistId { get; set; }

        //    public virtual ICollection<Track> Tracks { get; set; }
        //}

        //public partial class Artist
        //{
        //    public Artist()
        //    {
        //        Albums = new HashSet<Album>();
        //    }

        //    public int ArtistId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Artist name must be between 3 and 50 characters.")]
        //    //[Remote("doesArtistExist", "Artist", HttpMethod = "POST", ErrorMessage = "Artist name already exists")]
        //    public string Name { get; set; }
        //    public virtual ICollection<Album> Albums { get; set; }
        //}

        //public partial class Genre
        //{
        //    public Genre()
        //    {
        //        Tracks = new HashSet<Track>();
        //    }

        //    public int GenreId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Genre name must be between 3 and 50 characters.")]
        //    public string Name { get; set; }

        //    public virtual ICollection<Track> Tracks { get; set; }
        //}


        //public partial class MediaCategory
        //{
        //    public MediaCategory()
        //    {
        //        MediaTypes = new HashSet<MediaType>();
        //    }

        //    public int MediaCategoryId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Media Category name must be between 3 and 50 characters.")]
        //    public string Name { get; set; }

        //    public virtual ICollection<MediaType> MediaTypes { get; set; }
        //}

        //public partial class MediaType
        //{
        //    public MediaType()
        //    {
        //        Tracks = new HashSet<Track>();
        //    }

        //    public int MediaTypeId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Media Type name must be between 3 and 50 characters.")]
        //    public string Name { get; set; }

        //    [Required]
        //    public int MediaCategoryId { get; set; }

        //    //[Required]
        //    public virtual MediaCategory MediaCategory { get; set; }

        //    public virtual ICollection<Track> Tracks { get; set; }
        //}


        //public class Track
        //{
        //    public int TrackId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Track name must be between 3 and 50 characters.")]
        //    public string Name { get; set; }

        //    [Required]
        //    public int? AlbumId { get; set; }

        //    [Required]
        //    public int MediaTypeId { get; set; }

        //    [Required]
        //    public int? GenreId { get; set; }

        //    [Required]
        //    [RegularExpression(@"^.{3,50}$", ErrorMessage = "Composer name must be between 3 and 50 characters.")]
        //    public string Composer { get; set; }

        //    [Required]
        //    [RegularExpression(@"^[0-9]{1,9}$", ErrorMessage = "Exceeds maximum length restrictions.")]
        //    public int Milliseconds { get; set; }

        //    [Required]
        //    [RegularExpression(@"^[0-9]{1,11}$", ErrorMessage = "Exceeds maximum size restrictions.")]
        //    public int? Bytes { get; set; }

        //    [Required]
        //    [RegularExpression(@"^\d+(\.\d\d)?$", ErrorMessage = "Please use numeric currency format: 0.00")]
        //    public decimal UnitPrice { get; set; }

        //    public virtual Album Album { get; set; }

        //    public virtual Genre Genre { get; set; }

        //    public virtual MediaType MediaType { get; set; }
        //}


    }
}