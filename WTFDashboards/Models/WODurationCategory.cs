using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTFDashboards.Models
{
    public class WODurationCategory
    {
        public decimal? AverageWODuration { get; set; }
        public string Category { get; set; }
        public int WOCount { get; set; }
    }
}