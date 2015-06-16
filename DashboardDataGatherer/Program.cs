using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DashboardDataGatherer.Models;


namespace DashboardDataGatherer
{
    class Program
    {
        static void Main(string[] args)
        {
            DashboardDbEntities db = new DashboardDbEntities();
            List<DurationCategory> DurationCategories;


            var objAnnualMP2ByCategory = new WorkOrderData(out DurationCategories, DateTime.Now.AddYears(-1).Date, DateTime.Now.Date);
            foreach (var objDurationCategory in DurationCategories)
                db.WorkOrderMetrics.Add(new WorkOrderMetric
                {
                    DateCreated = DateTime.Now.Date,
                    MetricType = (char)WOMetricType.AnnualWOAvgDaysOpenByCategory + string.Empty,
                    WOCategory = objDurationCategory.Category,
                    AverageWODuration = objDurationCategory.AverageWODuration,
                    WOCount = objDurationCategory.WOCount
                });
            db.Database.ExecuteSqlCommand(QueryDefinitions.GetQuery("DeleteWOMetricByTypeAndDate", //Delete any work orders that may exist for this day to avoid duplicates...
                new string[] { (char)WOMetricType.AnnualWOAvgDaysOpenByCategory + string.Empty, DateTime.Now.Date.ToShortDateString()}));
            db.SaveChanges(); //Save the WOMetrics to the database...

            var objOpenReadyHoldMP2ByCategory = new WorkOrderData(out DurationCategories, new List<Status> { Status.Open, Status.Ready, Status.Hold });
            foreach (var objDurationCategory in DurationCategories)
                db.WorkOrderMetrics.Add(new WorkOrderMetric
                {
                    DateCreated = DateTime.Now.Date,
                    MetricType = (char)WOMetricType.CurrentWOAvgDaysOpenByCategory + string.Empty,
                    WOCategory = objDurationCategory.Category,
                    AverageWODuration = objDurationCategory.AverageWODuration,
                    WOCount = objDurationCategory.WOCount
                });
            db.Database.ExecuteSqlCommand(QueryDefinitions.GetQuery("DeleteWOMetricByTypeAndDate",
                new string[] { (char)WOMetricType.CurrentWOAvgDaysOpenByCategory + string.Empty, DateTime.Now.Date.ToShortDateString() }));
            db.SaveChanges();

            Console.WriteLine("Successfully Completed!");
            Console.ReadLine();
        }
    }
}
