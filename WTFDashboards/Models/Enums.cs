using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WTFDashboards.Models
{
    public enum WOMetricType
    {
        AnnualWOAvgDaysOpenByCategory = 'A', //Average Duration Open per Work Order over the past year
        CurrentWOAvgDaysOpenByCategory = 'C', //Average Duration Open per Work Order in the current bank of Work Orders
    }

    public enum WOChartType
    {
        Single = 0,
        Comparison = 1,
        Performance = 2
    }

    public enum InventoryCostGroup
    {
        None,
        Warehouse,
        InventoryType
    }

    public enum InventoryChartType
    {
        Single,
        CompareByWarehouse,
        CompareByInventoryType,
        Performance
    }
}