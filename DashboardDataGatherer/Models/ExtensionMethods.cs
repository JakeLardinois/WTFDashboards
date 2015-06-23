using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public static class ExtensionMethods
    {
        public static string ToCommaSeparatedString(this List<Status> source)
        {
            return string.Join(",", source.SelectMany(s => s.ToString()).Distinct().ToArray());
        }

        //takes a comma separated string, trims and encloses the text with single quotes. This was made mainly for developing Queries for SQL IN clauses
        public static string AddSingleQuotes(this string source)
        {
            string[] strArray;
            StringBuilder objStrBldr;


            strArray = source.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length > 0)
            {
                objStrBldr = new StringBuilder();

                foreach (string strTemp in strArray)
                    objStrBldr.Append("'" + strTemp.Trim() + "',");

                return objStrBldr.Remove(objStrBldr.Length - 1, 1).ToString();
            }
            else
                return string.Empty;


        }
    }
}
