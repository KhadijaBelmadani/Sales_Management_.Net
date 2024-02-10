using PagedList;
using SiteDeVente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteDeVente.Controllers
{
    public class CategoriesProduitsController : Controller
    {
        private readonly dbContext _context;

        // GET: CategoriesProduits
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListeProduitsParCategorie(int idCategorie, int? page)
        {
            const int pageSize = 8;
            using (var dbContext = new dbContext())
            {
                var produits = dbContext.Produits.Where(p => p.idCategorieF == idCategorie)
                                                 .OrderBy(p => p.idProduit) // Assurez-vous de spécifier un ordre pour une pagination cohérente
                                                 .ToPagedList(page ?? 1, pageSize);
                return View(produits);
            }
        }
    }
}