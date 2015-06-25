using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;
using System.Drawing;
using System.Reflection;

namespace WTFDashboards.Models
{
    public static class Settings
    {
        public static List<Color> ColorStructToList()
        {
            return typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                .Select(c => (Color)c.GetValue(null, null))
                                .ToList();
        }
    }

    public class QueryDefinitions
    {
        static System.Resources.ResourceManager objResourceManager = new System.Resources.ResourceManager("WTFDashboards.Models.QueryDefs", System.Reflection.Assembly.GetExecutingAssembly());
        private StringBuilder strSQL = new StringBuilder();

        public string GetQuery(string strQueryName)
        {
            return objResourceManager.GetString(strQueryName);
        }

        public string GetQuery(string strQueryName, string[] strParams)
        {

            strSQL.Clear();
            strSQL.Append(objResourceManager.GetString(strQueryName));

            for (int intCounter = strParams.Length - 1; intCounter > -1; intCounter--)
            {
                string strTemp = "~p" + intCounter;
                strSQL.Replace(strTemp, strParams[intCounter]);
            }

            return strSQL.ToString();
        }
    }
}