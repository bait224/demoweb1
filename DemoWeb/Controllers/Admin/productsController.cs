using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoWeb.Models;
using PagedList;

namespace DemoWeb.Controllers.Admin
{
    public class productsController : Controller
    {
        private demoDBEntities db = new demoDBEntities();

        // GET: foods
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            var product = db.products.Include(f => f.category).Include(f => f.Unit).OrderBy(f => f.categoryid);
            int pageSize = 5;

            return View(product.ToPagedList(pageNumber,pageSize));
        }

        // GET: foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product prod = db.products.Find(id);
            if (prod == null)
            {
                return HttpNotFound();
            }
            return View(prod);
        }

        // GET: foods/Create
        public ActionResult Create()
        {
            ViewBag.categoryid = new SelectList(db.categories, "id", "name");
            ViewBag.unit_id = new SelectList(db.Units, "id", "unit_name");
            return View();
        }

        // POST: foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,unit_id,price,categoryid")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.categoryid = new SelectList(db.categories, "id", "name", product.categoryid);
            ViewBag.unit_id = new SelectList(db.Units, "id", "unit_name", product.unit_id);
            return View(product);
        }

        // GET: foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.categoryid = new SelectList(db.categories, "id", "name", product.categoryid);
            ViewBag.unit_id = new SelectList(db.Units, "id", "unit_name", product.unit_id);
            return View(product);
        }

        // POST: foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,unit_id,price,categoryid")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryid = new SelectList(db.categories, "id", "name", product.categoryid);
            ViewBag.unit_id = new SelectList(db.Units, "id", "unit_name", product.unit_id);
            return View(product);
        }

        // GET: foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
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
