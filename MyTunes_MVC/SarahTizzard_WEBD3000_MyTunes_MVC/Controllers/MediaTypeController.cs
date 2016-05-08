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
    public class MediaTypeController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: MediaType
        //string searchString,, int? page
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.IdSortParm = sortOrder == "MediaCategoryId" ? "mcId_desc" : "MediaCategoryId";

            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchString;


            var mediaTypes = from a in db.MediaTypes
                         select a;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    mediaTypes = mediaTypes.Where(a => a.Name.Contains(searchString));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    mediaTypes = mediaTypes.OrderByDescending(a => a.Name);
                    break;
                case "mcId_desc":
                    mediaTypes = mediaTypes.OrderByDescending(a => a.MediaCategoryId);
                    break;
                case "Name":
                    mediaTypes = mediaTypes.OrderBy(a => a.Name);
                    break;
                case "MediaCategoryId":
                    mediaTypes = mediaTypes.OrderBy(a => a.MediaCategoryId);
                    break;
                default:
                    mediaTypes = mediaTypes.OrderBy(a => a.Name);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(mediaTypes.ToPagedList(pageNumber, pageSize));
        }

        // GET: MediaType/Details/5
        public ActionResult Details(int? id, int? page)
        {
            MediaTypeDetailsData data = new MediaTypeDetailsData();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MediaType mediaType = db.MediaTypes.Find(id);
            data.MediaType = mediaType;

            if (mediaType == null)
            {
                return HttpNotFound();
            }

            var tracks = db.Tracks.Where(i => i.GenreId == id).OrderBy(t => t.Name);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            data.Tracks = tracks.ToPagedList(pageNumber, pageSize);
            return View(data);
        }

        // GET: MediaType/Create
        public ActionResult Create()
        {
            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name");
            return View();
        }

        // POST: MediaType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MediaTypeId,Name,MediaCategoryId")] MediaType mediaType)
        {
            try {
                bool doesItExist = db.MediaTypes.Any(anyMediaType => anyMediaType.Name.Equals(mediaType.Name));
                ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
                if (!doesItExist)
                {
                    if (ModelState.IsValid)
                    {
                        db.MediaTypes.Add(mediaType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(mediaType);

                }
                else {
                    Response.Write("<div class='error'>Media Type already exists</div>");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }

        }
        //public ActionResult Create([Bind(Include = "MediaTypeId,Name,MediaCategoryId")] MediaType mediaType)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.MediaTypes.Add(mediaType);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //        ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
        //        return View(mediaType);
        //    }

        // GET: MediaType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaType mediaType = db.MediaTypes.Find(id);
            if (mediaType == null)
            {
                return HttpNotFound();
            }
            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
            return View(mediaType);
        }

        // POST: MediaType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "MediaTypeId", "Name", "MediaCategoryId" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var mediaTypeToUpdate = await db.MediaTypes.FindAsync(id);
            if (mediaTypeToUpdate == null)
            {
                MediaType deletedMediaType = new MediaType();
                TryUpdateModel(deletedMediaType, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The mediatype was deleted by another user.");
                ViewBag.MediaTypeID = new SelectList(db.MediaTypes, "MediaTypeId", "Name", deletedMediaType.MediaTypeId);
                return View(deletedMediaType);
            }

            if (TryUpdateModel(mediaTypeToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(mediaTypeToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (MediaType)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The mediatype was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (MediaType)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.MediaCategoryId != clientValues.MediaCategoryId)
                            ModelState.AddModelError("MediaCategoryId", "Current value: "
                                + String.Format("{0:d}", databaseValues.MediaCategoryId));
                        if (databaseValues.MediaTypeId != clientValues.MediaTypeId)
                            ModelState.AddModelError("MediaTypeId", "Current value: "
                                + db.Albums.Find(databaseValues.MediaTypeId).Title);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        mediaTypeToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaTypeToUpdate.MediaCategoryId);
            return View(mediaTypeToUpdate);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "MediaTypeId,Name,MediaCategoryId")] MediaType mediaType)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(mediaType).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", mediaType.MediaCategoryId);
        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

        //    }

        //    return View(mediaType);
        //}

        // GET: MediaType/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaType mediaType = await db.MediaTypes.FindAsync(id);
            if (mediaType == null)
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

            return View(mediaType);
        }
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MediaType mediaType = db.MediaTypes.Find(id);
        //    if (mediaType == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(mediaType);
        //}

        // POST: MediaType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(MediaType mediaType)
        {
            try
            {
                db.Entry(mediaType).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = mediaType.MediaTypeId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(mediaType);
            }
        }
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    try
        //    {
        //        MediaType mediaType = db.MediaTypes.Find(id);
        //        db.MediaTypes.Remove(mediaType);
        //        db.SaveChanges();

        //    }
        //    catch
        //    {
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //    }

        //    return RedirectToAction("Index");
        //}

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
