using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InsuraTech.DATA.EF;
using Microsoft.AspNet.Identity;
using InsuraTech.Models;
using PagedList;


namespace InsuraTech.Controllers
{
    [Authorize]
    public class OpenPositionsController : Controller
    {
        private InsuraTechEntities db = new InsuraTechEntities();

        //Apply Button/Function
        public ActionResult Apply(int id)
        {
            Application app = new Application();
            app.UserId = User.Identity.GetUserId();
            app.OpenPositionId = id;
            app.ApplicationDate = DateTime.Now;
            app.ManagerNotes = " ";
            app.AplicationStatus = 3;
            string userId = User.Identity.GetUserId();
            UserDetail ud = db.UserDetails.Where(x => x.UserId == userId).SingleOrDefault();
            if (ud.ResumeFileName == null)
            {
                Session["ErrorMessage"] = "PLEASE ADD A RESUME TO YOUR PROFILE TO APPLY";
                return RedirectToAction("Index");
            }//end if
            Session["ErrorMessage"] = null;
            app.ResumeFileName = ud.ResumeFileName;
            db.Applications.Add(app);
            db.SaveChanges();

            return RedirectToAction("Index", "OpenPositions");

        }//end result

        // GET: OpenPositions
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Manager"))
            {
                string userId = User.Identity.GetUserId();

                var openPositions = db.OpenPositions.Where(man => man.Location.ManagerId == userId).Include(o => o.Location).Include(o => o.Position);

                return View(openPositions.ToList());
            }
            else
            {
                var openPositions = db.OpenPositions.Include(o => o.Location).Include(o => o.Position);

                return View(openPositions.ToList());

            }



        }

        // GET: OpenPositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }
            return View(openPosition);
        }

        // GET: OpenPositions/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber");
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title");
            return View();
        }

        // POST: OpenPositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OpenPositionId,PositionId,LocationId")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                db.OpenPositions.Add(openPosition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: OpenPositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // POST: OpenPositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OpenPositionId,PositionId,LocationId")] OpenPosition openPosition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openPosition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "StoreNumber", openPosition.LocationId);
            ViewBag.PositionId = new SelectList(db.Positions, "PositionId", "Title", openPosition.PositionId);
            return View(openPosition);
        }

        // GET: OpenPositions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenPosition openPosition = db.OpenPositions.Find(id);
            if (openPosition == null)
            {
                return HttpNotFound();
            }
            return View(openPosition);
        }

        // POST: OpenPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OpenPosition openPosition = db.OpenPositions.Find(id);
            db.OpenPositions.Remove(openPosition);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private InsuraTechEntities ctx = new InsuraTechEntities();

        //public ActionResult ClientSide()
        //{
        //    var openPositions = db.OpenPositions.Include(o => o.Location)
        //                                        .Include(o => o.Position);

        //    return View(openPositions.ToList());

        //}//end action

        //public ActionResult OpenPositionsQS(string searchFilter)
        //{
        //    //2 Options
        //    //-Search has NOT been used (initial pg demand or subsequent demands)
        //    //-Search HAS been used and return filtered results

        //    //get a list of products
        //    var openPositions = ctx.OpenPositions;


        //    //branch - No filter
        //    if (string.IsNullOrEmpty(searchFilter))
        //    {
        //        //return all results
        //        return View(openPositions.ToList());

        //    }//end if



        //    else
        //    {


        //        //keyword syntax
        //        var filteredOpenPositions = (from p in openPositions
        //                                where p.Position.Title.Contains(searchFilter.ToLower())
        //                                select p).ToList();







        //        return View(filteredOpenPositions);
        //    }//end else







        //public ActionResult OpenPositionsMVCPaging(string searchString, int page = 1)
        //{
        //    int pageSize = 5;

        //    var openPositions = ctx.OpenPositions.OrderBy(p => p.Location).ToList();

        //    #region Search Logic

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        openPositions = openPositions.Where(p => p.Position.Title.ToLower().Contains(searchString.ToLower())).ToList();
        //    }//end if

        //    ViewBag.SearchString = searchString;

        //    #endregion

        //    return View(openPositions.ToPagedList(page, pageSize));


        //}//end action result



    }
}
