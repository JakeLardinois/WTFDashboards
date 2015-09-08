using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WTFDashboards.Models;


namespace WTFDashboards.Controllers
{
    public class InventoryController : Controller
    {
        //
        // GET: /Inventory/

        public ActionResult Index()
        {
            return View();
        }

        public FileResult CostByInventoryType()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.InventoryType, 10);
            InventoryCostData.AlignSeriesDataSetsByDate();
            InventoryCostData.CollectionDataName = "Current";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "Inventory By Source");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CostByWarehouse()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.Warehouse, 10);
            InventoryCostData.CollectionDataName = "Current";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "Inventory By Warehouse");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CostSourcesByWarehouse()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.None, 1);
            InventoryCostData.CollectionDataName = "Comparison";
            InventoryCostData.PopulateSeriesDataSetsByWarehouse();
            InventoryCostData.AlignSeriesDataSetsByInventoryType();
            var InventoryCostChart = new InventoryChart(InventoryChartType.CompareByWarehouse, InventoryCostData, "Warehouse Inventory By Type");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CostSourcesByInventoryType()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.None, 1);
            InventoryCostData.CollectionDataName = "Comparison";
            InventoryCostData.PopulateSeriesDataSetsByInventoryType();
            InventoryCostData.AlignSeriesDataSetsByWarehouse();
            var InventoryCostChart = new InventoryChart(InventoryChartType.CompareByInventoryType, InventoryCostData, "Type Inventory By Warehouse");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult WIPCosts()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.WIP, 10);
            InventoryCostData.CollectionDataName = "WIP";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "WIP");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult PurchasedInventoryCosts()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.Purchased, 10);
            InventoryCostData.CollectionDataName = "Purchased";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "Purchased Inventory");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult ManufacturedInventoryCosts()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.Manufactured, 10);
            InventoryCostData.CollectionDataName = "Manufactured";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "Manufactured Inventory");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }
    }
}
