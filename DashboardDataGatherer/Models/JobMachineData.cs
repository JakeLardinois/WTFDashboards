using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardDataGatherer.Models
{
    public class JobMachineTransaction
    {
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string WorkCenter { get; set; }
        public Nullable<decimal> MachineHours { get; set; }
        public Nullable<decimal> FixedOverhead { get; set; }
        public Nullable<decimal> VariableOverhead { get; set; }
    }

    public class JobMachineTransactions : List<JobMachineTransaction>
    {
        public QueryDefinitions mQueryDefs { get; set; }


        public JobMachineTransactions(DateTime StartDate, int DaysCount)
            : base()
        {
            var db = new SytelineDbEntities();
            mQueryDefs = new QueryDefinitions();

            this.AddRange(db.Database
                .SqlQuery<JobMachineTransaction>(mQueryDefs.GetQuery(
                    "SelectNMachineTransactions", new string[] {StartDate.Date.ToString(), DaysCount.ToString() }))
                .ToList());

        }

        public void UploadNewData()
        {
            var db = new DashboardDbEntities();


            db.Database.ExecuteSqlCommand(mQueryDefs.GetQuery("DeleteAllMachineHourMetrics"));

            foreach (var objJobMachineTransaction in this)
                db.MachineHourMetrics.Add(new MachineHourMetric {
                    TransactionDate = objJobMachineTransaction.TransactionDate.HasValue ? (DateTime)objJobMachineTransaction.TransactionDate : SharedVariables.MINDATE,
                    WorkCenter = objJobMachineTransaction.WorkCenter,
                    MachineHours = objJobMachineTransaction.MachineHours,
                    FixedOverhead = objJobMachineTransaction.FixedOverhead,
                    VariableOverhead = objJobMachineTransaction.VariableOverhead
                });

            db.SaveChanges();
        }
    }
}
