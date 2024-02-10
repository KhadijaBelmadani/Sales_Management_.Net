namespace SiteDeVente.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Produit")]
    public partial class Produit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produit()
        {
            Acheters = new HashSet<Acheter>();
            Favorisers = new HashSet<Favoriser>();
        }

        [Key]
        public int idProduit { get; set; }

        [StringLength(50)]
        public string nom { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        public decimal? prix { get; set; }

        public int? pourcentageSolde { get; set; }

        public int? quantite { get; set; }

        [StringLength(100)]
        public string valeursSpecifiques { get; set; }

        public int? idCategorieF { get; set; }

        [DisplayName("Image")]
        public string image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }


        public int idUserF { get; set; }

        public DateTime? date { get; set; }
		[NotMapped] // Cette propriété n'est pas mappée vers la base de données
		public decimal? prixNouveau => (prix != null && pourcentageSolde != null) ? prix - (prix * pourcentageSolde / 100) : prix;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Acheter> Acheters { get; set; }

        public virtual Categorie Categorie { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favoriser> Favorisers { get; set; }

        public virtual User User { get; set; }
    }
}
