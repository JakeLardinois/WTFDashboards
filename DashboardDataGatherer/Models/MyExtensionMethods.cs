using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public static class MyExtensionMethods
    {
        public static string ToCommaSeparatedString(this List<Status> source)
        {
            return string.Join(",", source.SelectMany(s => s.ToString()).Distinct().ToArray());
        }

    }
}
