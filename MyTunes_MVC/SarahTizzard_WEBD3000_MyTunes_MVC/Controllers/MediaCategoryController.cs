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
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SarahTizzard_WEBD3000_MyTunes_MVC.Controllers
{
    public class MediaCategoryController : Controller
    {
        private MyTunesContext db = new MyTunesContext();

        // GET: MediaCategory
        //, string searchString,, int? page
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";

            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchString;


            var mediaCategories = from a in db.MediaCategories
                          select a;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    mediaCategories = mediaCategories.Where(a => a.Name.Contains(searchString));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    mediaCategories = mediaCategories.OrderByDescending(a => a.Name);
                    break;
                case "Name":
                    mediaCategories = mediaCategories.OrderBy(a => a.Name);
                    break;
                default:
                    mediaCategories = mediaCategories.OrderBy(a => a.Name);
                    break;
            }
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            return View(mediaCategories.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    return View(db.MediaCategories.ToList());
        //}

        // GET: MediaCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaCategory mediaCategory = db.MediaCategories.Find(id);
            if (mediaCategory == null)
            {
                return HttpNotFound();
            }
            return View(mediaCategory);
        }

        // GET: MediaCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MediaCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MediaCategoryId,Name")] MediaCategory mediaCategory)
        {
            try
            {
                bool doesItExist = db.MediaCategories.Any(anyMediaCategory => anyMediaCategory.Name.Equals(mediaCategory.Name));
                if (!doesItExist)
                {

                    if (ModelState.IsValid)
                    {
                        db.MediaCategories.Add(mediaCategory);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(mediaCategory);
                }
                else
                {
                    Response.Write("<div class='error'>Media Category already exists</div>");
                    return View();
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
           
        }

        // GET: MediaCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaCategory mediaCategory = db.MediaCategories.Find(id);
            if (mediaCategory == null)
            {
                return HttpNotFound();
            }
            return View(mediaCategory);
        }

        // POST: MediaCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "MediaCategoryId", "Name" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var mediaCategoryToUpdate = await db.MediaCategories.FindAsync(id);
            if (mediaCategoryToUpdate == null)
            {
                MediaCategory deletedMediaCategory = new MediaCategory();
                TryUpdateModel(deletedMediaCategory, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The media category was deleted by another user.");
                ViewBag.MediaCategoryId = new SelectList(db.MediaCategories, "MediaCategoryId", "Name", deletedMediaCategory.MediaCategoryId);
                return View(deletedMediaCategory);
            }

            if (TryUpdateModel(mediaCategoryToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(mediaCategoryToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (MediaCategory)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The MediaCategory was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (MediaCategory)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                        if (databaseValues.MediaCategoryId != clientValues.MediaCategoryId)
                            ModelState.AddModelError("MediaCategoryId", "Current value: "
                                + db.MediaCategories.Find(databaseValues.MediaCategoryId).Name);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                            + "was modified by another user after you got the original value. The "
                            + "edit operation was canceled and the current values in the database "
                            + "have been displayed. If you still want to edit this record, click "
                            + "the Save button again. Otherwise click the Back to List hyperlink.");
                        mediaCategoryToUpdate.RowVersion = databaseValues.RowVersion;
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(mediaCategoryToUpdate);
        }

        // GET: MediaCategory/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaCategory mediaCategory = await db.MediaCategories.FindAsync(id);
            if (mediaCategory == null)
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

            return View(mediaCategory);
        }
       
        // POST: MediaCategory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(MediaCategory mediaCategory)
        {
            try
            {
                db.Entry(mediaCategory).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = mediaCategory.MediaCategoryId });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(mediaCategory);
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
