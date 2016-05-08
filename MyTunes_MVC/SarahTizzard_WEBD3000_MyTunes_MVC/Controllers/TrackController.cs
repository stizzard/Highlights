using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SarahTizzard_WEBD3000_MyTunes_MVC.DAL;
using SarahTizzard_WEBD3000_MyTunes_MVC.Models;
using PagedList;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Controllers
{
    public class TrackController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Track
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            
            ViewBag.AlbumSortParm = sortOrder == "Album" ? "album_desc" : "Album";
            ViewBag.GenreSortParm = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.MediaSortParm = sortOrder == "Media" ? "media_desc" : "Media";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.ComposerSortParm = sortOrder == "Composer" ? "composer_desc" : "Composer";
            ViewBag.TimeSortParm = sortOrder == "Milliseconds" ? "time_desc" : "Milliseconds";
            ViewBag.SizeSortParm = sortOrder == "Bytes" ? "size_desc" : "Bytes";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var tracks = from t in db.Tracks
                         select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                tracks = tracks.Where(t => t.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "album_desc":
                    tracks = tracks.OrderByDescending(t => t.Album.Title);
                    break;
                case "genre_desc":
                    tracks = tracks.OrderByDescending(t => t.Genre.Name);
                    break;
                case "media_desc":
                    tracks = tracks.OrderByDescending(t => t.MediaType.Name);
                    break;
                case "name_desc":
                    tracks = tracks.OrderByDescending(t => t.Name);
                    break;
                case "composer_desc":
                    tracks = tracks.OrderByDescending(t => t.Composer);
                    break;
                case "time_desc":
                    tracks = tracks.OrderByDescending(t => t.Milliseconds);
                    break;
                case "size_desc":
                    tracks = tracks.OrderByDescending(t => t.Bytes);
                    break;
                case "price_desc":
                    tracks = tracks.OrderByDescending(t => t.UnitPrice);
                    break;
                case "Album":
                    tracks = tracks.OrderBy(t => t.Album.Title);
                    break;
                case "Genre":
                    tracks = tracks.OrderBy(t => t.Genre.Name);
                    break;
                case "Media":
                    tracks = tracks.OrderBy(t => t.MediaType.Name);
                    break;
                case "Name":
                    tracks = tracks.OrderBy(t => t.Name);
                    break;
                case "Composer":
                    tracks = tracks.OrderBy(t => t.Composer);
                    break;
                case "Milliseconds":
                    tracks = tracks.OrderBy(t => t.Milliseconds);
                    break;
                case "Bytes":
                    tracks = tracks.OrderBy(t => t.Bytes);
                    break;
                case "Price":
                    tracks = tracks.OrderBy(t => t.UnitPrice);
                    break;
                default:
                    tracks = tracks.OrderBy(t => t.Genre.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tracks.ToPagedList(pageNumber, pageSize));
            //return View(tracks.ToList());
        }


        // GET: Track/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // GET: Track/Create
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name");
            return View();
        }

        // POST: Track/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrackId,Name,AlbumId,MediaTypeId,GenreId,Composer,Milliseconds,Bytes,UnitPrice")] Track track)
        {
            try {
                if (ModelState.IsValid)
                {
                    db.Tracks.Add(track);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException  /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }

            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", track.AlbumId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", track.GenreId);
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", track.MediaTypeId);
            return View(track);
        }

        // GET: Track/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = db.Tracks.Find(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", track.AlbumId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", track.GenreId);
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", track.MediaTypeId);
            return View(track);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "TrackId","Name", "AlbumId", "MediaTypeId", "GenreId", "Composer", "Milliseconds", "Bytes", "UnitPrice", "RowVersion" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var trackToUpdate = await db.Tracks.FindAsync(id);
            if (trackToUpdate == null)
            {
                Track deletedTrack = new Track();
                TryUpdateModel(deletedTrack, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The track was deleted by another user.");
                ViewBag.TrackId = new SelectList(db.Tracks, "TrackId", "Name", deletedTrack.TrackId);
                return View(deletedTrack);
            }

            if (TryUpdateModel(trackToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(trackToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Track)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The track was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Track)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.AlbumId != clientValues.AlbumId)
                            ModelState.AddModelError("AlbumId", "Current value: "
                                + String.Format("{0:c}", databaseValues.AlbumId));
                        if (databaseValues.MediaTypeId != clientValues.MediaTypeId)
                            ModelState.AddModelError("MediaTypeId", "Current value: "
                                + String.Format("{0:d}", databaseValues.MediaTypeId));

                        if (databaseValues.GenreId != clientValues.GenreId)
                            ModelState.AddModelError("GenreId", "Current value: "
                                + String.Format("{0:d}", databaseValues.GenreId));
                        if (databaseValues.Composer != clientValues.Composer)
                            ModelState.AddModelError("Composer", "Current value: "
                                + String.Format("{0:d}", databaseValues.Composer));
                        if (databaseValues.Milliseconds != clientValues.Milliseconds)
                            ModelState.AddModelError("Milliseconds", "Current value: "
                                + String.Format("{0:d}", databaseValues.Milliseconds));
                        if (databaseValues.Bytes != clientValues.Bytes)
                            ModelState.AddModelError("Bytes", "Current value: "
                                + String.Format("{0:d}", databaseValues.Bytes));
                        if (databaseValues.UnitPrice != clientValues.UnitPrice)
                            ModelState.AddModelError("UnitPrice", "Current value: "
                                + String.Format("{0:d}", databaseValues.UnitPrice));

                        if (databaseValues.TrackId != clientValues.TrackId)
                            ModelState.AddModelError("TrackId", "Current value: "
                                + db.Tracks.Find(databaseValues.TrackId).Name);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        trackToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            ViewBag.AlbumId = new SelectList(db.Albums, "AlbumId", "Title", trackToUpdate.AlbumId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", trackToUpdate.GenreId);
            ViewBag.MediaTypeId = new SelectList(db.MediaTypes, "MediaTypeId", "Name", trackToUpdate.MediaTypeId);
            
            return View(trackToUpdate);
        }


        // GET: Track/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(track);
        }
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Track track = db.Tracks.Find(id);
        //    if (track == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(track);
        //}

        // POST: Track/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Track track)
        {
            try
            {
                db.Entry(track).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = track.TrackId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(track);
            }
        }
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        Track track = db.Tracks.Find(id);
        //        db.Tracks.Remove(track);
        //        db.SaveChanges();
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //    }

        //    return RedirectToAction("Index");
        //}

        public ActionResult DBStats()
        {

            List<string> genresList = new List<string>();
            List<string> mediaTypesList = new List<string>();
            List<string> mediaCategoriesList = new List<string>();

            string connectionString = @"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\MyTunes.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework";
            int totalTracks = db.Tracks.Count();
            int totalArtists = db.Artists.Count();
            int totalAlbums = db.Albums.Count();

            string genres = "SELECT Name, GenreId FROM Genre";
            string mediaTypes = "SELECT Name, MediaTypeId FROM MediaType";
            string mediaCategories = "SELECT Name, MediaCategoryId FROM MediaCategory";
            //for each Genre total Tracks
            //for each MediaType total tracks
            //for each Media Category total tracks

            string largestTrack = "SELECT TOP 1 Name, Bytes FROM Track ORDER BY Bytes DESC;";
            string longestTrack = "SELECT TOP 1 Name, Milliseconds FROM Track ORDER BY Milliseconds DESC;";
            
            //artist with most tracks...

            using (SqlConnection connection = new SqlConnection(connectionString)) {
                SqlCommand largestTrackCommand = new SqlCommand(largestTrack, connection);
                SqlCommand longestTrackCommand = new SqlCommand(longestTrack, connection);
                SqlCommand genresCommand = new SqlCommand(genres, connection);
                SqlCommand mediaTypesCommand = new SqlCommand(mediaTypes, connection);
                SqlCommand mediaCategoriesCommand = new SqlCommand(mediaCategories, connection);
                connection.Open();
                SqlDataReader largestTrackReader = largestTrackCommand.ExecuteReader();
                SqlDataReader longestTrackReader = longestTrackCommand.ExecuteReader();
                SqlDataReader genresReader = genresCommand.ExecuteReader();
                SqlDataReader mediaTypesReader = mediaTypesCommand.ExecuteReader();
                SqlDataReader mediaCategoriesReader = mediaCategoriesCommand.ExecuteReader();

                foreach (var i in genresReader)
                {
                    string numTracks = "SELECT COUNT(TrackId) FROM Track INNER JOIN Genre ON Track.GenreId = Genre.GenreId WHERE Genre.GenreId = @GenreId;";

                    System.Diagnostics.Debug.WriteLine(numTracks);
                    System.Diagnostics.Debug.WriteLine(genresReader.GetValue(1));

                    SqlCommand numTracksCommand = new SqlCommand(numTracks, connection);

                    numTracksCommand.Parameters.Add("@GenreId", System.Data.SqlDbType.Int);
                    numTracksCommand.Parameters["@GenreId"].Value = genresReader.GetValue(1);

                    SqlDataReader numTracksReader = numTracksCommand.ExecuteReader();
                    numTracksReader.Read();

                    System.Diagnostics.Debug.WriteLine(numTracksReader.GetValue(0));

                    genresList.Add(String.Format("{0}: {1} tracks", genresReader[0], numTracksReader[0]));

                    ViewBag.Genres = genresList;
                }

                foreach (var i in mediaTypesReader)
                {
                    string numTracks = "SELECT COUNT(TrackId) FROM Track INNER JOIN MediaType ON Track.MediaTypeId = MediaType.MediaTypeId WHERE MediaType.MediaTypeId = @MediaTypeId;";

                    SqlCommand numTracksCommand = new SqlCommand(numTracks, connection);

                    numTracksCommand.Parameters.Add("@MediaTypeId", System.Data.SqlDbType.Int);
                    numTracksCommand.Parameters["@MediaTypeId"].Value = mediaTypesReader.GetValue(1);

                    SqlDataReader numTracksReader = numTracksCommand.ExecuteReader();
                    numTracksReader.Read();

                    mediaTypesList.Add(String.Format("{0}: {1} tracks", mediaTypesReader[0], numTracksReader[0]));

                    ViewBag.MediaTypes = mediaTypesList;
                }

                foreach (var i in mediaCategoriesReader)
                {
                    string numTracks = "SELECT COUNT(TrackId) FROM Track INNER JOIN MediaType ON Track.MediaTypeId = MediaType.MediaTypeId INNER JOIN MediaCategory ON MediaType.MediaCategoryId = MediaCategory.MediaCategoryId WHERE MediaCategory.MediaCategoryId = @MediaCategoryId;";

                    SqlCommand numTracksCommand = new SqlCommand(numTracks, connection);

                    numTracksCommand.Parameters.Add("@MediaCategoryId", System.Data.SqlDbType.Int);
                    numTracksCommand.Parameters["@MediaCategoryId"].Value = mediaCategoriesReader.GetValue(1);

                    SqlDataReader numTracksReader = numTracksCommand.ExecuteReader();
                    numTracksReader.Read();

                    mediaCategoriesList.Add(String.Format("{0}: {1} tracks", mediaCategoriesReader[0], numTracksReader[0]));

                    ViewBag.MediaCategories = mediaCategoriesList;
                }

                try
                {
                    while (largestTrackReader.Read())
                    {
                        ViewBag.LargestTrack = String.Format("Name: {0}, Bytes: {1}", largestTrackReader[0], largestTrackReader[1]);
                    }

                    while (longestTrackReader.Read())
                    {
                        ViewBag.LongestTrack = String.Format("Name: {0}, Milliseconds: {1}", longestTrackReader[0], longestTrackReader[1]);
                    }
                }
                finally
                {
                    largestTrackReader.Close();
                    longestTrackReader.Close();
                }

            };

            
            ViewBag.TotalTracks = totalTracks;
            ViewBag.TotalArtists = totalArtists;
            ViewBag.TotalAlbums = totalAlbums;

            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
