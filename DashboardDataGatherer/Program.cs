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
            WODataUploader objWODataUploader;
            InventoryCostItems objInventoryCostItems;
            InventoryDataUploader objInventoryDataUploader;
            JobMachineTransactions objJobMachineTransactions;


            if (args.Length > 0)
            {
                string strArgs = string.Join(" ", args).Remove(0, 1);//joins the initial argument array and removes the first space, else the space is also split into an argument
                string[] objArgs = strArgs.Split('/');

                for (int x = 0; x < objArgs.Length; x++)//loop through all the arguments
                {
                    switch (objArgs[x].ToUpper()[0])//examine the first character of the argument
                    {
                        case '?':
                            DisplayUsage();
                            Environment.Exit(0);
                            break;
                        case 'i':
                        case 'I':

                            //Utilizes Sytelines Stored Procedure to retrieve inventory Costs on an Item Level
                            //objInventoryCostItems = new InventoryCostItems(InventoryCostSource.SytelineCostReport);

                            //Utilizes the Fact-Trak database to retrieve inventory Costs on an Item level for a specific Day or range of Days 
                            //objInventoryCostItems = new InventoryCostItems(InventoryCostSource.FactTrak, new DateTime(2015, 1, 1), new DateTime(2015, 1, 1));

                            //Gets the current inventory costs based on Syteline's ItemWhse table
                            objInventoryCostItems = new InventoryCostItems(InventoryCostSource.SytelineItemWhse);
                            objInventoryDataUploader = new InventoryDataUploader(objInventoryCostItems.InventoryCostMetrics);
                            break;
                        case 'w':
                        case 'W'://Gathers & Uploads the MP2 Work Order Data

                            objWODataUploader = new WODataUploader();
                            objWODataUploader.UploadWorkOrderData();
                            break;
                        case 'm':
                        case 'M': //Gathers & Uploads Machine hours and absorbed Overhead per WC per Day...

                            objJobMachineTransactions = new JobMachineTransactions(DateTime.Now, 365);
                            objJobMachineTransactions.UploadNewData();
                            break;
                        default:
                            Console.WriteLine("Invalid Parameter!!");
                            DisplayUsage();
                            break;
                    }
                }
            }
            else { //Gathers & Uploads all metrics since no parameter is passed...
                //Uploads the MP2 Work Order Data
                objWODataUploader = new WODataUploader();
                objWODataUploader.UploadWorkOrderData();

                //Gathers and Uploads Inventory Cost Data
                objInventoryCostItems = new InventoryCostItems(InventoryCostSource.SytelineItemWhse);
                objInventoryDataUploader = new InventoryDataUploader(objInventoryCostItems.InventoryCostMetrics);

                //Gathers & Uploads Machine hours and absorbed Overhead per WC
                objJobMachineTransactions = new JobMachineTransactions(DateTime.Now, 365);
                objJobMachineTransactions.UploadNewData();
            }
        }

        private static void DisplayUsage()
        {
            StringBuilder objStrBldr = new StringBuilder();

            objStrBldr.AppendLine("To gather and upload the current Inventory Metrics");
            objStrBldr.AppendLine("DashboardDataGatherer.exe /I");
            objStrBldr.AppendLine("");
            objStrBldr.AppendLine("To gather and upload the current Work Order Metrics");
            objStrBldr.AppendLine("DashboardDataGatherer.exe /W");
            objStrBldr.AppendLine("");
            objStrBldr.AppendLine("To gather and upload the Machine Metrics.");
            objStrBldr.AppendLine("/tThese Metrics include Machine Hours as well as Fixed and Variable Overhead absorbed by the equipment");
            objStrBldr.AppendLine("DashboardDataGatherer.exe /M");
            objStrBldr.AppendLine("");
            objStrBldr.AppendLine("To gather and upload all of the Metrics");
            objStrBldr.AppendLine("DashboardDataGatherer.exe");

            Console.WriteLine(objStrBldr.ToString());
            //Console.ReadLine();
        }
    }
}
