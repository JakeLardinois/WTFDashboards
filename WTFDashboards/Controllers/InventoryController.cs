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
            InventoryCostData.CollectionDataName = "History";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "Inventory Cost By Source");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CostByWarehouse()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.Warehouse, 10);
            InventoryCostData.CollectionDataName = "History";
            var InventoryCostChart = new InventoryChart(InventoryChartType.Performance, InventoryCostData, "Inventory Cost By Warehouse");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CostSourcesByWarehouse()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.None, 1);
            InventoryCostData.CollectionDataName = "Comparison";
            InventoryCostData.PopulateSeriesDataSetsByWarehouse();
            InventoryCostData.AlignSeriesDataSetsByInventoryType();
            var InventoryCostChart = new InventoryChart(InventoryChartType.CompareByWarehouse, InventoryCostData, "Warehouse Inventory Cost By Type");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult CostSourcesByInventoryType()
        {
            var InventoryCostData = new InventoryData(InventoryCostGroup.None, 1);
            InventoryCostData.CollectionDataName = "Comparison";
            InventoryCostData.PopulateSeriesDataSetsByInventoryType();
            InventoryCostData.AlignSeriesDataSetsByWarehouse();
            var InventoryCostChart = new InventoryChart(InventoryChartType.CompareByInventoryType, InventoryCostData, "Type Inventory Cost By Warehouse");

            return (File(InventoryCostChart.chartStream.GetBuffer(), @"image/png"));
        }

        public FileResult WIPCosts()
        {
            return File("Temp", @"image/png");
        }

        public FileResult PurchasedInventoryCosts()
        {
            return File("Temp", @"image/png");
        }

        public FileResult ManufacturedInventoryCosts()
        {
            return File("Temp", @"image/png");
        }
    }
}
