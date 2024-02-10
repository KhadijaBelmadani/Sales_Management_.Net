using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiteDeVente.Models;

namespace SiteDeVente.Controllers
{
    public class ProduitsController : Controller
    {
        private dbContext db = new dbContext();

        // GET: Produits

        public ActionResult Index()
        {
            
            var produits = db.Produits.Include(p => p.Categorie).Include(p => p.User);
            return View(produits.ToList());
        }

        public ActionResult FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            var filteredProducts = db.Produits
                .Where(p => p.prix >= minPrice && p.prix <= maxPrice)
                .ToList();

            return PartialView("_ProductList", filteredProducts);
        }
        public ActionResult Search(string searchTerm)
        {
            var produits = db.Produits
                .Include(p => p.Categorie)
                .Include(p => p.User)
                .Where(p => p.nom.Contains(searchTerm))
                .ToList();

            return View("Search", produits);
        }
        public ActionResult NewProducts()
        {
            var newestProducts = db.Produits
                .OrderByDescending(p => p.date)
                .Take(3)
                .ToList();

            return View(newestProducts);
        }
        
        // GET: Produits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // GET: Produits/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "idCategorie", "nom");
          
            ViewBag.idUserF = Session["UserId"];

            var produit = new Produit(); // Créez une instance de Produit
            return View(produit); // Passez l'instance de Produit à la vue
        }

        // POST: Produits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(Produit prod)
        {
            ViewBag.Categories = new SelectList(db.Categories, "idCategorie", "nom");
            var id = Session["UserId"];
            string filename = Path.GetFileNameWithoutExtension(prod.ImageFile.FileName);
            string extension = Path.GetExtension(prod.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            prod.image = "../img/" + filename;
            filename = Path.Combine(Server.MapPath("../img/"), filename);
            prod.ImageFile.SaveAs(filename);
            using (dbContext dbc = new dbContext())
            {
                prod.idUserF=Convert.ToInt32(id);
                dbc.Produits.Add(prod);
                dbc.SaveChanges();
            }
            ModelState.Clear();



            return View();
        }

        // GET: Produits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategorieF = new SelectList(db.Categories, "idCategorie", "nom", produit.idCategorieF);
            ViewBag.idUserF = new SelectList(db.Users, "id", "role", produit.idUserF);
            return View(produit);
        }

        // POST: Produits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProduit,nom,description,prix,pourcentageSolde,quantite,valeursSpecifiques,idCategorieF,image,idUserF,date")] Produit produit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategorieF = new SelectList(db.Categories, "idCategorie", "nom", produit.idCategorieF);
            ViewBag.idUserF = new SelectList(db.Users, "id", "role", produit.idUserF);
            return View(produit);
        }

        // GET: Produits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produit produit = db.Produits.Find(id);
            db.Produits.Remove(produit);
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
