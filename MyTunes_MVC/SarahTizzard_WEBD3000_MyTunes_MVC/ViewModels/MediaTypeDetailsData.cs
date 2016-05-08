using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SarahTizzard_WEBD3000_MyTunes_MVC.Models;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.ViewModels
{
    public class MediaTypeDetailsData
    {
        public MediaType MediaType { get; set; }
        public PagedList.IPagedList<Track> Tracks { get; set; }
    }
}