using SiteDeVente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace SiteDeVente.Controllers
{
    public class HomeController : Controller
    {

        private readonly dbContext _context = new dbContext();

        public ActionResult Index()
        {
            var categoriesWithProducts = _context.Categories
                .Include(c => c.Produits)
                .ToList();

            return View(categoriesWithProducts);
        }

        public PartialViewResult CategoryPartial()
        {
            var categories = _context.Categories.ToList();
            return PartialView("_CategoryPartial", categories);
        }


        public PartialViewResult ListeProduits()
        {
            using (var dbContext = new dbContext())
            {
                // Obtenez la date de début et de fin du mois en cours
                DateTime debutMois = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime finMois = debutMois.AddMonths(1).AddSeconds(-1);

                // Filtrez les produits du mois en cours
                List<Produit> produits = dbContext.Produits
                    .Where(p => p.date >= debutMois && p.date <= finMois)
                    .ToList();

                return PartialView("_ListeProduitsPartial", produits);
            }
        }
        public PartialViewResult ListeProduitssuggesion(int? produitId)
        {
            using (var dbContext = new dbContext())
            {
                // Récupérez tous les produits sauf celui avec l'ID spécifié
                List<Produit> produits = dbContext.Produits
                    .Where(p => p.idProduit != produitId)
                    .ToList();

                return PartialView("_ListeProduitssuggesionPartial", produits);
            }
        }

        public PartialViewResult ImageCarousel()
        {
            using (var db = new dbContext())
            {
                var users = db.Users.Where(u => u.role == "propriétaire" && u.image != null).ToList();
                return PartialView("_ImageCarouselPartial", users);
            }
        }
        public ActionResult Details(int id)
        {

            using (var dbContext = new dbContext())
            {
                // Utilisez Include pour charger les propriétés liées (dans ce cas, la catégorie, les UserProduits et les Users associés)
                Produit produit = dbContext.Produits
                .Include(p => p.Categorie)
                 .Include(p => p.User)
               .FirstOrDefault(p => p.idProduit == id);

                if (produit == null)
                {
                    return HttpNotFound();
                }



                return View(produit);
            }
        }

        public PartialViewResult Offre()
        {
            using (var db = new dbContext())
            {
                // Récupérer les deux produits avec le pourcentage de solde le plus élevé
                List<Produit> produits = db.Produits
                .Where(p => p.pourcentageSolde != null)
                .OrderByDescending(p => p.pourcentageSolde)
                
                .ToList();

                return PartialView("_OffrePartial", produits);
            }
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

		public ActionResult Admin()
		{

			return View();
		}

		public ActionResult ListeDeTousLesUsers()
		{
			// Logique de l'action Admin

			// Redirection vers l'action "Index" du contrôleur "Users"
			return RedirectToAction("Index", "Users");
		}
	}
}