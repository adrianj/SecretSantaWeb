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
    public class PeopleController : Controller
    {
        private SecretSantaWebContext db = new SecretSantaWebContext();

        // GET: People
        public ActionResult Index()
        {
            var people = db.People.Include(p => p.Family);
            return View(people.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            CheckPersonModel(person);
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.FamilyID = new SelectList(db.Families, "FamilyID", "FamilyName");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,Name,FamilyID")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FamilyID = new SelectList(db.Families, "FamilyID", "FamilyName", person.FamilyID);
            return View(person);
        }

        // Get: People/AddToFamily/1
        public ActionResult AddToFamily(int ?familyID)
        {
            if (familyID == null)
                return RedirectToAction("Create");
            Family family = db.Families.Find(familyID);
            if (family == null)
                return HttpNotFound();
            Person p = new Person() { FamilyID = family.FamilyID, Family = family,  };
            ViewBag.FamilyID = new SelectList(db.Families, "FamilyID", "FamilyName");
            return View(p);
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToFamily([Bind(Include = "PersonID,Name,FamilyID")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FamilyID = new SelectList(db.Families, "FamilyID", "FamilyName", person.FamilyID);
            return View(person);
        }

        List<SelectListItem> GetFamilyMemberSelectList(int familyID, object selectedValue, bool inculdeEmptyOption)
        {
            var fam = db.People.Where(p => p.FamilyID == familyID);

            var sl = new SelectList(fam, "PersonID", "Name", selectedValue).ToList();
            if(inculdeEmptyOption)
                sl.Insert(0, new SelectListItem { Text = "[Empty]", Value = "0" });
            return sl;
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.NotBuyingForID = GetFamilyMemberSelectList(person.FamilyID, person.NotBuyingForID, true);
            ViewBag.BuyingForID = GetFamilyMemberSelectList(person.FamilyID, person.BuyingForID, true);
            ViewBag.FamilyID = new SelectList(db.Families, "FamilyID", "FamilyName", person.FamilyID);

            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,Name,FamilyID,BuyingForID,NotBuyingForID,BuyingFor,NotBuyingFor")] Person person)
        {
            if (ModelState.IsValid)
            {
                var toUpdate = db.People.Find(person.PersonID);
                if (toUpdate == null)
                    return HttpNotFound();

                toUpdate.Name = person.Name;
                toUpdate.FamilyID = person.FamilyID;
                if (!UpdateBuyingFor(toUpdate, person.BuyingForID,true))
                    return HttpNotFound();
                if (!UpdateBuyingFor(toUpdate, person.NotBuyingForID, false))
                    return HttpNotFound();

                db.People.Attach(toUpdate);
                db.Entry(toUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NotBuyingForID = GetFamilyMemberSelectList(person.FamilyID, person.NotBuyingForID, true);
            ViewBag.BuyingForID = GetFamilyMemberSelectList(person.FamilyID, person.BuyingForID, true);
            ViewBag.FamilyID = new SelectList(db.Families, "FamilyID", "FamilyName", person.FamilyID);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        

        bool UpdateBuyingFor(Person toUpdate, int? buyingID, bool isBuyingFor)
        {
            Person other = null;
            if (buyingID == null || buyingID <= 0)
            {
                if (isBuyingFor)
                {  toUpdate.BuyingForID = null; toUpdate.BuyingFor = null; }
                else
                { toUpdate.NotBuyingForID = null; toUpdate.NotBuyingFor = null; }
            }
            else
            {
                other = db.People.Find(buyingID);
                if (other == null)
                    return false;
                if (isBuyingFor)
                { toUpdate.BuyingForID = other.PersonID; toUpdate.BuyingFor = other; }
                else
                { toUpdate.NotBuyingForID = other.PersonID; toUpdate.NotBuyingFor = other; }
            }
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        void CheckPersonModel(Person model)
        {
            if (model.NotBuyingForID == null)
                model.NotBuyingFor = null;
            else
            {
                // Fix up link to actual person. It they aren't there (deleted), null the ID
                model.NotBuyingFor = db.People.Find(model.NotBuyingForID);
                if (model.NotBuyingFor == null)
                    model.NotBuyingForID = null;
            }
            if (model.BuyingForID == null)
                model.BuyingFor = null;
            else
            {
                model.BuyingFor = db.People.Find(model.BuyingForID);
                if (model.BuyingFor == null)
                    model.BuyingForID = null;
            }
        }
    }
}
