using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteDeVente.Models
{
    public class HistoryViewModel
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
    }
}