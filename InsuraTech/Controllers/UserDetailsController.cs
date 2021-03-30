using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InsuraTech.DATA.EF;
using Microsoft.AspNet.Identity;

namespace InsuraTech.Controllers
{
    [Authorize]
    public class UserDetailsController : Controller
    {
        private InsuraTechEntities db = new InsuraTechEntities();

        // GET: UserDetails
        public ActionResult Index()
        {
            if (Request.IsAuthenticated && User.IsInRole("Employee"))
            {
                string userId = User.Identity.GetUserId();

                var empDetails = db.UserDetails.Where(emp => emp.UserId == userId);


                return View(empDetails.ToList());
            }
            return View(db.UserDetails.ToList());
        }

        // GET: UserDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,ResumeFileName")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                db.UserDetails.Add(userDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetail);
        }

        // GET: UserDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,ResumeFileName")] UserDetail userDetail, HttpPostedFileBase resume)
        {
            if (ModelState.IsValid)
            {
                

                //newUserDeets.ResumeFileName = model.ResumeFileName; //To-Do --Handle file upload
                #region File Upload
                if (resume != null)
                {
                    //get pdf and assign to a variable
                    string fileName = resume.FileName;

                    //declare and assign ext value
                    string ext = fileName.Substring(fileName.LastIndexOf("."));

                    //declare a list of valid extensions
                    string[] goodExts = { ".pdf", ".docx", ".rtf", ".rtx" };

                    //check the ext variable (tolower()) against a valid list
                    if (goodExts.Contains(ext.ToLower()) && (resume.ContentLength <= 4194304))//4194304 is the max by ASP.net (4MB)
                    {
                        //if its in the list rename using a guid
                        fileName = Guid.NewGuid() + ext;


                        //save to the webserver
                        resume.SaveAs(Server.MapPath("~/Content/img/resume/" + fileName));

                        //Make sure you are not deleting your default ---------Ask Jeff about this


                        //only save if the file meets criteria imageName to the object
                        userDetail.ResumeFileName = fileName;

                    }//end if



                }//end if

                //If the file is bad (not in our list or NO file was included) the HiddenFor() in the view will care for retaining the value.

                #endregion

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                


                return RedirectToAction("Index", "UserDetails");
            }
            // If we got this far, something failed, redisplay form
            return View(userDetail);
        }





        // GET: UserDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetail userDetail = db.UserDetails.Find(id);
            if (userDetail == null)
            {
                return HttpNotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetail userDetail = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetail);
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
    }
}

