using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;


namespace DashboardDataGatherer.Models
{
    public class MP2_DataBaseSettings
    {

        public MP2_DataBaseSettings()
        {
            OleDBConnection = new OleDbConnection(ConnectionString);
        }
        public static string ConnectionString { get { return System.Configuration.ConfigurationManager.ConnectionStrings["mp250db"].ConnectionString; } }
        public OleDbConnection OleDBConnection { get; set; }
    }

    public class QueryDefinitions
    {
        static System.Resources.ResourceManager objResourceManager = new System.Resources.ResourceManager("DashboardDataGatherer.Models.QueryDefs", System.Reflection.Assembly.GetExecutingAssembly());
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

    public static class SharedVariables
    {
        public static DateTime MINDATE = new DateTime(1900, 1, 1);
        public static DateTime MAXDATE = new DateTime(2999, 1, 1);
    }
}
