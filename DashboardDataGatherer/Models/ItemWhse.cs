using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public class ItemWhse
    {
        public string item { get; set; }
        public string whse { get; set; }
        public Nullable<decimal> qty_sold_ytd { get; set; }
        public Nullable<decimal> qty_pur_ytd { get; set; }
    }
}
