using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public class MP2
    {
        public int? AverageWODuration {
            get
            {
                var TotalDaysOpen = WorkOrders
                    .Sum(w => w.DaysOpen);

                var TotalWOs = WorkOrders
                    .Where(w => w.DaysOpen != null)
                    .Count();

                return TotalWOs == 0 ? null : TotalDaysOpen / TotalWOs;
            }
        }

        public IEnumerable<WO> WorkOrders { get; set; }


        public MP2(DateTime dtmStartDate, DateTime dtmEndDate)
        {
            using (var db = new mp250dbDB())
            {
                WorkOrders = db.WOes
                    .Where(w => w.REQUESTDATE >= dtmStartDate || w.COMPLETIONDATE >= dtmStartDate || w.COMPLETIONDATE == null)
                    .Where(w => w.REQUESTDATE <= dtmEndDate || w.COMPLETIONDATE <= dtmEndDate || w.COMPLETIONDATE == null)
                    .ToList();
            }
        }

        public MP2(out List<DurationCategory> DurationCategories, DateTime dtmStartDate, DateTime dtmEndDate)
        {
            using (var db = new mp250dbDB())
            {
                WorkOrders = db.WOes
                    .Where(w => w.REQUESTDATE >= dtmStartDate || w.COMPLETIONDATE >= dtmStartDate || w.COMPLETIONDATE == null)
                    .Where(w => w.REQUESTDATE <= dtmEndDate || w.COMPLETIONDATE <= dtmEndDate || w.COMPLETIONDATE == null)
                    .ToList();
            }

            DurationCategories = SegmentByWOType();
        }

        public MP2(List<Status> StatusList)
        {
            using (var db = new mp250dbDB())
            {
                var objStatusArray = StatusList.ToCommaSeparatedString().Split(',');
                WorkOrders = db.WOes
                    .Where(w => objStatusArray.Contains(w.STATUS + string.Empty))
                    .ToList();
            }
        }

        public MP2(out List<DurationCategory> DurationCategories, List<Status> StatusList)
        {
            using (var db = new mp250dbDB())
            {
                var objStatusArray = StatusList.ToCommaSeparatedString().Split(',');
                WorkOrders = db.WOes
                    .Where(w => objStatusArray.Contains(w.STATUS + string.Empty))
                    .ToList();
            }
            DurationCategories = SegmentByWOType();
        }

        private List<DurationCategory> SegmentByWOType()
        {
            var DurationCategories = new List<DurationCategory>();
            var categories = WorkOrders
                .GroupBy(w => string.IsNullOrEmpty(w.WOTYPE) ? w.WOTYPE : w.WOTYPE.ToUpper())
                .Select(w => new { w.Key });

            var objTempWorkOrders = WorkOrders;
            foreach (var objCategory in categories)
            {
                if (string.IsNullOrEmpty(objCategory.Key))
                    WorkOrders = objTempWorkOrders
                        .Where(w => string.IsNullOrEmpty(w.WOTYPE));
                else
                    WorkOrders = objTempWorkOrders
                        .Where(w => !string.IsNullOrEmpty(w.WOTYPE))
                        .Where(w => w.WOTYPE.ToUpper().Equals(objCategory.Key.ToUpper()));
                
                if (WorkOrders.Count() > 0)
                    DurationCategories.Add(new DurationCategory { Category = objCategory.Key, AverageWODuration = this.AverageWODuration, WOCount = WorkOrders.Count() });
            }
            WorkOrders = objTempWorkOrders;

            return DurationCategories;
        }
    }



    public class DurationCategory
    {
        public int? AverageWODuration { get; set; }
        public string Category { get; set; }
        public int WOCount { get; set; }
    }
}
