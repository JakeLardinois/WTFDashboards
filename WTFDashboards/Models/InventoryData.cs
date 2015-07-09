using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTFDashboards.Models
{
    public class InventoryData
    {
        private InventoryCostGroup mobjInventoryCostGroup { get; set; }
        public List<string> mobjInventoryTypes { get; set; }
        public List<string> mobjWarehouses { get; set; }

        public List<InventoryCostMetric> CollectionData { get; set; }

        public List<List<InventoryCostMetric>> SeriesDataSets { get; set; }
        public string CollectionDataName { get; set; }
        public List<string> InventoryTypes { get { return mobjInventoryTypes; } }
        public List<string> Warehouses { get { return mobjWarehouses; } }
        public InventoryCostGroup InventoryCostGroup { get { return mobjInventoryCostGroup; } }


        public InventoryData(InventoryCostGroup objInventoryCostGroup, int RecordSetCount)
        {
            SeriesDataSets = new List<List<InventoryCostMetric>>();
            mobjInventoryCostGroup = objInventoryCostGroup;

            var db = new DashboardDbEntities();
            var objQueryDefs = new QueryDefinitions();
            var strSQL = objQueryDefs.GetQuery("SelectTopNInventoryCosts", new string[] { RecordSetCount.ToString() });

            var CostMetrics = db.Database
                .SqlQuery<InventoryCostMetric>(strSQL);


            mobjInventoryTypes = CostMetrics
                .GroupBy(w => w.InventoryType)
                .Select(w => w.Key)
                .ToList();
            mobjWarehouses = CostMetrics
                .GroupBy(w => w.Warehouse)
                .Select(w => w.Key)
                .ToList();

            switch (InventoryCostGroup)
            {
                case InventoryCostGroup.InventoryType:
                    GroupCollectionDataByInventoryType(CostMetrics);
                    PopulateSeriesDataSetsByInventoryType();
                    break;
                case InventoryCostGroup.Warehouse:
                    GroupCollectionDataByWarehouse(CostMetrics);
                    PopulateSeriesDataSetsByWarehouse();
                    break;
                case InventoryCostGroup.Purchased:
                    GetCollectionDataByInventoryType(CostMetrics, "Purchased");
                    break;
                case InventoryCostGroup.Manufactured:
                    GetCollectionDataByInventoryType(CostMetrics, "Manufactured");
                    break;
                case InventoryCostGroup.WIP:
                    GetCollectionDataByInventoryType(CostMetrics, "WIP");
                    break;
                case InventoryCostGroup.None:
                    GetCollectionData(CostMetrics);
                    break;
            }

        }

        private void GetCollectionData(IEnumerable<InventoryCostMetric> objCostMetrics)
        {
            CollectionData = objCostMetrics
                .ToList();
        }

        private void GetCollectionDataByInventoryType(IEnumerable<InventoryCostMetric> objCostMetrics, string InventoryType)
        {
            CollectionData = objCostMetrics
                .Where(t => t.InventoryType.ToUpper().Equals(InventoryType.ToUpper()))
                .GroupBy(t => new { t.DateCreated })
                .Select(t => new InventoryCostMetric { DateCreated = t.Key.DateCreated, Cost = t.Sum(c => c.Cost)})
                .OrderBy(c => c.DateCreated)
                .ToList();
        }

        private void GroupCollectionDataByWarehouse(IEnumerable<InventoryCostMetric> objCostMetrics)
        {
            CollectionData = objCostMetrics
                .GroupBy(c => new { c.Warehouse, c.DateCreated })
                .Select(c => new InventoryCostMetric { Warehouse = c.Key.Warehouse, DateCreated = c.Key.DateCreated, Cost = c.Sum(m => m.Cost) })
                .OrderBy(c => c.DateCreated)
                .ToList();
        }

        public void PopulateSeriesDataSetsByWarehouse()
        {
            foreach (var Warehouse in Warehouses)
                SeriesDataSets
                    .Add(CollectionData
                        .Where(w => w.Warehouse == Warehouse)
                        .ToList());
        }

        public void AlignSeriesDataSetsByInventoryType()
        {
            List<InventoryCostMetric> tempCollectionData;
            var tempSeriesCollection = new List<List<InventoryCostMetric>>();

            foreach (var objCollection in SeriesDataSets)
            {
                tempCollectionData = new List<InventoryCostMetric>();
                foreach (var InventoryType in InventoryTypes)
                {
                    tempCollectionData
                        .Add(objCollection.
                        Where(w => w.InventoryType == InventoryType)
                        .DefaultIfEmpty(new InventoryCostMetric { InventoryType = InventoryType, Warehouse = objCollection[0].Warehouse, Cost = 0 })
                        .SingleOrDefault());
                }
                tempSeriesCollection.Add(tempCollectionData);
            }

            SeriesDataSets = tempSeriesCollection;
        }


        private void GroupCollectionDataByInventoryType(IEnumerable<InventoryCostMetric> objCostMetrics)
        {
            var db = new DashboardDbEntities();

            CollectionData = objCostMetrics
                .GroupBy(c => new { c.InventoryType, c.DateCreated })
                .Select(c => new InventoryCostMetric { InventoryType = c.Key.InventoryType, DateCreated = c.Key.DateCreated, Cost = c.Sum(m => m.Cost) })
                .OrderBy(c => c.DateCreated)
                .ToList();
        }

        public void PopulateSeriesDataSetsByInventoryType()
        {
            foreach (var InventoryType in InventoryTypes)
                SeriesDataSets
                    .Add(CollectionData
                        .Where(w => w.InventoryType == InventoryType)
                        .ToList());
        }

        public void AlignSeriesDataSetsByWarehouse()
        {
            List<InventoryCostMetric> tempCollectionData;
            var tempSeriesCollection = new List<List<InventoryCostMetric>>();

            foreach (var objCollection in SeriesDataSets)
            {
                tempCollectionData = new List<InventoryCostMetric>();
                foreach (var Warehouse in Warehouses)
                {
                    tempCollectionData
                        .Add(objCollection.
                        Where(w => w.Warehouse == Warehouse)
                        .DefaultIfEmpty(new InventoryCostMetric { Warehouse = Warehouse, InventoryType = objCollection[0].InventoryType, Cost = 0 })
                        .SingleOrDefault());
                }
                tempSeriesCollection.Add(tempCollectionData);
            }

            SeriesDataSets = tempSeriesCollection;
        }


        public void AlignSeriesDataSetsByDate()
        {
            List<DateTime> Dates;
            List<InventoryCostMetric> tempCollectionData;
            var tempSeriesCollection = new List<List<InventoryCostMetric>>();

            Dates = new List<DateTime>();
            foreach (var objCollection in SeriesDataSets)
            {
                Dates = Dates.Union(objCollection
                .GroupBy(w => w.DateCreated.Date)
                .Select(w => w.Key)
                .ToList())
                .ToList();
            }

            foreach (var objCollection in SeriesDataSets)
            {
                tempCollectionData = new List<InventoryCostMetric>();
                foreach (var Date in Dates)
                {
                    tempCollectionData
                        .Add(objCollection.
                        Where(w => w.DateCreated.Date == Date.Date)
                        .DefaultIfEmpty(new InventoryCostMetric { DateCreated = Date, InventoryType = objCollection[0].InventoryType, Cost = 0 })
                        .SingleOrDefault());
                }
                tempSeriesCollection.Add(tempCollectionData);
            }

            SeriesDataSets = tempSeriesCollection;
        }
    }
}