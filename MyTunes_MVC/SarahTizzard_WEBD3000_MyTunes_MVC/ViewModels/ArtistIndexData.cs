using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SarahTizzard_WEBD3000_MyTunes_MVC.Models;
using PagedList;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.ViewModels
{
    public class ArtistIndexData
    {
        public PagedList.IPagedList<Artist> Artists { get; set; }
        public IEnumerable<Album> Albums { get; set; }
        public IEnumerable<Track> Tracks { get; set; }
    }
}