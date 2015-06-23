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
            //Uploads the MP2 Work Order Data
            //var objWODataUploader = new WODataUploader();
            //objWODataUploader.UploadWorkOrderData();

            //Utilizes Sytelines Stored Procedure to retrieve inventory Costs on an Item Level
            //var objInventoryCostItems = new InventoryCostItems(InventoryCostSource.SytelineItemWhse);
            //var objInventoryCostItems = new InventoryCostItems(InventoryCostSource.SytelineCostReport);
            var objInventoryCostItems = new InventoryCostItems(InventoryCostSource.FactTrak, new DateTime(2015, 6, 15), new DateTime(2015, 6, 15));
            
            var InventoryDataUploader = new InventoryDataUploader(objInventoryCostItems.InventoryCostMetrics);


            Console.WriteLine("Successfully Completed!");
            Console.ReadLine();
        }
    }
}
