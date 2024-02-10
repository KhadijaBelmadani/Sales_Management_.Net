using Microsoft.AspNet.Identity;
using SiteDeVente.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteDeVente.Controllers
{

    public class ProfileController : Controller
    {
        private dbContext db = new dbContext();

        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Infos()
        {
            return View();
        }

        private readonly dbContext _dbContext = new dbContext();

        // Other actions...

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(User editedUser, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Récupérer l'utilisateur à partir de la base de données
                var password = Session["UserPassword"] as string;
                var email = Session["UserEmail"] as string;

                // Récupérer l'utilisateur à partir de la base de données en utilisant le nom complet et l'e-mail
                User userToUpdate = _dbContext.Users.FirstOrDefault(x => x.password == password && x.email == email);

                if (userToUpdate != null)
                {
                    // Mettre à jour les propriétés de l'utilisateur
                    userToUpdate.nomComplet = editedUser.nomComplet;
                    userToUpdate.email = editedUser.email;
                    userToUpdate.tel = editedUser.tel;
                    userToUpdate.password = editedUser.password;
                    userToUpdate.adresse = editedUser.adresse;

					// Vérifier si un fichier a été téléchargé
					// partie khadija
					//if (editedUser.ImageFile != null && editedUser.ImageFile.ContentLength > 0)
					//{
					//    // Convertir le fichier en tableau de bytes
					//    using (var binaryReader = new BinaryReader(editedUser.ImageFile.InputStream))
					//    {
					//        userToUpdate.image = binaryReader.ReadBytes(editedUser.ImageFile.ContentLength);
					//    }
					//}
					if (editedUser.ImageFile != null && editedUser.ImageFile.ContentLength > 0)
					{
						// Si une nouvelle image est téléchargée, enregistrez le chemin d'accès à cette image
						string filename = Path.GetFileNameWithoutExtension(editedUser.ImageFile.FileName);
						string extension = Path.GetExtension(editedUser.ImageFile.FileName);
						filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
						userToUpdate.image = "/img/User/" + filename;

						// Enregistrez le fichier sur le serveur
						filename = Path.Combine(Server.MapPath("/img/User/"), filename);
						editedUser.ImageFile.SaveAs(filename);
					}
					//using (dbContext dbc = new dbContext())
					//{
					//	dbc.Users.Add(userToUpdate);
					//	dbc.SaveChanges();
					//}
					//ModelState.Clear();

					// Mettre à jour les champs supplémentaires pour les sociétés
					if (Session["UserRole"] as string == "societe")
                    {
                        userToUpdate.specialite = editedUser.specialite;
                    }

                    // Enregistrer les changements dans la base de données
                    _dbContext.SaveChanges();
                    Session["UserNom"] = userToUpdate.nomComplet;
                    Session["UserRole"] = userToUpdate.role;
                    Session["UserEmail"] = userToUpdate.email;
                    Session["UserTelephone"] = userToUpdate.tel;
                    Session["UserPassword"] = userToUpdate.password;
                    Session["UserAdresse"] = userToUpdate.adresse;
                    Session["UserSpecialite"] = userToUpdate.specialite;
                    Session["UserImage"] = userToUpdate.image;


                    ViewBag.Notification = "Profil mis à jour avec succès.";
					ModelState.Clear();
					return RedirectToAction("Infos", "Profile");
                }
                else
                {
                    ViewBag.Notification = "Utilisateur non trouvé.";
                }
            }

            // Si la validation échoue, retourner à la vue d'édition avec les erreurs
            return RedirectToAction("Index", "Home");
        }



        //Partie Mes Commandes
        public ActionResult Achats()
        {
            // Récupérer
            // l'utilisateur connecté
            var password = Session["UserPassword"] as string;
            var email = Session["UserEmail"] as string;
            var userAuthentifie = db.Users.FirstOrDefault(x => x.email.Equals(email) && x.password.Equals(password));

            // Récupérer les achats de l'utilisateur connecté à partir de la base de données
            var achats = db.Acheters.Where(a => a.idUserF == userAuthentifie.id).ToList();

            // Passer les données à la vue
            return View(achats);
        }

        public ActionResult Produits()
        {
            // Récupérer
            // l'utilisateur connecté
            var password = Session["UserPassword"] as string;
            var email = Session["UserEmail"] as string;
            var userAuthentifie = db.Users.FirstOrDefault(x => x.email.Equals(email) && x.password.Equals(password));

            // Récupérer les achats de l'utilisateur connecté à partir de la base de données
            var produits = db.Produits.Where(p => p.idUserF == userAuthentifie.id).ToList();
            // Passer les données à la vue
            return View(produits);
        }

        public ActionResult Historique()
        {
            // Assurez-vous de remplacer cela par votre propre logique d'authentification
            var userId = (int)Session["UserId"];

            // Ajouter l'entrée pour la création du compte
            var accountCreationEntry = db.Users
                .Where(u => u.id == userId && u.date != null)
                .OrderByDescending(u => u.date)
                .Select(u => new HistoryViewModel
                {
                    Date = u.date.Value,
                    Action = "Création de compte",
                    Details = "Compte créé"
                })
                .FirstOrDefault();

            var productAdditions = db.Produits
                .Where(p => p.idUserF == userId && p.date != null)
                .OrderByDescending(p => p.date)
                .ToList()
                .Select(p => new HistoryViewModel
                {
                    Date = p.date.Value,
                    Action = "Ajout de produit",
                    Details = $"{p.nom} - Quantité: {p.quantite}, Prix: {p.prix}"
                })
                .ToList();

            var productPurchases = db.Acheters
                .Where(a => a.idUserF == userId && a.date != null)
                .OrderByDescending(a => a.date)
                .ToList()
                .Select(a => new HistoryViewModel
                {
                    Date = a.date.Value,
                    Action = "Achat de produit",
                    Details = $"{a.Produit.nom} - Quantité: {a.quantite}, Prix total: {a.Produit.prix * a.quantite}"
                })
                .ToList();

            var productLikes = db.Favorisers
                .Where(f => f.idUserF == userId && f.date != null)
                .OrderByDescending(f => f.date)
                .ToList()
                .Select(f => new HistoryViewModel
                {
                    Date = f.date.Value,
                    Action = "Produit aimé",
                    Details = f.Produit.nom
                })
                .ToList();

            var history = new List<HistoryViewModel> { accountCreationEntry }
                .Concat(productAdditions)
                .Concat(productPurchases)
                .Concat(productLikes)
                .OrderByDescending(item => item.Date)
                .ToList();

            return View(history);
        }



        public ActionResult WishList()
        {
            // Récupérer
            // l'utilisateur connecté
            var password = Session["UserPassword"] as string;
            var email = Session["UserEmail"] as string;
            var userAuthentifie = db.Users.FirstOrDefault(x => x.email.Equals(email) && x.password.Equals(password));

            // Récupérer les achats de l'utilisateur connecté à partir de la base de données
            var favoris = db.Favorisers.Where(f => f.idUserF == userAuthentifie.id).Select(f => f.Produit).ToList();

            return View(favoris);
        }


        // GET: WishList/Delete/5
        public ActionResult Delete(int id)
        {
            var password = Session["UserPassword"] as string;
            var email = Session["UserEmail"] as string;
            var userAuthentifie = db.Users.FirstOrDefault(x => x.email.Equals(email) && x.password.Equals(password));

            // Vérifier si l'élément favori existe pour l'utilisateur connecté
            var favori = db.Favorisers.FirstOrDefault(f => f.idUserF == userAuthentifie.id && f.idProduitF == id);
            if (favori == null)
            {
                // Gérer le cas où l'élément favori n'est pas trouvé
                return HttpNotFound();
            }

            // Supprimer l'élément favori de la table Favoriser
            db.Favorisers.Remove(favori);
            db.SaveChanges();

            ViewBag.Notification = "Avec succes";
            // Rediriger vers la vue de la liste des favoris mise à jour
            return RedirectToAction("WishList");
        }


        public ActionResult DeleteProduit(int id)
        {
            var password = Session["UserPassword"] as string;
            var email = Session["UserEmail"] as string;
            var userAuthentifie = db.Users.FirstOrDefault(x => x.email.Equals(email) && x.password.Equals(password));

            // Vérifier si l'élément favori existe pour l'utilisateur connecté
            var produit = db.Produits.FirstOrDefault(f => f.idUserF == userAuthentifie.id && f.idProduit == id);
            if (produit == null)
            {
                // Gérer le cas où l'élément favori n'est pas trouvé
                return HttpNotFound();
            }

            // Supprimer l'élément favori de la table Favoriser
            db.Produits.Remove(produit);
            db.SaveChanges();

            ViewBag.Notification = "Avec succes";
            // Rediriger vers la vue de la liste des favoris mise à jour
            return RedirectToAction("Produits");
        }

    }
}