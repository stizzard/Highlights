using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using SarahTizzard_WEBD3000_MyTunes_MVC.DAL;
using SarahTizzard_WEBD3000_MyTunes_MVC.Models;
using PagedList;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Controllers
{
    public class ModelController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        public ActionResult Index()
        { 
            int totalTracks = db.Tracks.Count();
            int totalArtists = db.Artists.Count();
            int totalAlbums = db.Albums.Count();

            //for each genre totalTracks
            //for each MediaType total tracks
            //for each Media Category total tracks

            string largestTrack = "SELECT * FROM db.Tracks ORDER BY Bytes DESC LIMIT 1;";
            string longestTrack = "SELECT * FROM db.Tracks ORDER BY Milliseconds DESC LIMIT 1;";


            return View();

        }
    }


}