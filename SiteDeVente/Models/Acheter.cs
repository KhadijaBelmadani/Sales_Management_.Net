namespace SiteDeVente.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Acheter")]
    public partial class Acheter
    {
        public int Id { get; set; }

        public int? idUserF { get; set; }

        public int? idProduitF { get; set; }

        public int? quantite { get; set; }

        public DateTime? date { get; set; }

        public virtual User User { get; set; }

        public virtual Produit Produit { get; set; }
    }
}
