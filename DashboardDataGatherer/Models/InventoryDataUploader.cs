using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public class InventoryDataUploader
    {
        public InventoryDataUploader(List<InventoryCostMetric> InventoryCostMetrics)
        {
            var db = new DashboardDbEntities();
            foreach (var objData in InventoryCostMetrics)
                db.InventoryCostMetrics.Add(objData);

            db.SaveChanges();
        }
    }
}
