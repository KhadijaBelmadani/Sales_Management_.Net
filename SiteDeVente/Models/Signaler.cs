namespace SiteDeVente.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Signaler")]
    public partial class Signaler
    {
        public int Id { get; set; }

        public int? idUserSignale { get; set; }

        public int? idUserSignalant { get; set; }

        public DateTime? date { get; set; }

        public string message { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
