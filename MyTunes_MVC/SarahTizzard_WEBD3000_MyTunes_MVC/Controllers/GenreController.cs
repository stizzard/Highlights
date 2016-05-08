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
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Controllers
{
    public class GenreController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: Genre
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var genre = from g in db.Genres
                         select g;

            if (!String.IsNullOrEmpty(searchString))
            {
                genre = genre.Where(g => g.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    genre = genre.OrderByDescending(g => g.Name);
                    break;                
                case "Name":
                    genre = genre.OrderBy(g => g.Name);
                    break;
                default:
                    genre = genre.OrderBy(g => g.Name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(genre.ToPagedList(pageNumber, pageSize));
        }


        // GET: Genre/Details/5
        public ActionResult Details(int? id, int? page)
        {
            GenreDetailsData data = new GenreDetailsData();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);
            data.Genre = genre;

            if (genre == null)
            {
                return HttpNotFound();
            }


            var tracks = db.Tracks.Where(i => i.GenreId == id).OrderBy(t => t.Name);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            data.Tracks = tracks.ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        // GET: Genre/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenreId,Name")] Genre genre)
        {
            try {
                bool doesItExist = db.Genres.Any(anyGenre => anyGenre.Name.Equals(genre.Name));
                if (!doesItExist)
                {

                    if (ModelState.IsValid)
                    {
                        db.Genres.Add(genre);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(genre);
                }
                else
                {
                    Response.Write("<div class='error'>Genre already exists</div>");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
            
        }

        // GET: Genre/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = db.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Genre/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "GenreId", "Name" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var genreToUpdate = await db.Genres.FindAsync(id);
            if (genreToUpdate == null)
            {
                Genre deletedGenre = new Genre();
                TryUpdateModel(deletedGenre, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The genre was deleted by another user.");
                ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", deletedGenre.GenreId);
                return View(deletedGenre);
            }

            if (TryUpdateModel(genreToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(genreToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Genre)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The Genre was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Genre)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);                        
                        if (databaseValues.GenreId != clientValues.GenreId)
                            ModelState.AddModelError("GenreId", "Current value: "
                                + db.Albums.Find(databaseValues.GenreId).Title);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        genreToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(genreToUpdate);
        }


        // GET: Genre/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = await db.Genres.FindAsync(id);
            if (genre == null)
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

            return View(genre);
        }
       

        // POST: Genre/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Genre genre)
        {
            try
            {
                db.Entry(genre).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = genre.GenreId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(genre);
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
