using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;


namespace WTFDashboards.Models
{
    public static class ExtensionMethods
    {
        public static DataTable ToDataTable(this List<WorkOrderMetric> source)
        {
            DataTable objDataTable = new DataTable();
            DataRow objRow;
            Type objType = typeof(WorkOrderMetric);
            System.Reflection.PropertyInfo[] objPropertyInfo = objType.GetProperties();

            objDataTable.TableName = "WorkOrderMetrics";
            foreach (System.Reflection.PropertyInfo objInfo in objPropertyInfo)
                objDataTable.Columns.Add(new DataColumn(objInfo.Name, objInfo.PropertyType));


            foreach (WorkOrderMetric objWorkOrderMetric in source)
            {
                objRow = objDataTable.NewRow();
                foreach (DataColumn objTempColumn in objDataTable.Columns)
                    objRow[objTempColumn] = objWorkOrderMetric.GetType()
                        .InvokeMember(objTempColumn.ColumnName, System.Reflection.BindingFlags.GetProperty, null, objWorkOrderMetric, null);
                objDataTable.Rows.Add(objRow);
            }
            return objDataTable;
        }
    }
}