using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public class WODurationCategory
    {
        public decimal? AverageWODuration { get; set; }
        public string Category { get; set; }
        public int WOCount { get; set; }
    }
}
