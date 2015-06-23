using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;


namespace DashboardDataGatherer.Models
{
    public class InventoryCost
    {
        public string whse { get; set; }
        public string loc { get; set; }
        public string lot { get; set; }
        public string status { get; set; }
        public string item { get; set; }
        public decimal qty_on_hand { get; set; }
        public Nullable<decimal> cpr_cost { get; set; }
        public decimal cpr_price { get; set; }
        public Nullable<decimal> amt_cost { get; set; }
        public decimal amt_price { get; set; }
        public string item_desc { get; set; }
        public Int16 item_stocked { get; set; }
        public string type_desc { get; set; }
        public string method { get; set; }
        public string u_m { get; set; }
        public string u_m_desc { get; set; }
        public string pmt_code { get; set; }
        public string product_code { get; set; }
        public string cost_type { get; set; }
        public Nullable<decimal> matl_cost { get; set; }
        public Nullable<decimal> lbr_cost { get; set; }
        public Nullable<decimal> fovhd_cost { get; set; }
        public Nullable<decimal> vovhd_cost { get; set; }
        public Nullable<decimal> out_cost { get; set; }
        public string CostPriceFormat { get; set; }
        public byte CostPricePlaces { get; set; }

        //public decimal? CurrentUnitPrice { get; set; }
        //public decimal? TotalPrice { get { return qty_on_hand * CurrentUnitPrice; } }
    }

    public class InventoryCostItem
    {
        private string mSource { get; set; }

        public string Warehouse { get; set; }
        public string Location { get; set; }
        public string Item { get; set; }
        public decimal QtyOnHand { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public decimal TotalPrice { get; set; }
        public string ItemDescription { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Source
        {
            get
            {
                if (!string.IsNullOrEmpty(mSource))
                    switch (mSource.Trim().ToUpper())
                    {
                        case "M":
                            return "Manufactured";
                        case "P":
                            return "Purchased";
                        case "T":
                            return "Transferred";
                        default:
                            return mSource;
                    }
                else
                    return mSource;
            }
            set { mSource = value; }
        }
        public string ProductCode { get; set; }
        public Nullable<decimal> TotalMaterialCost { get; set; }
        public Nullable<decimal> TotalLaborCost { get; set; }
        public Nullable<decimal> FixedOvhdCost { get; set; }
        public Nullable<decimal> VariableOvhdCost { get; set; }
        public Nullable<decimal> OutsideCost { get; set; }
        public Nullable<decimal> QtyPurchasedYTD { get; set; }
        public Nullable<decimal> QtyManufacturedYTD { get; set; }
        public Nullable<decimal> QtyUsedYTD { get; set; }
        public Nullable<decimal> QtySoldYTD { get; set; }
        public Nullable<System.DateTime> LastTransactionDate { get; set; }

        public DateTime DateCreated { get; set; }
    }

    public class FactTrakCostItem
    {
        private string mPurch_Manuf { get; set; }

        public string Warehouse { get; set; }
        public string Item_Nbr { get; set; }
        public Nullable<decimal> Whse_Qty_On_Hand { get; set; }
        public Nullable<decimal> Whse_Inv_Value { get; set; }
        public DateTime Record_Date { get; set; }
        public string Purch_Manuf
        {
            get {
                if (!string.IsNullOrEmpty(mPurch_Manuf))
                    switch (mPurch_Manuf.Trim().ToUpper())
                    {
                        case "MFG":
                            return "Manufactured";
                        case "PUR":
                            return "Purchased";
                        default:
                            return mPurch_Manuf;
                    }
                else
                    return mPurch_Manuf;
            }
            set { mPurch_Manuf = value; }
        }
    }

    public class WIPCostItem
    {
        public string whse { get; set; }
        public string item { get; set; }
        public Nullable<decimal> WIP_Remaining { get; set; }
    }

    public class InventoryCostItems : List<InventoryCostItem>
    {
        private StringBuilder mStrBldr { get; set; }
        private SytelineDbEntities mSyteLineDb { get; set; }
        private QueryDefinitions mQueyrDefs { get; set; }
        public List<InventoryCostMetric> InventoryCostMetrics { get; set; }


        //dtmStartDate and dtmEndDate only need to be populated when the CostSourceType is FactTrak
        public InventoryCostItems(InventoryCostSource CostSource, DateTime? dtmStartDate = null, DateTime? dtmEndDate = null)
            : base()
        {
            mSyteLineDb = new SytelineDbEntities();
            mStrBldr = new StringBuilder();
            mQueyrDefs = new QueryDefinitions();


            switch (CostSource)
            {
                case InventoryCostSource.SytelineCostReport:
                    GetInventoryCostsFromSytelineCostReport();
                    AddItemInfo(mStrBldr.ToString().AddSingleQuotes());
                    break;
                case InventoryCostSource.SytelineItemWhse:
                    GetInventoryCostsFromSytelineItemWhse();
                    break;
                case InventoryCostSource.FactTrak:
                    if (dtmStartDate.HasValue && dtmEndDate.HasValue)
                        GetInventoryCostsFromFactTrak((DateTime)dtmStartDate, (DateTime)dtmEndDate);
                    break;
            }
            PopulateInventoryCostMetrics();
        }

        private void PopulateInventoryCostMetrics()
        {
            InventoryCostMetrics = new List<InventoryCostMetric>();

            InventoryCostMetrics.AddRange(this
                .GroupBy(c => new {c.Warehouse, c.Source})
                .Select(c => new InventoryCostMetric {
                     InventoryType = c.Key.Source,
                     Warehouse = c.Key.Warehouse,
                     Cost = c.Sum(m => m.TotalCost),
                     DateCreated = c.Max(m => m.DateCreated)})
                .ToList());

            AddWIPValueCostMetrics();
        }

        private void AddWIPValueCostMetrics()
        {
            var strSQL = mQueyrDefs.GetQuery("SelectItemWIPRemaining");
            InventoryCostMetrics.AddRange(mSyteLineDb.Database.SqlQuery<WIPCostItem>(strSQL)
                .GroupBy(c => c.whse)
                .Select(c => new InventoryCostMetric
                {
                    Warehouse = c.Key,
                    Cost = c.Sum(m => m.WIP_Remaining),
                    DateCreated = DateTime.Now.Date,
                    InventoryType = "WIP"
                })
                .ToList());
        }

        private void GetInventoryCostsFromFactTrak(DateTime StartDate, DateTime EndDate)
        {
            var FactTrakDb = new FactTrakEntities();
            FactTrakDb.SetCommandTimeOut(0); //sets the timeout value to unlimited...

            var strSQL = mQueyrDefs.GetQuery("SelectFactTrakItemCostsByDate", new string[] { 
                new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0).ToString(), 
                new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59).ToString()});

            var FactTrakItemList = FactTrakDb.Database.SqlQuery<FactTrakCostItem>(strSQL)
                .ToList();

            this.AddRange(FactTrakItemList
                .Select(c => new InventoryCostItem
                {
                    Item = c.Item_Nbr,
                    Source = c.Purch_Manuf,
                    Warehouse = c.Warehouse,
                    TotalCost = c.Whse_Inv_Value,
                    DateCreated = c.Record_Date
                })
                .ToList());
        }

        private void GetInventoryCostsFromSytelineItemWhse()
        {
            this.AddRange(mSyteLineDb.Database
                .SqlQuery<InventoryCostItem>(mQueyrDefs.GetQuery("SelectItemWhseItemCosts")) //SelectItemWhseTypeCosts would give the costs grouped by the SQL Server
                .ToList());
        }

        private void GetInventoryCostsFromSytelineCostReport()
        {
            mStrBldr.Clear();

            var objTempList = mSyteLineDb.Database.SqlQuery<InventoryCost>(
                "Rpt_InventoryCostSp " +
                "@ExbegWhse,@ExendWhse,@ExbegLoc,@ExendLoc,@ExbegProductcode,@ExendProductcode,@ExbegItem,@ExendItem,@ExOptgoItemStat,@ExOptgoMatlType,@ExOptprPMTCode," +
                "@ExOptszStocked,@ExOptacAbcCode,@ExOptprPrZeroQty,@ShowDetail,@PrintCost,@DisplayHeader,@PMessageLanguage",
                new SqlParameter("ExbegWhse", DBNull.Value),
                new SqlParameter("ExendWhse", DBNull.Value),
                new SqlParameter("ExbegLoc", DBNull.Value),
                new SqlParameter("ExendLoc", DBNull.Value),
                new SqlParameter("ExbegProductcode", DBNull.Value),
                new SqlParameter("ExendProductcode", DBNull.Value),
                new SqlParameter("ExbegItem", DBNull.Value),
                new SqlParameter("ExendItem", DBNull.Value),

                new SqlParameter("ExOptgoItemStat", "ASO"),
                new SqlParameter("ExOptgoMatlType", "MTFO"),
                new SqlParameter("ExOptprPMTCode", "MPT"),
                new SqlParameter("ExOptszStocked", "B"),
                new SqlParameter("ExOptacAbcCode", "ABC"),
                new SqlParameter("ExOptprPrZeroQty", "0"),
                new SqlParameter("ShowDetail", "0"),
                new SqlParameter("PrintCost", "1"),
                new SqlParameter("DisplayHeader", "1"),
                new SqlParameter("PMessageLanguage", "1033"));

            mSyteLineDb.SetCommandTimeOut(0); //sets the timeout value to unlimited...
            foreach (var objItem in objTempList.ToList())
            {
                InventoryCostItem objCostItem = new InventoryCostItem
                {
                    Warehouse = objItem.whse,
                    Location = objItem.loc,
                    Item = objItem.item,
                    QtyOnHand = objItem.qty_on_hand,
                    UnitCost = objItem.cpr_cost,
                    UnitPrice = objItem.cpr_price,
                    TotalCost = objItem.amt_cost,
                    TotalPrice = objItem.amt_price,
                    ItemDescription = objItem.item_desc,
                    UnitOfMeasure = objItem.u_m_desc,
                    Source = objItem.pmt_code,
                    ProductCode = objItem.product_code,
                    TotalMaterialCost = objItem.matl_cost,
                    TotalLaborCost = objItem.lbr_cost,
                    FixedOvhdCost = objItem.fovhd_cost,
                    VariableOvhdCost = objItem.vovhd_cost,
                    OutsideCost = objItem.out_cost,
                    DateCreated = DateTime.Now.Date
                };
                this.Add(objCostItem);
                mStrBldr.Append(objCostItem.Item + ", ");
            }
        }

        private void AddItemInfo(string strItemList)
        {

            if (mStrBldr.Length == 0)//prevents an error in the scenario where no records are returned...
                mStrBldr.Append("''");

            var objItemList = mSyteLineDb.Database.SqlQuery<Item>(
                mQueyrDefs.GetQuery("SelectItemByItemList", new string[] { strItemList }))
                .ToList();
            var objItemWhseList = mSyteLineDb.Database.SqlQuery<ItemWhse>(
                mQueyrDefs.GetQuery("SelectItemWhseByItemList", new string[] { strItemList }))
                .ToList();
            var objGroupedItemWhseList = objItemWhseList
                .GroupBy(i => i.item)
                .Select(i => new ItemWhse { item = i.Key, qty_pur_ytd = i.Sum(w => w.qty_pur_ytd), qty_sold_ytd = i.Sum(w => w.qty_sold_ytd) })
                .ToList();
            var objMatlTranList = mSyteLineDb.Database.SqlQuery<MatlTran>(
                mQueyrDefs.GetQuery("SelectRecentMatlTranByItemList", new string[] { strItemList }))
                .ToList();


            foreach (var objInventoryCostItem in this)
            {
                var objItem = objItemList
                    .Where(i => i.item.Equals(objInventoryCostItem.Item))
                    .DefaultIfEmpty(new Item { item = "EmptyItem", qty_mfg_ytd = 0, qty_used_ytd = 0 })
                    .SingleOrDefault();
                var objItemWhse = objGroupedItemWhseList
                    .Where(i => i.item.Equals(objInventoryCostItem.Item))
                    .DefaultIfEmpty(new ItemWhse { item = "EmptyItem", qty_pur_ytd = 0, qty_sold_ytd = 0 })
                    .SingleOrDefault();
                var objMatlTran = objMatlTranList
                    .Where(i => i.item.Equals(objInventoryCostItem.Item))
                    .DefaultIfEmpty(new MatlTran { item = "EmptyItem", trans_date = DateTime.MinValue })
                    .SingleOrDefault();

                objInventoryCostItem.QtyManufacturedYTD = objItem.qty_mfg_ytd;
                objInventoryCostItem.QtyUsedYTD = objItem.qty_used_ytd;
                objInventoryCostItem.QtyPurchasedYTD = objItemWhse.qty_pur_ytd;
                objInventoryCostItem.QtySoldYTD = objItemWhse.qty_sold_ytd;
                objInventoryCostItem.LastTransactionDate = objMatlTran.trans_date;
            }

        }
    }
}
