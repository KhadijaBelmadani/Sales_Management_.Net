namespace SiteDeVente.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;
    using System.Web.Mvc;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Acheters = new HashSet<Acheter>();
            Favorisers = new HashSet<Favoriser>();
            Produits = new HashSet<Produit>();
            Signalers = new HashSet<Signaler>();
            Signalers1 = new HashSet<Signaler>();
        }
		
		public int id { get; set; }

        [StringLength(50)]
        public string role { get; set; }

		[Required(ErrorMessage = "Le champ Nom complet est requis.")]
		[StringLength(50)]
        public string nomComplet { get; set; }

		[DisplayName("Image")]
		public string image { get; set; }
		[NotMapped]
		public HttpPostedFileBase ImageFile { get; set; }
		[Required(ErrorMessage = "Le champ Adress  est requis.")]

		[StringLength(50)]
        public string adresse { get; set; }
		

		[StringLength(50)]
        public string specialite { get; set; }
		[Required(ErrorMessage = "Le champ tel est requis.")]

		[StringLength(50)]

        public string tel { get; set; }
		[Required(ErrorMessage = "Le champ Email est requis.")]
		[EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide.")]
		[Remote("IsEmailUnique", "Validation", HttpMethod = "POST", ErrorMessage = "Cet email est déjà utilisé.")]

		[StringLength(50)]
        public string email { get; set; }

		[Required(ErrorMessage = "Le champ Mot de passe est requis.")]

		[StringLength(50)]
        public string password { get; set; }
       
        public DateTime? date { get; set; }
        public int? favoris { get; set; }

        public int? blackList { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acheter> Acheters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favoriser> Favorisers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Produit> Produits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Signaler> Signalers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Signaler> Signalers1 { get; set; }
    }
}
