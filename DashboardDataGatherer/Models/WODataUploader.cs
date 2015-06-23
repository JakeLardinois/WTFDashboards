using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public class WODataUploader
    {
        public void UploadWorkOrderData()
        {
            DashboardDbEntities db = new DashboardDbEntities();
            List<WODurationCategory> DurationCategories;
            var objQueryDefinitions = new QueryDefinitions();


            var objAnnualMP2ByCategory = new WOData(out DurationCategories, DateTime.Now.AddYears(-1).Date, DateTime.Now.Date);
            foreach (var objDurationCategory in DurationCategories)
                db.WorkOrderMetrics.Add(new WorkOrderMetric
                {
                    DateCreated = DateTime.Now.Date,
                    MetricType = (char)WOMetricType.AnnualWOAvgDaysOpenByCategory + string.Empty,
                    WOCategory = objDurationCategory.Category,
                    AverageWODuration = objDurationCategory.AverageWODuration,
                    WOCount = objDurationCategory.WOCount
                });
            db.Database.ExecuteSqlCommand(objQueryDefinitions.GetQuery("DeleteWOMetricByTypeAndDate", //Delete any work orders that may exist for this day to avoid duplicates...
                new string[] { (char)WOMetricType.AnnualWOAvgDaysOpenByCategory + string.Empty, DateTime.Now.Date.ToShortDateString() }));
            db.SaveChanges(); //Save the WOMetrics to the database...

            var objOpenReadyHoldMP2ByCategory = new WOData(out DurationCategories, new List<Status> { Status.Open, Status.Ready, Status.Hold });
            foreach (var objDurationCategory in DurationCategories)
                db.WorkOrderMetrics.Add(new WorkOrderMetric
                {
                    DateCreated = DateTime.Now.Date,
                    MetricType = (char)WOMetricType.CurrentWOAvgDaysOpenByCategory + string.Empty,
                    WOCategory = objDurationCategory.Category,
                    AverageWODuration = objDurationCategory.AverageWODuration,
                    WOCount = objDurationCategory.WOCount
                });
            db.Database.ExecuteSqlCommand(objQueryDefinitions.GetQuery("DeleteWOMetricByTypeAndDate",
                new string[] { (char)WOMetricType.CurrentWOAvgDaysOpenByCategory + string.Empty, DateTime.Now.Date.ToShortDateString() }));
            db.SaveChanges();
        }
    }
}
