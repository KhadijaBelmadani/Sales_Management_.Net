using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace SiteDeVente.Models
{
	public partial class dbContext : DbContext
	{
		public dbContext()
			: base("name=dbContext")
		{
		}

		public virtual DbSet<Acheter> Acheters { get; set; }
		public virtual DbSet<Categorie> Categories { get; set; }
		public virtual DbSet<Favoriser> Favorisers { get; set; }
		public virtual DbSet<Produit> Produits { get; set; }
		public virtual DbSet<Signaler> Signalers { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Categorie>()
				.Property(e => e.nom)
				.IsUnicode(false);

			modelBuilder.Entity<Categorie>()
				.Property(e => e.attributsSpecifiques)
				.IsUnicode(false);

			modelBuilder.Entity<Categorie>()
				.Property(e => e.image)
				.IsUnicode(false);

			modelBuilder.Entity<Categorie>()
				.HasMany(e => e.Produits)
				.WithOptional(e => e.Categorie)
				.HasForeignKey(e => e.idCategorieF);

			modelBuilder.Entity<Produit>()
				.Property(e => e.nom)
				.IsUnicode(false);

			modelBuilder.Entity<Produit>()
				.Property(e => e.description)
				.IsUnicode(false);

			modelBuilder.Entity<Produit>()
				.Property(e => e.prix)
				.HasPrecision(18, 0);

			modelBuilder.Entity<Produit>()
				.Property(e => e.valeursSpecifiques)
				.IsUnicode(false);

			modelBuilder.Entity<Produit>()
				.Property(e => e.image)
				.IsUnicode(false);

			modelBuilder.Entity<Produit>()
				.HasMany(e => e.Acheters)
				.WithOptional(e => e.Produit)
				.HasForeignKey(e => e.idProduitF);

			modelBuilder.Entity<Produit>()
				.HasMany(e => e.Favorisers)
				.WithRequired(e => e.Produit)
				.HasForeignKey(e => e.idProduitF)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Signaler>()
				.Property(e => e.message)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.role)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.nomComplet)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.adresse)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.specialite)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.tel)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.email)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.password)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Acheters)
				.WithOptional(e => e.User)
				.HasForeignKey(e => e.idUserF);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Favorisers)
				.WithRequired(e => e.User)
				.HasForeignKey(e => e.idUserF)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Produits)
				.WithRequired(e => e.User)
				.HasForeignKey(e => e.idUserF)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Signalers)
				.WithOptional(e => e.User)
				.HasForeignKey(e => e.idUserSignale);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Signalers1)
            .WithOptional(e => e.User1)
				.HasForeignKey(e => e.idUserSignalant);

			//modelBuilder.Entity<IdentityRole>().HasData(
			//new IdentityRole { Name = "admin", NormalizedName = "ADMIN" },
			//new IdentityRole { Name = "proprietaire", NormalizedName = "PROPRIETAIRE" });

        }
	}
}
