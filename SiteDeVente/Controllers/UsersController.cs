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
	public class UsersController : Controller
	{
		private dbContext db = new dbContext();

		// GET: Users
		public ActionResult Index()
		{
			return View(db.Users.ToList());
		}

		// GET: Users/Favoris
		public ActionResult Favoris()
		{
			return View(db.Users.ToList());
		}


		public ActionResult Reclamations()
		{
			return View();
		}

		public ActionResult Categories()
		{
			return View();
		}

		public ActionResult BlackList()
		{
			return View(db.Users.ToList());
		}

		// GET: Users/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// GET: Users/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Users/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Create([Bind(Include = "id,role,nomComplet,image,adresse,specialite,tel,email,password,idCommandeF")] User user)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		db.Users.Add(user);
		//		db.SaveChanges();
		//		return RedirectToAction("Index");
		//	}

		//	return View(user);
		//}

		// GET: Users/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// POST: Users/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "id,role,nomComplet,image,adresse,specialite,tel,email,password,idCommandeF")] User user)
		{
			if (ModelState.IsValid)
			{
				db.Entry(user).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(user);
		}

		// GET: Users/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			User user = db.Users.Find(id);
			db.Users.Remove(user);
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

		// GET: Users/AddToBlackList/5
		public ActionResult AddToBlackList(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			User user = db.Users.Find(id);

			if (user == null)
			{
				return HttpNotFound();
			}

			// Mettez à jour l'attribut blackList à 1
			user.blackList = 1;
			db.SaveChanges();

			// Redirigez vers la page Index
			return RedirectToAction("Index");
		}

		// GET: Users/AddToFavorites/5
		public ActionResult AddToFavorites(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			User user = db.Users.Find(id);

			if (user == null)
			{
				return HttpNotFound();
			}

			// Mettez à jour l'attribut favoris à 1
			user.favoris = 1;
			db.SaveChanges();

			// Redirigez vers la page Index
			return RedirectToAction("Index");
		}

		// GET: Users/AddToFavorites/5
		public ActionResult RemoveFromFavorites(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			User user = db.Users.Find(id);

			if (user == null)
			{
				return HttpNotFound();
			}

			user.favoris = 0;
			db.SaveChanges();

			return RedirectToAction("Favoris");
		}

		public ActionResult AddToBlackList2(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			User user = db.Users.Find(id);

			if (user == null)
			{
				return HttpNotFound();
			}

			// Mettez à jour l'attribut blackList à 1
			user.blackList = 1;
			db.SaveChanges();

			// Redirigez vers la page Index
			return RedirectToAction("BlackList");
		}

		public ActionResult RemoveFromBlack(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			User user = db.Users.Find(id);

			if (user == null)
			{
				return HttpNotFound();
			}

			user.blackList = 0;
			db.SaveChanges();

			return RedirectToAction("BlackList");
		}

		public ActionResult GetList()
		{
			using (dbContext db = new dbContext())
			{
				var empList = db.Users.ToList<User>();
				return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
			}
		}


	}
}
