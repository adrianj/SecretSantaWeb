using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SecretSantaWeb.Models;

namespace SecretSantaWeb.Controllers
{
    public class ExclusionsController : Controller
    {
        private SecretSantaWebContext db = new SecretSantaWebContext();

        // GET: Exclusions
        public ActionResult Index()
        {
            return View(db.Exclusions.ToList());
        }

        // GET: Exclusions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exclusion exclusion = db.Exclusions.Find(id);
            if (exclusion == null)
            {
                return HttpNotFound();
            }
            return View(exclusion);
        }

        // GET: Exclusions/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.People, "PersonID", "Name");
            ViewBag.NotBuyingForID = new SelectList(db.People, "PersonID", "Name");
            return View();
        }

        // POST: Exclusions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExclusionID,NotBuyingForID,OwnerID")] Exclusion exclusion)
        {
            if (ModelState.IsValid)
            {
                exclusion.NotBuyingFor = db.People.Find(exclusion.NotBuyingForID);
                exclusion.Owner = db.People.Find(exclusion.OwnerID);
                if (exclusion.Owner == null)
                {
                    return HttpNotFound();
                }
                exclusion.Owner.Exclusions.Add(exclusion);
                db.Exclusions.Add(exclusion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.People, "PersonID", "Name", exclusion.OwnerID);
            ViewBag.NotBuyingForID = new SelectList(db.People, "PersonID", "Name", exclusion.NotBuyingForID);
            return View(exclusion);
        }

        // GET: Exclusions/AddToPerson/5
        public ActionResult AddToPerson(int? personID)
        {
            if (personID == null)
                return RedirectToAction("Create");
            Person person = db.People.Find(personID);
            if (person == null)
                return HttpNotFound();
            Exclusion e = new Exclusion() { OwnerID = person.PersonID, Owner = person };
            ViewBag.OwnerID = new SelectList(db.People.Where(p => p.Name == person.Name), "PersonID", "Name", person.Name);
            ViewBag.NotBuyingForID = new SelectList(db.People.Where(p => p.FamilyID == person.FamilyID), "PersonID", "Name");
            return View(e);
        }

        // POST: Exclusions/AddToPerson/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToPerson([Bind(Include = "ExclusionID,NotBuyingForID,OwnerID")] Exclusion exclusion)
        {
            if (ModelState.IsValid)
            {
                exclusion.NotBuyingFor = db.People.Find(exclusion.NotBuyingForID);
                exclusion.Owner = db.People.Find(exclusion.OwnerID);
                if (exclusion.Owner == null)
                {
                    return HttpNotFound();
                }
                exclusion.Owner.Exclusions.Add(exclusion);
                db.Exclusions.Add(exclusion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.People, "PersonID", "Name", exclusion.OwnerID);
            ViewBag.NotBuyingForID = new SelectList(db.People, "PersonID", "Name", exclusion.NotBuyingForID);
            return View(exclusion);
        }

        // GET: Exclusions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exclusion exclusion = db.Exclusions.Find(id);
            if (exclusion == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(db.People, "PersonID", "Name", exclusion.OwnerID);
            ViewBag.NotBuyingForID = new SelectList(db.People, "PersonID", "Name", exclusion.NotBuyingForID);
            return View(exclusion);
        }

        // POST: Exclusions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExclusionID,NotBuyingForID,OwnerID")] Exclusion exclusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exclusion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(db.People, "PersonID", "Name", exclusion.OwnerID);
            ViewBag.NotBuyingForID = new SelectList(db.People, "PersonID", "Name", exclusion.NotBuyingForID);
            return View(exclusion);
        }

        // GET: Exclusions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exclusion exclusion = db.Exclusions.Find(id);
            if (exclusion == null)
            {
                return HttpNotFound();
            }
            return View(exclusion);
        }

        // POST: Exclusions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exclusion exclusion = db.Exclusions.Find(id);
            db.Exclusions.Remove(exclusion);
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
