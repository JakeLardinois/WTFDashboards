//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashboardDataGatherer
{
    using System;
    using System.Collections.Generic;
    
    public partial class jobtran
    {
        public decimal trans_num { get; set; }
        public string job { get; set; }
        public Nullable<short> suffix { get; set; }
        public string trans_type { get; set; }
        public Nullable<System.DateTime> trans_date { get; set; }
        public Nullable<decimal> qty_complete { get; set; }
        public Nullable<decimal> qty_scrapped { get; set; }
        public int oper_num { get; set; }
        public Nullable<decimal> a_hrs { get; set; }
        public Nullable<int> next_oper { get; set; }
        public string emp_num { get; set; }
        public Nullable<decimal> a__ { get; set; }
        public Nullable<int> start_time { get; set; }
        public Nullable<int> end_time { get; set; }
        public string ind_code { get; set; }
        public string pay_rate { get; set; }
        public Nullable<decimal> qty_moved { get; set; }
        public string whse { get; set; }
        public string loc { get; set; }
        public string user_code { get; set; }
        public Nullable<byte> close_job { get; set; }
        public Nullable<byte> issue_parent { get; set; }
        public string lot { get; set; }
        public Nullable<byte> complete_op { get; set; }
        public Nullable<decimal> pr_rate { get; set; }
        public Nullable<decimal> job_rate { get; set; }
        public string shift { get; set; }
        public byte posted { get; set; }
        public Nullable<byte> low_level { get; set; }
        public Nullable<byte> backflush { get; set; }
        public string reason_code { get; set; }
        public string trans_class { get; set; }
        public string ps_num { get; set; }
        public string wc { get; set; }
        public Nullable<byte> awaiting_eop { get; set; }
        public Nullable<decimal> fixovhd { get; set; }
        public Nullable<decimal> varovhd { get; set; }
        public string cost_code { get; set; }
        public Nullable<byte> co_product_mix { get; set; }
        public byte NoteExistsFlag { get; set; }
        public System.DateTime RecordDate { get; set; }
        public System.Guid RowPointer { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public byte InWorkflow { get; set; }
        public string import_doc_id { get; set; }
        public string container_num { get; set; }
        public string uf_pref_type { get; set; }
    }
}