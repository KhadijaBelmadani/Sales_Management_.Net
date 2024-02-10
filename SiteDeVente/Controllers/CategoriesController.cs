
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using SiteDeVente.Models;
using System.Data.SqlClient;
using System.Net;

public class CategoriesController : Controller
{
	// GET: Categories
	private readonly dbContext db = new dbContext();
	string connectionString = "data source=(LocalDB)\\MSSQLLocalDB;attachdbfilename=|DataDirectory|VenteDB.mdf;integrated security=True;App=EntityFramework";

	// Action pour afficher la vue partielle des catégories et produits
	public ActionResult CategoriesPartial()
	{
		var categoriesWithProducts = db.Categories
			.Include(c => c.Produits)
			.ToList();

		return PartialView("_CategoriesPartial", categoriesWithProducts);
	}
	public ActionResult CategoryNavigation()
	{
		using (var context = new dbContext())
		{
			var categories = context.Categories.Include("Produits").ToList();
			return PartialView("CategoryNavigation", categories);
		}
	}

	public ActionResult Index()
	{
		return View();
		//db.Categories.ToList()
	}

	// GET: Categories/Details/5
	public ActionResult Details(int? id)
	{
		if (id == null)
		{
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}
		Categorie categorie = db.Categories.Find(id);
		if (categorie == null)
		{
			return HttpNotFound();
		}
		return View(categorie);
	}

	// GET: Categories/Create
	public ActionResult Create()
	{
		return View();
	}

	// POST: Categories/Create
	// To protect from overposting attacks, enable the specific properties you want to bind to, for 
	// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Create([Bind(Include = "idCategorie,nom,attributsSpecifiques,image")] Categorie categorie)
	{
		if (ModelState.IsValid)
		{
			db.Categories.Add(categorie);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		return View(categorie);
	}

	// GET: Categories/Edit/5
	public ActionResult Edit(int? id)
	{
		if (id == null)
		{
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}
		Categorie categorie = db.Categories.Find(id);
		if (categorie == null)
		{
			return HttpNotFound();
		}
		return View(categorie);
	}

	// POST: Categories/Edit/5
	// To protect from overposting attacks, enable the specific properties you want to bind to, for 
	// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
	[HttpPost]
	[ValidateAntiForgeryToken]
	public ActionResult Edit([Bind(Include = "idCategorie,nom,attributsSpecifiques,image")] Categorie categorie)
	{
		if (ModelState.IsValid)
		{
			db.Entry(categorie).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Index");
		}
		return View(categorie);
	}

	// GET: Categories/Delete/5
	public ActionResult Delete(int? id)
	{
		if (id == null)
		{
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}
		Categorie categorie = db.Categories.Find(id);
		if (categorie == null)
		{
			return HttpNotFound();
		}
		return View(categorie);
	}

	// POST: Categories/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public ActionResult DeleteConfirmed(int id)
	{
		Categorie categorie = db.Categories.Find(id);
		db.Categories.Remove(categorie);
		db.SaveChanges();
		return RedirectToAction("Index");
	}

	public ActionResult Delete2(int? id)
	{
		if (id == null)
		{
			return RedirectToAction("Index");
		}
		Categorie categorie = db.Categories.Find(id);
		if (categorie == null)
		{
			return RedirectToAction("Index");
		}
		return View(categorie);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			db.Dispose();
		}
		base.Dispose(disposing);
	}


	[HttpPost]
	public ActionResult Create1(string nomCategorie, string imageCategorie, string attributsSpecifiquesCategorie)
	{
		using (var context = new dbContext())
		{


			var categorie = new Categorie
			{
				nom = nomCategorie,
				attributsSpecifiques = attributsSpecifiquesCategorie,
				image = imageCategorie
			};

			context.Categories.Add(categorie);
			context.SaveChanges();
		}

		return RedirectToAction("Index", "Categories"); // Redirigez vers la page d'accueil ou une autre page appropriée
	}

	[HttpPost]
	public ActionResult Edit1(string idCategorieEdit, string nomCategorieEdit, string imageCategorieEdit, string attributsSpecifiquesCategorieEdit)
	{

		using (var context = new dbContext())
		{
			Categorie categorie = new Categorie
			{
				idCategorie = Convert.ToInt32(idCategorieEdit),
				nom = nomCategorieEdit,
				attributsSpecifiques = attributsSpecifiquesCategorieEdit,
				image = imageCategorieEdit
			};

			if (ModelState.IsValid)
			{
				db.Entry(categorie).State = EntityState.Modified;
				db.SaveChanges();
			}
		}


		return RedirectToAction("Index", "Categories");
	}

	[HttpPost]
	public ActionResult Delete1(int? id)
	{
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			connection.Open();

			//string sqlQuery = "DELETE FROM Categorie WHERE idCategorie = @idCategorie";

			//using (SqlCommand command = new SqlCommand(sqlQuery, connection))
			//{
			//	command.Parameters.AddWithValue("@idCategorie", id);
			//	command.ExecuteNonQuery();
			//}
			string deleteFavoriserQuery = "DELETE FROM Favoriser WHERE idProduitF IN (SELECT idProduit FROM Produit WHERE idCategorieF = @idCategorie)";
			using (SqlCommand deleteFavoriserCommand = new SqlCommand(deleteFavoriserQuery, connection))
			{
				deleteFavoriserCommand.Parameters.AddWithValue("@idCategorie", id);
				deleteFavoriserCommand.ExecuteNonQuery();
			}
			string deleteAcheterQuery = "DELETE FROM Acheter WHERE idProduitF IN (SELECT idProduit FROM Produit WHERE idCategorieF = @idCategorie)";
			using (SqlCommand deleteAcheterCommand = new SqlCommand(deleteAcheterQuery, connection))
			{
				deleteAcheterCommand.Parameters.AddWithValue("@idCategorie", id);
				deleteAcheterCommand.ExecuteNonQuery();
			}

			// Delete related records in the "Produit" table
			string deleteProduitQuery = "DELETE FROM Produit WHERE idCategorieF = @idCategorie";
			using (SqlCommand deleteProduitCommand = new SqlCommand(deleteProduitQuery, connection))
			{
				deleteProduitCommand.Parameters.AddWithValue("@idCategorie", id);
				deleteProduitCommand.ExecuteNonQuery();
			}

			// Delete the record in the "Categorie" table
			string deleteCategorieQuery = "DELETE FROM Categorie WHERE idCategorie = @idCategorie";
			using (SqlCommand deleteCategorieCommand = new SqlCommand(deleteCategorieQuery, connection))
			{
				deleteCategorieCommand.Parameters.AddWithValue("@idCategorie", id);
				deleteCategorieCommand.ExecuteNonQuery();
			}
		}

		return RedirectToAction("Index", "Categories");

	}
}
