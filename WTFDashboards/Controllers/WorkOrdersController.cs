using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WTFDashboards.Models;


namespace WTFDashboards.Controllers
{
    public class WorkOrdersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileResult AnnualWOAverages()
        {
            var AnnualWOData = new WOData(WOMetricType.AnnualWOAvgDaysOpenByCategory, 1);
            AnnualWOData.CollectionDataName = "History";
            var WOChart = new WOChart(WOChartType.Single, AnnualWOData, "Annual WO Avg Days Open By Category");

            return (File(WOChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CurrentWOAverages()
        {
            var CurrentWOData = new WOData(WOMetricType.CurrentWOAvgDaysOpenByCategory, 1);
            CurrentWOData.CollectionDataName = "Current";
            var WOChart = new WOChart(WOChartType.Single, CurrentWOData, "Current WO Avg Days Open By Category");

            return (File(WOChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult WOAvgCurrentToAnnualComparison()
        {
            var CurrentWOData = new WOData(WOMetricType.CurrentWOAvgDaysOpenByCategory, 1);
            var AnnualWOData = new WOData(WOMetricType.AnnualWOAvgDaysOpenByCategory, 1);
            var CombinedData = new WOData();

            CombinedData.CollectionData.AddRange(CurrentWOData.CollectionData);
            CombinedData.CollectionDataName = "Current"; //displays on the legend...
            CombinedData.CollectionComparisonData.AddRange(AnnualWOData.CollectionData);
            CombinedData.ComparisonDataName = "History";
            CombinedData.AlignComparisonDataSets();

            var WOChart = new WOChart(WOChartType.Comparison, CombinedData, "Current to History WO Avg Days Open Comparison");

            return (File(WOChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult WOAvgAnnualToCurrentComparison()
        {
            var CurrentWOData = new WOData(WOMetricType.CurrentWOAvgDaysOpenByCategory, 1);
            var AnnualWOData = new WOData(WOMetricType.AnnualWOAvgDaysOpenByCategory, 1);
            var CombinedData = new WOData();

            CombinedData.CollectionData.AddRange(AnnualWOData.CollectionData);
            CombinedData.CollectionDataName = "History";
            CombinedData.CollectionComparisonData.AddRange(CurrentWOData.CollectionData);
            CombinedData.ComparisonDataName = "Current";
            CombinedData.AlignComparisonDataSets();

            var WOChart = new WOChart(WOChartType.Comparison, CombinedData, "History to Current WO Avg Days Open Comparison");

            return (File(WOChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CurrentWOAvgPerformance()
        {
            var CurrentWOData = new WOData(WOMetricType.CurrentWOAvgDaysOpenByCategory, 10, true);
            CurrentWOData.AlignSeriesDataSetsByDate();
            var WOChart = new WOChart(WOChartType.Performance, CurrentWOData, "Current WO Avg Days Open Performance");

            return (File(WOChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult AnnualWOAvgPerformance()
        {
            var AnnualWOData = new WOData(WOMetricType.AnnualWOAvgDaysOpenByCategory, 10, true);
            AnnualWOData.AlignSeriesDataSetsByDate();
            var WOChart = new WOChart(WOChartType.Performance, AnnualWOData, "Annual WO Avg Days Open Performance");

            return (File(WOChart.chartStream.GetBuffer(), @"image/png"));
        }
    }
}
