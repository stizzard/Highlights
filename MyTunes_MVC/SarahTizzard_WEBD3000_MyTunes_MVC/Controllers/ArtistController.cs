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
using SarahTizzard_WEBD3000_MyTunes_MVC.ViewModels;
using PagedList;
using System.Web.Security;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Controllers
{
    public class ArtistController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Artist
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page, int? id, int? albumId, int? artistPage, int? albumPage, int? trackPage)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            int artistPageNumber = (artistPage ?? 1);
            int albumPageNumber = (albumPage ?? 1);
            int trackPageNumber = (trackPage ?? 1);

            var viewModel = new ArtistIndexData();
            viewModel.Artists = db.Artists
                .Include(a => a.Albums)
               // .Include(a => a.Tracks.Select(t => t.Name))
                .OrderBy(a => a.Name).ToPagedList(artistPageNumber, 5);

            if (id != null)
            {
                ViewBag.ArtistId = id.Value;
                viewModel.Albums = viewModel.Artists.Where(
                    i => i.ArtistId == id.Value).Single().Albums.ToList();
            }

            if (albumId != null)
            {
                ViewBag.AlbumId = albumId.Value;
                viewModel.Tracks = viewModel.Albums.Where(
                    x => x.AlbumId == albumId).Single().Tracks.ToList();
            }

           
            return View(viewModel);


            //ViewBag.IdSortParm = sortOrder == "ArtistId" ? "artistId_desc" : "ArtistId";

            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            ////ViewBag.CurrentFilter = searchString;

            ////var artists = from a in db.Artists
            ////             select a;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    viewModel.Artists = viewModel.Artists.Where(a => a.Name.Contains(searchString)).ToPagedList(artistPageNumber, 5);
            //}
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        viewModel.Artists = viewModel.Artists.OrderByDescending(a => a.Name).ToPagedList(artistPageNumber, 5);
            //        break;
            //    case "Name":
            //        viewModel.Artists = viewModel.Artists.OrderBy(a => a.Name).ToPagedList(artistPageNumber, 5);
            //        break;
            //    //case "artistId_desc":
            //    //    artists = artists.OrderByDescending(a => a.ArtistId);
            //    //    break;
            //    //case "ArtistId":
            //    //    artists = artists.OrderBy(a => a.ArtistId);
            //    //    break;
            //    default:
            //        viewModel.Artists = artists.OrderBy(a => a.Name).ToPagedList(artistPageNumber, 5);
            //        break;
            //}

            //int pageSize = 25;
            //int pageNumber = (page ?? 1);
            //return View(artists.ToPagedList(pageNumber, pageSize));
            //return View(tracks.ToList());
        }
       

        // GET: Artist/Details/5
        public ActionResult Details(int? id, int? page)
        {
            ArtistDetailsData data = new ArtistDetailsData();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Artist artist = db.Artists.Find(id);
            data.Artist = artist;

            if (artist == null)
            {
                return HttpNotFound();
            }


            var albums = db.Albums.Where(i => i.ArtistId == id).OrderBy(a => a.Title);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            data.Albums = albums.ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistId,Name")] Artist artist)
        {
            try
            {
                bool doesItExist = db.Artists.Any(anyArtist => anyArtist.Name.Equals(artist.Name));
                if (!doesItExist)
                {

                    if (ModelState.IsValid)
                    {
                        db.Artists.Add(artist);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(artist);
                }
                else
                {
                    Response.Write("<div class='error'>Artist already exists</div>");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
            
        }

        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "ArtistId", "Name" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var artistToUpdate = await db.Artists.FindAsync(id);
            if (artistToUpdate == null)
            {
                Artist deletedArtist = new Artist();
                TryUpdateModel(deletedArtist, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The artist was deleted by another user.");
                ViewBag.AlbumID = new SelectList(db.Albums, "AlbumId", "Title", deletedArtist.ArtistId);
                return View(deletedArtist);
            }

            if (TryUpdateModel(artistToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(artistToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Artist)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The artist was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Artist)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);                        
                        if (databaseValues.ArtistId != clientValues.ArtistId)
                            ModelState.AddModelError("ArtistId", "Current value: "
                                + db.Albums.Find(databaseValues.ArtistId).Title);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        artistToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(artistToUpdate);
        }


        // GET: Artist/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = await db.Artists.FindAsync(id);
            if (artist == null)
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

            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Artist artist)
        {
            try
            {
                db.Entry(artist).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = artist.ArtistId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(artist);
            }
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
