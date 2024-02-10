using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SiteDeVente.Models;

namespace SiteDeVente.Controllers
{
    public class AchetersController : Controller
    {
        private dbContext db = new dbContext();

        [HttpPost]
        public ActionResult AjouterALaListeDeSouhaits(int idProduit)
        {
            // Récupérer l'ID de l'utilisateur actuel, par exemple à partir de la session
            var idObject = Session["UserId"];
            int id;
            if (idObject != null && int.TryParse(idObject.ToString(), out id))
            {
                // Vérifier si le produit est déjà dans la liste de souhaits de l'utilisateur
                bool produitExiste = db.Favorisers.Any(f => f.idUserF == id && f.idProduitF == idProduit);

                if (!produitExiste)
                {
                    // Insérer une nouvelle entrée dans la table Favoriser pour représenter le produit ajouté à la liste de souhaits
                    Favoriser nouveauFavori = new Favoriser
                    {
                        idUserF = id,
                        idProduitF = idProduit,
                        date = DateTime.Now
                    };

                    // Ajouter le favori dans la base de données
                    db.Favorisers.Add(nouveauFavori);
                    db.SaveChanges();
                    int nombreProduitsDansListeSouhaits = db.Favorisers.Count(f => f.idUserF == id);
                    ViewBag.NombreProduitsDansListeSouhaits = nombreProduitsDansListeSouhaits;


                    TempData["Message"] = "Le produit a été ajouté à votre liste de souhaits.";
                }
                else
                {
                    TempData["Message"] = "Le produit est déjà dans votre liste de souhaits.";
                }
            }
            else
            {
                // Gérer le cas où la valeur de session n'est pas valide ou n'est pas présente
                TempData["Message"] = "Impossible de récupérer l'ID de l'utilisateur.";
            }

            return RedirectToAction("Details", new { id = idProduit });
        }

        [HttpPost]
        public ActionResult Acheter(int idProduit, int quantite)
        {
            
            var id = Session["UserId"];
            if (id == null)
            {
                // L'utilisateur n'est pas connecté, affichez une alerte appropriée
                TempData["Message"] = new MvcHtmlString("Vous devez <a href='" + Url.Action("Signup", "LoginRegister") + "'>vous inscrire</a> ou <a href='" + Url.Action("Login", "LoginRegister") + "'>vous connecter</a> pour effectuer un achat.");
                return RedirectToAction("Details/" + idProduit, "Produits");
            }

           
            // Vérifiez si la quantité est disponible
            // Effectuez les vérifications nécessaires pour déterminer si la quantité est disponible pour l'achat

            if (quantite >0)
            {
               
                // Insérer les données dans la table Acheter
                using (var context = new dbContext())
                {
					var produit = context.Produits.Find(idProduit);

					var acheter = new Acheter
                    {
                        idUserF = Convert.ToInt32(id),
                        idProduitF = idProduit,
                        quantite = quantite,
                        date = DateTime.Now
                    };

                    context.Acheters.Add(acheter);
					produit.quantite -= quantite;
					context.SaveChanges();
                }

                // Affichez une alerte "produit ajouté"
                TempData["Message"] = "Produit acheté";
            }
            else
            {
                // Affichez une alerte "quantité non disponible"
                TempData["Message"] = "Quantité non disponible";
            }

            return RedirectToAction("Details/"+idProduit,"Produits"); // Redirigez vers la page d'accueil ou une autre page appropriée
        }
        // GET: Acheters
        public ActionResult Index()
        {
            var acheters = db.Acheters.Include(a => a.Produit).Include(a => a.User);
            return View(acheters.ToList());
        }

        // GET: Acheters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acheter acheter = db.Acheters.Find(id);
            if (acheter == null)
            {
                return HttpNotFound();
            }
            return View(acheter);
        }

        // GET: Acheters/Create
        public ActionResult Create()
        {
            ViewBag.idProduitF = new SelectList(db.Produits, "idProduit", "nom");
            ViewBag.idUserF = new SelectList(db.Users, "id", "role");
            return View();
        }

        // POST: Acheters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,idUserF,idProduitF,quantite,date")] Acheter acheter)
        {
            if (ModelState.IsValid)
            {
                db.Acheters.Add(acheter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProduitF = new SelectList(db.Produits, "idProduit", "nom", acheter.idProduitF);
            ViewBag.idUserF = new SelectList(db.Users, "id", "role", acheter.idUserF);
            return View(acheter);
        }

        // GET: Acheters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acheter acheter = db.Acheters.Find(id);
            if (acheter == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProduitF = new SelectList(db.Produits, "idProduit", "nom", acheter.idProduitF);
            ViewBag.idUserF = new SelectList(db.Users, "id", "role", acheter.idUserF);
            return View(acheter);
        }

        // POST: Acheters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,idUserF,idProduitF,quantite,date")] Acheter acheter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acheter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProduitF = new SelectList(db.Produits, "idProduit", "nom", acheter.idProduitF);
            ViewBag.idUserF = new SelectList(db.Users, "id", "role", acheter.idUserF);
            return View(acheter);
        }

        // GET: Acheters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acheter acheter = db.Acheters.Find(id);
            if (acheter == null)
            {
                return HttpNotFound();
            }
            return View(acheter);
        }

        // POST: Acheters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Acheter acheter = db.Acheters.Find(id);
            db.Acheters.Remove(acheter);
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
