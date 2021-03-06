﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTFDashboards.Models
{
    public class WOData
    {
        public List<WorkOrderMetric> CollectionData { get; set; }
        public List<WorkOrderMetric> CollectionComparisonData { get; set; }
        public List<List<WorkOrderMetric>> SeriesDataSets { get; set; }
        public string CollectionDataName { get; set; }
        public string ComparisonDataName { get; set; }

        public WOData()
        {
            CollectionData = new List<WorkOrderMetric>();
            CollectionComparisonData = new List<WorkOrderMetric>();
            SeriesDataSets = new List<List<WorkOrderMetric>>();
        }

        public WOData(WOMetricType WOMetric, int RecordSetCount, bool PopulateSeriesDataSets = false)
        {
            var db = new DashboardDbEntities();

            SeriesDataSets = new List<List<WorkOrderMetric>>();
            CollectionComparisonData = new List<WorkOrderMetric>();

            var objQueryDefs = new QueryDefinitions();
            var strSQL = objQueryDefs.GetQuery("SelectTopNWOAvgsByCategory", new string[] { ((char)WOMetric).ToString(), RecordSetCount.ToString() });
            CollectionData = db.Database
                .SqlQuery<WorkOrderMetric>(strSQL)
                .ToList();

            if (PopulateSeriesDataSets)
                PopulateSeriesDataSetsByCategory();
        }

        public void AlignComparisonDataSets()
        {
            var Categories = CollectionData
                .GroupBy(w => w.WOCategory)
                .Select(w => w.Key)
                .ToList()
                .Union(CollectionComparisonData
                    .GroupBy(w => w.WOCategory)
                    .Select(w => w.Key)
                    .ToList());

            var tempCollectionData = new List<WorkOrderMetric>();
            var tempCollectionComparisonData = new List<WorkOrderMetric>();
            foreach (var Category in Categories)
            {
                tempCollectionData
                    .Add(CollectionData.
                        Where(w => w.WOCategory == Category)
                        .DefaultIfEmpty(new WorkOrderMetric { WOCategory = Category, AverageWODuration = 0, WOCount = 0 })
                        .SingleOrDefault());
                tempCollectionComparisonData
                    .Add(CollectionComparisonData.
                        Where(w => w.WOCategory == Category)
                        .DefaultIfEmpty(new WorkOrderMetric { WOCategory = Category, AverageWODuration = 0, WOCount = 0 })
                        .SingleOrDefault());
            }
            CollectionData = tempCollectionData;
            CollectionComparisonData = tempCollectionComparisonData;
        }

        private void PopulateSeriesDataSetsByCategory()
        {
            var Categories = CollectionData.GroupBy(w => w.WOCategory)
                        .Select(w => w.Key)
                        .ToList();
            //GroupList.Add(string.Empty);

            foreach (var objCategory in Categories)
            {
                SeriesDataSets
                    .Add(CollectionData
                        .Where(w => w.WOCategory == objCategory)
                        .ToList());
            }
        }

        public void AlignSeriesDataSetsByDate()
        {
            List<DateTime> Dates;
            List<WorkOrderMetric> tempCollectionData;
            var tempSeriesCollection = new List<List<WorkOrderMetric>>();

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
                tempCollectionData = new List<WorkOrderMetric>();
                foreach (var Date in Dates)
                {
                    tempCollectionData
                        .Add(objCollection.
                        Where(w => w.DateCreated.Date == Date.Date)
                        .DefaultIfEmpty(new WorkOrderMetric { DateCreated = Date, WOCategory = objCollection[0].WOCategory, MetricType = objCollection[0].MetricType,
                            WOCount = 0, AverageWODuration = 0})
                        .SingleOrDefault());
                }
                tempSeriesCollection.Add(tempCollectionData);
            }

            SeriesDataSets = tempSeriesCollection;
        }
    }
}