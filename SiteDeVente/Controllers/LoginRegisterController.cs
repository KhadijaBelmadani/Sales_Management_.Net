using SiteDeVente.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;


namespace SiteDeVente.Controllers
{
    public class LoginRegisterController : Controller
    {
        private dbContext db = new dbContext();

        // GET: User/Create
        public ActionResult Signup()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Signup(User user)
        {
            if (ModelState.IsValid)
            {
                // Ajouter le code pour sauvegarder l'utilisateur dans la base de données
                // Exemple avec Entity Framework :
                using (var context = new dbContext())
                {
                    user.date = DateTime.Now;
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                return RedirectToAction("Index", "Home"); // Rediriger vers la page d'accueil ou une autre page
            }

            return View();
        }

        // Remplacez YourDbContext par le contexte de votre base de données

        // ... d'autres actions du contrôleur





        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var checkLogin = db.Users.FirstOrDefault(x => x.email.Equals(user.email) && x.password.Equals(user.password));
            if (checkLogin != null)
            {
                // Récupérer les informations de l'utilisateur depuis la base de données
                var userFromDatabase = db.Users.FirstOrDefault(x => x.email.Equals(user.email) && x.password.Equals(user.password));

				// Vérifier si l'utilisateur existe dans la base de données
				if (userFromDatabase != null)
                {
                    // Enregistrer les informations de l'utilisateur dans la session
                    Session["UserId"] = userFromDatabase.id;
                    Session["UserNom"] = userFromDatabase.nomComplet;
                    Session["UserRole"] = userFromDatabase.role;
                    Session["UserEmail"] = userFromDatabase.email;
                    Session["UserTelephone"] = userFromDatabase.tel;
                    Session["UserPassword"] = userFromDatabase.password;
                    Session["UserAdresse"] = userFromDatabase.adresse;
                    Session["UserSpecialite"] = userFromDatabase.specialite;

                    Session["UserImage"] = userFromDatabase.image;

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Notification = "Email ou mot de passe incorrect !";
            }

            return View();
        }

        public JsonResult IsEmailUnique(string email)
        {
            bool isUnique = !db.Users.Any(x => x.email == email);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }








        public ActionResult Logout()
        {
            // Effacer toutes les valeurs de session ou effectuer toute autre opération de déconnexion nécessaire
            Session["UserNom"] = "";
            // Rediriger vers la vue de connexion
            return RedirectToAction("Index", "Home");
        }
    }
}